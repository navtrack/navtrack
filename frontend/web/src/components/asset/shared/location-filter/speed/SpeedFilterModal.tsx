import { faTachometerAlt } from "@fortawesome/free-solid-svg-icons";
import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { FormikTextInput } from "../../../../ui/form/text-input/FormikTextInput";
import { Modal } from "../../../../ui/modal/Modal";
import { TextInputRightAddon } from "../../../../ui/form/text-input/TextInputRightAddon";
import { FilterModal } from "../FilterModal";
import { SpeedFilterFormValues } from "../locationFilterTypes";
import { useCurrentUnits } from "@navtrack/shared/hooks/util/useCurrentUnits";
import { nameOf } from "@navtrack/shared/utils/typescript";
import { useAtom } from "jotai";
import { useCallback } from "react";
import { isNumeric } from "@navtrack/shared/utils/numbers";
import { averageSpeedFilterAtom, speedFilterAtom } from "../locationFilterState";

type SpeedFilterModalProps = {
  filterKey: string;
};

export function SpeedFilterModal(props: SpeedFilterModalProps) {
  return (
    <SpeedFilterModalBase
      filterAtom={speedFilterAtom}
      filterKey={props.filterKey}
      titleId="locations.filter.speed.title"
    />
  );
}

export function AverageSpeedFilterModal(props: SpeedFilterModalProps) {
  return (
    <SpeedFilterModalBase
      filterAtom={averageSpeedFilterAtom}
      filterKey={props.filterKey}
      titleId="locations.filter.avg-speed.title"
    />
  );
}

type SpeedFilterModalBaseProps = SpeedFilterModalProps & {
  filterAtom: typeof speedFilterAtom;
  titleId: string;
};

function SpeedFilterModalBase(props: SpeedFilterModalBaseProps) {
  const units = useCurrentUnits();
  const [state, setState] = useAtom(props.filterAtom(props.filterKey));

  const handleChange = useCallback(
    (
      e: React.ChangeEvent<HTMLInputElement>,
      field: string,
      setFieldValue: (
        field: string,
        value: any,
        shouldValidate?: boolean | undefined
      ) => void
    ) => {
      if (isNumeric(e.target.value)) {
        setFieldValue(field, e.target.value);
      }
    },
    []
  );

  return (
    <Modal
      open={state.open}
      close={() => setState((prev) => ({ ...prev, open: false }))}>
      <Formik<SpeedFilterFormValues>
        initialValues={state}
        onSubmit={(values: SpeedFilterFormValues) => {
          setState({
            ...state,
            minSpeed: values.minSpeed,
            maxSpeed: values.maxSpeed,
            active: !!values.minSpeed || !!values.maxSpeed,
            open: false
          });
        }}>
        {({ values, setFieldValue }) => (
          <Form>
            <FilterModal
              icon={faTachometerAlt}
              className="max-w-sm"
              onCancel={() => setState((prev) => ({ ...prev, open: false }))}>
              <h3 className="text-lg font-medium leading-6 text-gray-900">
                <FormattedMessage id={props.titleId} />
              </h3>
              <div className="mt-2">
                <div className="flex space-x-4">
                  <FormikTextInput
                    name={nameOf<SpeedFilterFormValues>("minSpeed")}
                    label="generic.min"
                    value={values.minSpeed}
                    type="number"
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
                      if (
                        values.maxSpeed !== undefined &&
                        newValue > values.maxSpeed
                      ) {
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
                    type="number"
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
                      if (
                        values.minSpeed !== undefined &&
                        newValue < values.minSpeed
                      ) {
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
