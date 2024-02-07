import { faTachometerAlt } from "@fortawesome/free-solid-svg-icons";
import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { FormikTextInput } from "../../../../ui/form/text-input/FormikTextInput";
import { Modal } from "../../../../ui/modal/Modal";
import { TextInputRightAddon } from "../../../../ui/form/text-input/TextInputRightAddon";
import { FilterModal } from "../FilterModal";
import { SpeedFilterFormValues } from "../locationFilterTypes";
import { useSpeedFilter } from "./useSpeedFilter";
import { useCurrentUnits } from "@navtrack/shared/hooks/util/useCurrentUnits";
import { nameOf } from "@navtrack/shared/utils/typescript";

type SpeedFilterModalProps = {
  average?: boolean;
  filterKey: string;
};

export function SpeedFilterModal(props: SpeedFilterModalProps) {
  const units = useCurrentUnits();
  const { handleSubmit, close, handleChange, state, initialValues } =
    useSpeedFilter(props.filterKey);

  return (
    <Modal open={state.open} close={close}>
      <Formik<SpeedFilterFormValues>
        initialValues={initialValues}
        onSubmit={handleSubmit}>
        {({ values, setFieldValue }) => (
          <Form>
            <FilterModal
              icon={faTachometerAlt}
              className="max-w-sm"
              onCancel={close}>
              <h3 className="text-lg font-medium leading-6 text-gray-900">
                <FormattedMessage
                  id={
                    props.average
                      ? "locations.filter.avg-speed.title"
                      : "locations.filter.speed.title"
                  }
                />
              </h3>
              <div className="mt-2">
                <div className="flex space-x-4">
                  <FormikTextInput
                    name={nameOf<SpeedFilterFormValues>("minSpeed")}
                    label="generic.min"
                    value={values.minSpeed}
                    rightAddon={
                      <TextInputRightAddon>{units.speed}</TextInputRightAddon>
                    }
                    onChange={(e) =>
                      handleChange(
                        e,
                        nameOf<SpeedFilterFormValues>("minSpeed"),
                        setFieldValue
                      )
                    }
                    onBlur={(e) => {
                      const newValue = parseInt(e.target.value);
                      if (newValue > parseInt(values.maxSpeed)) {
                        setFieldValue(
                          nameOf<SpeedFilterFormValues>("maxSpeed"),
                          newValue
                        );
                      }
                    }}
                  />
                  <FormikTextInput
                    name={nameOf<SpeedFilterFormValues>("maxSpeed")}
                    label="generic.max"
                    value={values.maxSpeed}
                    rightAddon={
                      <TextInputRightAddon>{units.speed}</TextInputRightAddon>
                    }
                    onChange={(e) =>
                      handleChange(
                        e,
                        nameOf<SpeedFilterFormValues>("maxSpeed"),
                        setFieldValue
                      )
                    }
                    onBlur={(e) => {
                      const newValue = parseInt(e.target.value);
                      if (newValue < parseInt(values.minSpeed)) {
                        setFieldValue(
                          nameOf<SpeedFilterFormValues>("minSpeed"),
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
