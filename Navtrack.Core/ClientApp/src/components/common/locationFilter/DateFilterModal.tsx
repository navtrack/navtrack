import moment, { Moment } from "moment";
import React, { useState } from "react";
import Configuration from "../../../services/Configuration";
import useClickOutside from "../../../services/hooks/useClickOutside";
import Button from "../../shared/elements/Button";
import DatePicker from "../../shared/elements/Calendar";
import Checkbox from "../../shared/elements/Checkbox";
import Modal from "../../shared/elements/Modal";
import TextInput from "../../shared/forms/TextInput";
import Icon from "../../shared/util/Icon";
import { DateFilterModel } from "./types/DateFilterModel";
import { DateFilterType } from "./types/DateFilterType";
import { NumberFilterType } from "./types/NumberFilterType";

type Props = {
  dateFilter: DateFilterModel;
  setDateFilter: (dateFilter: DateFilterModel) => void;
  closeModal: () => void;
};

export default function DateFilterModal(props: Props) {
  const [filter, setFilter] = useState(props.dateFilter);

  const [
    sngleDatePickerVisible,
    showSingleDateDatePicker,
    hideSingleDateDatePicker
  ] = useClickOutside();
  const [startDatePickerVisible, showStartDatePicker, hideStartDatePicker] = useClickOutside();
  const [endDatePickerVisible, showEndDatePicker, hideEndDatePicker] = useClickOutside();

  const saveFilter = () => {
    const dateFilters: Record<number, DateFilterModel> = {
      [DateFilterType.Today]: {
        ...filter,
        singleDate: moment(),
        startDate: getDate(moment()).startOf("day"),
        endDate: getDate(moment()).endOf("day"),
        numberFilterType: NumberFilterType.Single,
        enabled: true
      },
      [DateFilterType.Last7Days]: {
        ...filter,
        startDate: getDate(moment()).startOf("day").subtract(6, "day"),
        endDate: getDate(moment()).endOf("day"),
        numberFilterType: NumberFilterType.Interval,
        enabled: true
      },
      [DateFilterType.Last28Days]: {
        ...filter,
        startDate: getDate(moment()).subtract(27, "day").startOf("day"),
        endDate: getDate(moment()).endOf("day"),
        numberFilterType: NumberFilterType.Interval,
        enabled: true
      },
      [DateFilterType.CurrentMonth]: {
        ...filter,
        startDate: getDate(moment()).startOf("month").startOf("day"),
        endDate: getDate(moment()).endOf("month").endOf("day"),
        numberFilterType: NumberFilterType.Interval,
        enabled: true
      },
      [DateFilterType.SingleDay]: {
        ...filter,
        startDate: getDate(filter.singleDate).startOf("day"),
        endDate: getDate(filter.singleDate).endOf("day"),
        numberFilterType: NumberFilterType.Single,
        enabled: true
      },
      [DateFilterType.Interval]: {
        ...filter,
        startDate: getDate(filter.intervalStartDate).startOf("day"),
        endDate: getDate(filter.intervalEndDate).endOf("day"),
        numberFilterType: NumberFilterType.Interval,
        enabled: true
      }
    };

    props.setDateFilter({ ...filter, ...dateFilters[filter.dateFilterType] });
    props.closeModal();
  };

  const hideAll = () => {
    hideSingleDateDatePicker();
    hideStartDatePicker();
    hideEndDatePicker();
  };

  const setRangeStartDate = (date: Moment) => {
    setFilter({ ...filter, intervalStartDate: date });
    if (date > filter.intervalEndDate) {
      setFilter({ ...filter, intervalEndDate: date });
    }
  };
  const setRangeEndDate = (date: Moment) => {
    setFilter({ ...filter, intervalEndDate: date });
    if (date < filter.intervalStartDate) {
      setFilter({ ...filter, intervalStartDate: date });
    }
  };

  return (
    <Modal closeModal={props.closeModal} onContentClick={hideAll}>
      <div className="font-medium text-lg mb-3">
        <Icon className="fa-calendar-alt mr-1" />
        Date filter
      </div>
      <div className="flex flex-col cursor-default text-sm">
        <Checkbox
          className="mb-1"
          checked={filter.dateFilterType === DateFilterType.Today}
          readOnly
          onClick={() => setFilter({ ...filter, dateFilterType: DateFilterType.Today })}>
          Today
        </Checkbox>
        <Checkbox
          className="mb-1"
          checked={filter.dateFilterType === DateFilterType.Last7Days}
          readOnly
          onClick={() => setFilter({ ...filter, dateFilterType: DateFilterType.Last7Days })}>
          Last 7 days
        </Checkbox>
        <Checkbox
          className="mb-1"
          checked={filter.dateFilterType === DateFilterType.Last28Days}
          readOnly
          onClick={() => setFilter({ ...filter, dateFilterType: DateFilterType.Last28Days })}>
          Last 28 days
        </Checkbox>
        <Checkbox
          className="mb-1"
          checked={filter.dateFilterType === DateFilterType.CurrentMonth}
          readOnly
          onClick={() => setFilter({ ...filter, dateFilterType: DateFilterType.CurrentMonth })}>
          Current month
        </Checkbox>
        <Checkbox
          className="mb-1"
          checked={filter.dateFilterType === DateFilterType.SingleDay}
          readOnly
          onClick={() => setFilter({ ...filter, dateFilterType: DateFilterType.SingleDay })}>
          Single day
        </Checkbox>
        <div
          className="mb-1 flex flex-row items-center"
          onClick={() => setFilter({ ...filter, dateFilterType: DateFilterType.SingleDay })}>
          <div className="ml-5">
            <div className="text-sm relative">
              <TextInput
                type="text"
                value={filter.singleDate.format(Configuration.dateFormat)}
                onClick={(e) => {
                  setFilter({ ...filter, dateFilterType: DateFilterType.SingleDay });
                  hideAll();
                  showSingleDateDatePicker(e);
                }}
                readOnly
              />
              {sngleDatePickerVisible && (
                <div className="absolute bottom-0 mb-10 fadeIn animated faster">
                  <DatePicker
                    date={filter.singleDate}
                    setDate={(e) => setFilter({ ...filter, singleDate: e })}
                    hide={hideSingleDateDatePicker}
                  />
                </div>
              )}
            </div>
          </div>
        </div>
        <Checkbox
          className="mb-1"
          checked={filter.dateFilterType === DateFilterType.Interval}
          readOnly
          onClick={() => setFilter({ ...filter, dateFilterType: DateFilterType.Interval })}>
          Interval
        </Checkbox>
        <div
          className="mb-1 flex flex-row items-center mr"
          onClick={() => setFilter({ ...filter, dateFilterType: DateFilterType.Interval })}>
          <div className="ml-5">
            <div className="text-xs uppercase font-semibold text-gray-700">Start date</div>
            <div className="text-sm relative">
              <TextInput
                type="text"
                value={filter.intervalStartDate.format(Configuration.dateFormat)}
                onClick={(e) => {
                  setFilter({ ...filter, dateFilterType: DateFilterType.Interval });
                  hideAll();
                  showStartDatePicker(e);
                }}
                readOnly
              />
              {startDatePickerVisible && (
                <div className="absolute bottom-0 mb-10 fadeIn animated faster">
                  <DatePicker
                    date={filter.intervalStartDate}
                    setDate={setRangeStartDate}
                    hide={hideStartDatePicker}
                  />
                </div>
              )}
            </div>
          </div>
          <div className="ml-3">
            <div className="text-xs uppercase font-semibold text-gray-700">End date</div>
            <div className="text-sm relative">
              <TextInput
                type="text"
                value={filter.intervalEndDate.format(Configuration.dateFormat)}
                onClick={(e) => {
                  setFilter({ ...filter, dateFilterType: DateFilterType.Interval });
                  hideAll();
                  showEndDatePicker(e);
                }}
                readOnly
              />
              {endDatePickerVisible && (
                <div className="absolute bottom-0 mb-10 fadeIn animated faster">
                  <DatePicker
                    date={filter.intervalEndDate}
                    setDate={setRangeEndDate}
                    hide={hideEndDatePicker}
                  />
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
      <div className="flex justify-end mt-3">
        <Button color="secondary" onClick={props.closeModal} className="mr-3">
          Cancel
        </Button>
        <Button color="primary" onClick={saveFilter}>
          Save
        </Button>
      </div>
    </Modal>
  );
}

function getDate(date: Moment): Moment {
  return moment().utc().year(date.year()).month(date.month()).date(date.date());
}
