import { faMountain } from "@fortawesome/free-solid-svg-icons";
import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { FormikTextInput } from "../../../../ui/form/text-input/FormikTextInput";
import { Modal } from "../../../../ui/modal/Modal";
import { TextInputRightAddon } from "../../../../ui/form/text-input/TextInputRightAddon";
import { FilterModal } from "../FilterModal";
import { AltitudeFilterFormValues } from "../locationFilterTypes";
import { useCurrentUnits } from "@navtrack/shared/hooks/util/useCurrentUnits";
import { nameOf } from "@navtrack/shared/utils/typescript";
import { number, object, ObjectSchema } from "yup";
import { useAtom } from "jotai";
import { altitudeFilterAtom } from "../locationFilterState";
import { useCallback } from "react";

type AltitudeFilterModalProps = {
  average?: boolean;
  filterKey: string;
};

export function AltitudeFilterModal(props: AltitudeFilterModalProps) {
  const units = useCurrentUnits();
  const [state, setState] = useAtom(altitudeFilterAtom(props.filterKey));

  const handleSubmit = useCallback(
    (values: AltitudeFilterFormValues) => {
      const minAltitude =
        values.minAltitude !== undefined ? values.minAltitude : undefined;
      const maxAltitude =
        values.maxAltitude !== undefined ? values.maxAltitude : undefined;
      setState({
        ...state,
        minAltitude:
          minAltitude !== undefined && isNaN(minAltitude)
            ? undefined
            : minAltitude,
        maxAltitude:
          maxAltitude !== undefined && isNaN(maxAltitude)
            ? undefined
            : maxAltitude,
        active: !!values.minAltitude || !!values.maxAltitude,
        open: false
      });
    },
    [setState, state]
  );

  const validationSchema: ObjectSchema<AltitudeFilterFormValues> = object({
    minAltitude: number().typeError("number.required"),
    maxAltitude: number().typeError("number.required")
  }).defined();

  return (
    <Modal
      open={state.open}
      close={() => setState((prev) => ({ ...prev, open: false }))}>
      <Formik<AltitudeFilterFormValues>
        initialValues={state}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}>
        {({ values, setFieldValue }) => (
          <Form>
            <FilterModal
              icon={faMountain}
              className="max-w-sm"
              onCancel={() => setState((prev) => ({ ...prev, open: false }))}>
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
                    label="min"
                    value={values.minAltitude}
                    type="number"
                    rightAddon={
                      <TextInputRightAddon>{units.length}</TextInputRightAddon>
                    }
                    onBlur={(e) => {
                      const newValue = parseInt(e.target.value);
                      if (
                        values.maxAltitude !== undefined &&
                        newValue > values.maxAltitude
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
                    label="max"
                    value={values.maxAltitude}
                    type="number"
                    rightAddon={
                      <TextInputRightAddon>{units.length}</TextInputRightAddon>
                    }
                    onBlur={(e) => {
                      const newValue = parseInt(e.target.value);
                      if (
                        values.minAltitude !== undefined &&
                        newValue < values.minAltitude
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
