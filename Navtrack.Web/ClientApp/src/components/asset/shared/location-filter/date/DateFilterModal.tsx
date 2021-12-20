import { faCalendarAlt } from "@fortawesome/free-regular-svg-icons";
import { RadioGroup } from "@headlessui/react";
import classNames from "classnames";
import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { nameOf } from "../../../../../utils/typescript";
import DatePicker from "../../../../ui/shared/date-picker/DatePicker";
import Modal from "../../../../ui/shared/modal/Modal";
import FilterModal from "../FilterModal";
import { DateFilter, DateRange } from "../types";
import { dateOptions } from "./date-options";
import useDateFilter from "./useDateFilter";

interface IDateFilterModal {
  filterKey: string;
}

export default function DateFilterModal(props: IDateFilterModal) {
  const { state, close, handleSubmit } = useDateFilter(props);

  return (
    <Modal open={state.open} close={close}>
      <Formik<DateFilter> initialValues={state} onSubmit={handleSubmit}>
        {({ values, setFieldValue }) => (
          <Form>
            <FilterModal icon={faCalendarAlt} onCancel={close}>
              <h3 className="text-lg leading-6 font-medium text-gray-900">
                <FormattedMessage id="locations.filter.date.title" />
              </h3>
              <div className="mt-2 flex flex-col">
                <div className="inline-block">
                  <RadioGroup
                    value={values.range}
                    onChange={(range) => setFieldValue(nameOf<DateFilter>("range"), range)}
                    className="bg-white rounded-md -space-y-px flex flex-col w-40">
                    {dateOptions.map((setting, settingIdx) => (
                      <RadioGroup.Option
                        as={"div"}
                        key={setting.range}
                        value={setting.range}
                        className={({ checked }) =>
                          classNames(
                            "py-1 flex relative  cursor-pointer focus:outline-none",
                            settingIdx === dateOptions.length - 1
                              ? "rounded-bl-md rounded-br-md"
                              : "",
                            checked ? "" : "border-gray-200"
                          )
                        }>
                        {({ active, checked }) => (
                          <>
                            <span
                              className={classNames(
                                checked
                                  ? "bg-gray-800 border-transparent"
                                  : "bg-white border-gray-300",
                                active ? "ring-2 ring-offset-2 ring-gray-600" : "",
                                "h-4 w-4 mt-0.5 rounded-full border flex items-center justify-center"
                              )}>
                              <span className="rounded-full bg-white w-1.5 h-1.5" />
                            </span>
                            <span className="ml-3 flex flex-col">
                              <RadioGroup.Label
                                as="span"
                                className={classNames(
                                  "text-gray-900",
                                  "block text-sm font-medium"
                                )}>
                                <FormattedMessage id={setting.name} />
                              </RadioGroup.Label>
                            </span>
                          </>
                        )}
                      </RadioGroup.Option>
                    ))}
                  </RadioGroup>
                </div>
                <div className="flex gap-x-4">
                  <DatePicker
                    value={values.startDate ?? new Date()}
                    disabled={values.range !== DateRange.Custom}
                    onChange={(date) => {
                      if (date) {
                        setFieldValue(nameOf<DateFilter>("startDate"), date);

                        if (values.startDate !== undefined && date > values.startDate) {
                          setFieldValue(nameOf<DateFilter>("endDate"), date);
                        }
                      }
                    }}
                  />
                  <DatePicker
                    value={values.endDate ?? new Date()}
                    disabled={values.range !== DateRange.Custom}
                    onChange={(date) => {
                      if (date) {
                        setFieldValue(nameOf<DateFilter>("endDate"), date);

                        if (values.startDate !== undefined && date < values.startDate) {
                          setFieldValue(nameOf<DateFilter>("startDate"), date);
                        }
                      }
                    }}
                  />
                </div>
              </div>
            </FilterModal>
          </Form>
        )}
      </Formik>
    </Modal>
  );
}
