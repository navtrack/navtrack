import { faClock } from "@fortawesome/free-regular-svg-icons";
import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { FormikTextInput } from "../../../../ui/form/text-input/FormikTextInput";
import { Modal } from "../../../../ui/modal/Modal";
import { TextInputRightAddon } from "../../../../ui/form/text-input/TextInputRightAddon";
import { FilterModal } from "../FilterModal";
import { DurationFilterFormValues } from "../locationFilterTypes";
import { useCurrentUnits } from "@navtrack/shared/hooks/util/useCurrentUnits";
import { nameOf } from "@navtrack/shared/utils/typescript";
import { useAtom } from "jotai";
import { useCallback } from "react";
import { number, object, ObjectSchema } from "yup";
import { durationFilterAtom } from "../locationFilterState";

type DurationFilterModalProps = {
  filterKey: string;
};

export function DurationFilterModal(props: DurationFilterModalProps) {
  const units = useCurrentUnits();
  const [state, setState] = useAtom(durationFilterAtom(props.filterKey));

  const handleSubmit = useCallback(
    (values: DurationFilterFormValues) => {
      setState({
        ...state,
        minDuration:
          values.minDuration !== undefined && isNaN(values.minDuration)
            ? undefined
            : values.minDuration,
        maxDuration:
          values.maxDuration !== undefined && isNaN(values.maxDuration)
            ? undefined
            : values.maxDuration,
        active: !!values.minDuration || !!values.maxDuration,
        open: false
      });
    },
    [setState, state]
  );

  const validationSchema: ObjectSchema<DurationFilterFormValues> = object({
    minDuration: number().typeError("generic.number.required"),
    maxDuration: number().typeError("generic.number.required")
  }).defined();

  return (
    <Modal
      open={state.open}
      close={() => setState((prev) => ({ ...prev, open: false }))}>
      <Formik<DurationFilterFormValues>
        initialValues={state}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}>
        {({ values, setFieldValue }) => (
          <Form>
            <FilterModal
              icon={faClock}
              className="max-w-sm"
              onCancel={() => setState((prev) => ({ ...prev, open: false }))}>
              <h3 className="text-lg font-medium leading-6 text-gray-900">
                <FormattedMessage id="locations.filter.duration.title" />
              </h3>
              <div className="mt-2">
                <div className="flex space-x-4">
                  <FormikTextInput
                    name={nameOf<DurationFilterFormValues>("minDuration")}
                    label="generic.min"
                    value={values.minDuration}
                    type="number"
                    rightAddon={
                      <TextInputRightAddon>{units.length}</TextInputRightAddon>
                    }
                    onBlur={(e) => {
                      const newValue = parseInt(e.target.value);
                      if (
                        !!values.maxDuration &&
                        newValue > values.maxDuration
                      ) {
                        setFieldValue(
                          nameOf<DurationFilterFormValues>("maxDuration"),
                          newValue
                        );
                      }
                    }}
                  />
                  <FormikTextInput
                    name={nameOf<DurationFilterFormValues>("maxDuration")}
                    label="generic.max"
                    value={values.maxDuration}
                    type="number"
                    rightAddon={
                      <TextInputRightAddon>{units.length}</TextInputRightAddon>
                    }
                    onBlur={(e) => {
                      const newValue = parseInt(e.target.value);
                      if (
                        !!values.minDuration &&
                        newValue < values.minDuration
                      ) {
                        setFieldValue(
                          nameOf<DurationFilterFormValues>("minDuration"),
                          newValue
                        );
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
