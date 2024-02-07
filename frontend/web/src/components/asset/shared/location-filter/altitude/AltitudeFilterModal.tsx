import { faMountain } from "@fortawesome/free-solid-svg-icons";
import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { FormikTextInput } from "../../../../ui/form/text-input/FormikTextInput";
import { Modal } from "../../../../ui/modal/Modal";
import { TextInputRightAddon } from "../../../../ui/form/text-input/TextInputRightAddon";
import { FilterModal } from "../FilterModal";
import { AltitudeFilterFormValues } from "../locationFilterTypes";
import { useAltitudeFilter } from "./useAltitudeFilter";
import { useAltitudeFilterFormValidation } from "./useAltitudeFilterFormValidation";
import { useCurrentUnits } from "@navtrack/shared/hooks/util/useCurrentUnits";
import { nameOf } from "@navtrack/shared/utils/typescript";

type AltitudeFilterModalProps = {
  average?: boolean;
  filterKey: string;
};

export function AltitudeFilterModal(props: AltitudeFilterModalProps) {
  const units = useCurrentUnits();
  const validationSchema = useAltitudeFilterFormValidation();
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
                <FormattedMessage
                  id={
                    props.average
                      ? "locations.filter.avg-altitude.title"
                      : "locations.filter.altitude.title"
                  }
                />
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
                      if (
                        values.maxAltitude !== undefined &&
                        newValue > parseInt(values.maxAltitude)
                      ) {
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
                      if (
                        values.minAltitude !== undefined &&
                        newValue < parseInt(values.minAltitude)
                      ) {
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
