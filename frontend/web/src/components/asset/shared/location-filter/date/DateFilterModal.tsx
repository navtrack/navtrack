import { faCalendarAlt } from "@fortawesome/free-regular-svg-icons";
import { RadioGroup } from "@headlessui/react";
import { nameOf } from "@navtrack/shared/utils/typescript";
import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { Modal } from "../../../../ui/modal/Modal";
import { FilterModal } from "../FilterModal";
import { DateFilter, DateRange } from "../locationFilterTypes";
import { dateOptions } from "./dateOptions";
import { classNames } from "@navtrack/shared/utils/tailwind";
import { FormikDatePicker } from "../../../../ui/datepicker/FormikDatePicker";
import { useCallback } from "react";
import { useRecoilState } from "recoil";
import { dateFilterAtom } from "../locationFilterState";

type DateFilterModalProps = {
  filterKey: string;
};

export function DateFilterModal(props: DateFilterModalProps) {
  const [state, setState] = useRecoilState(dateFilterAtom(props.filterKey));

  const close = useCallback(
    () => setState((x) => ({ ...x, open: false })),
    [setState]
  );

  const handleSubmit = useCallback(
    (values: DateFilter) => {
      setState((x) => ({
        startDate: values.startDate,
        endDate: values.endDate,
        range: values.range,
        open: false
      }));
    },
    [setState]
  );

  return (
    <Modal open={state.open} close={close}>
      <Formik<DateFilter> initialValues={state} onSubmit={handleSubmit}>
        {({ values, setFieldValue }) => (
          <Form>
            <FilterModal icon={faCalendarAlt} onCancel={close}>
              <h3 className="text-lg font-medium leading-6 text-gray-900">
                <FormattedMessage id="locations.filter.date.title" />
              </h3>
              <div className="mt-2 flex flex-col">
                <div className="inline-block">
                  <RadioGroup
                    value={values.range}
                    onChange={(range) => {
                      setFieldValue(nameOf<DateFilter>("range"), range);

                      const dateOption = dateOptions.find(
                        (x) => x.range === range
                      );

                      if (
                        dateOption !== undefined &&
                        dateOption?.range !== DateRange.Custom
                      ) {
                        setFieldValue(
                          nameOf<DateFilter>("startDate"),
                          dateOption.startDate
                        );
                        setFieldValue(
                          nameOf<DateFilter>("endDate"),
                          dateOption.endDate
                        );
                      }
                    }}
                    className="flex w-40 flex-col -space-y-px rounded-md bg-white">
                    {dateOptions.map((setting, settingIdx) => (
                      <RadioGroup.Option
                        as={"div"}
                        key={setting.range}
                        value={setting.range}
                        className={({ checked }) =>
                          classNames(
                            "relative flex cursor-pointer  py-1 focus:outline-none",
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
                                  ? "border-transparent bg-gray-800"
                                  : "border-gray-300 bg-white",
                                active
                                  ? "ring-2 ring-gray-600 ring-offset-2"
                                  : "",
                                "mt-0.5 flex h-4 w-4 items-center justify-center rounded-full border"
                              )}>
                              <span className="h-1.5 w-1.5 rounded-full bg-white" />
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
                <div className="mt-2 flex gap-x-4">
                  <FormikDatePicker
                    name={nameOf<DateFilter>("startDate")}
                    label="generic.from"
                    key={`startDate-${values.startDate?.toString()}`}
                    onChange={(date) => {
                      setFieldValue(
                        nameOf<DateFilter>("range"),
                        DateRange.Custom
                      );
                      setFieldValue(nameOf<DateFilter>("startDate"), date);
                      if (
                        values.startDate !== undefined &&
                        date > values.startDate
                      ) {
                        setFieldValue(nameOf<DateFilter>("endDate"), date);
                      }
                    }}
                  />
                  <FormikDatePicker
                    name={nameOf<DateFilter>("endDate")}
                    label="generic.to"
                    key={`endDate-${values.endDate?.toString()}`}
                    onChange={(date) => {
                      if (date) {
                        setFieldValue(
                          nameOf<DateFilter>("range"),
                          DateRange.Custom
                        );
                        setFieldValue(nameOf<DateFilter>("endDate"), date);
                        if (
                          values.startDate !== undefined &&
                          date < values.startDate
                        ) {
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
