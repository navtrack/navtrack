import { faClock } from "@fortawesome/free-regular-svg-icons";
import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { FormikTextInput } from "../../../../ui/form/text-input/FormikTextInput";
import { Modal } from "../../../../ui/modal/Modal";
import { TextInputRightAddon } from "../../../../ui/form/text-input/TextInputRightAddon";
import { FilterModal } from "../FilterModal";
import { DurationFilterFormValues } from "../locationFilterTypes";
import { useDurationFilter } from "./useDurationFilter";
import { useDurationFilterFormValidation } from "./useDurationFilterFormValidation";
import { useCurrentUnits } from "@navtrack/shared/hooks/util/useCurrentUnits";
import { nameOf } from "@navtrack/shared/utils/typescript";

type DurationFilterModalProps = {
  filterKey: string;
};

export function DurationFilterModal(props: DurationFilterModalProps) {
  const units = useCurrentUnits();
  const validationSchema = useDurationFilterFormValidation();
  const { initialValues, state, close, handleSubmit } = useDurationFilter(
    props.filterKey
  );

  return (
    <Modal open={state.open} close={close}>
      <Formik<DurationFilterFormValues>
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}>
        {({ values, setFieldValue }) => (
          <Form>
            <FilterModal icon={faClock} className="max-w-sm" onCancel={close}>
              <h3 className="text-lg font-medium leading-6 text-gray-900">
                <FormattedMessage id="locations.filter.duration.title" />
              </h3>
              <div className="mt-2">
                <div className="flex space-x-4">
                  <FormikTextInput
                    name={nameOf<DurationFilterFormValues>("minDuration")}
                    label="generic.min"
                    value={values.minDuration}
                    rightAddon={
                      <TextInputRightAddon>{units.length}</TextInputRightAddon>
                    }
                    onBlur={(e) => {
                      const newValue = parseInt(e.target.value);
                      if (newValue > parseInt(values.maxDuration)) {
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
                    rightAddon={
                      <TextInputRightAddon>{units.length}</TextInputRightAddon>
                    }
                    onBlur={(e) => {
                      const newValue = parseInt(e.target.value);
                      if (newValue < parseInt(values.minDuration)) {
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
