import { faTachometerAlt } from "@fortawesome/free-solid-svg-icons";
import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import FormikTextInput from "../../../../ui/shared/text-input/FormikTextInput";
import Modal from "../../../../ui/shared/modal/Modal";
import Slider from "../../../../ui/shared/slider/Slider";
import TextInputRightAddon from "../../../../ui/shared/text-input/TextInputRightAddon";
import FilterModal from "../FilterModal";
import { DEFAULT_MAX_SPEED, SpeedFilterFormValues } from "../types";
import useSpeedFilter from "./useSpeedFilter";
import { useCurrentUnits } from "@navtrack/ui-shared/hooks/util/useCurrentUnits";
import { nameOf } from "@navtrack/ui-shared/utils/typescript";

interface ISpeedFilterModal {
  average?: boolean;
  filterKey: string;
}

export default function SpeedFilterModal(props: ISpeedFilterModal) {
  const units = useCurrentUnits();
  const {
    handleSubmit,
    getSliderValue,
    close,
    handleChange,
    state,
    initialValues
  } = useSpeedFilter(props.filterKey);

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
                <Slider
                  value={getSliderValue(values)}
                  max={DEFAULT_MAX_SPEED} // TODO: get max speed from current asset
                  onChange={(_, speed) => {
                    if (Array.isArray(speed)) {
                      setFieldValue(
                        nameOf<SpeedFilterFormValues>("minSpeed"),
                        speed[0]
                      );
                      setFieldValue(
                        nameOf<SpeedFilterFormValues>("maxSpeed"),
                        speed[1]
                      );
                    }
                  }}
                />
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
