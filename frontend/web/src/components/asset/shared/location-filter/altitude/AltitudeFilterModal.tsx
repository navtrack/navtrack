import { faMountain } from "@fortawesome/free-solid-svg-icons";
import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { FormikTextInput } from "../../../../ui/shared/text-input/FormikTextInput";
import { Modal } from "../../../../ui/shared/modal/Modal";
import { TextInputRightAddon } from "../../../../ui/shared/text-input/TextInputRightAddon";
import { FilterModal } from "../FilterModal";
import { AltitudeFilterFormValues } from "../types";
import { useAltitudeFilter } from "./useAltitudeFilter";
import { useAltitudeFilterFormValuesValidation } from "./useAltitudeFilterFormValuesValidation";
import { useCurrentUnits } from "@navtrack/shared/hooks/util/useCurrentUnits";
import { nameOf } from "@navtrack/shared/utils/typescript";

interface IAltitudeFilterModal {
  average?: boolean;
  filterKey: string;
}

export function AltitudeFilterModal(props: IAltitudeFilterModal) {
  const units = useCurrentUnits();
  const validationSchema = useAltitudeFilterFormValuesValidation();
  const { initialValues, state, close, handleSubmit } = useAltitudeFilter(
    props.filterKey
  );

  return (
    <Modal open={state.open} close={close}>
      <Formik<AltitudeFilterFormValues>
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}>
        {({ values, setFieldValue }) => (
          <Form>
            <FilterModal
              icon={faMountain}
              className="max-w-sm"
              onCancel={close}>
              <h3 className="text-lg font-medium leading-6 text-gray-900">
                <FormattedMessage id="locations.filter.altitude.title" />
              </h3>
              <div className="mt-2">
                <div className="flex space-x-4">
                  <FormikTextInput
                    name={nameOf<AltitudeFilterFormValues>("minAltitude")}
                    label="generic.min"
                    value={values.minAltitude}
                    rightAddon={
                      <TextInputRightAddon>{units.length}</TextInputRightAddon>
                    }
                    onBlur={(e) => {
                      const newValue = parseInt(e.target.value);
                      if (newValue > parseInt(values.maxAltitude)) {
                        setFieldValue(
                          nameOf<AltitudeFilterFormValues>("maxAltitude"),
                          newValue
                        );
                      }
                    }}
                  />
                  <FormikTextInput
                    name={nameOf<AltitudeFilterFormValues>("maxAltitude")}
                    label="generic.max"
                    value={values.maxAltitude}
                    rightAddon={
                      <TextInputRightAddon>{units.length}</TextInputRightAddon>
                    }
                    onBlur={(e) => {
                      const newValue = parseInt(e.target.value);
                      if (newValue < parseInt(values.minAltitude)) {
                        setFieldValue(
                          nameOf<AltitudeFilterFormValues>("minAltitude"),
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
