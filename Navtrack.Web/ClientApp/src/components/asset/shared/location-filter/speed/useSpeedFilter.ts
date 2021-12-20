import { useCallback, useMemo } from "react";
import { useRecoilState } from "recoil";
import { speedFilterAtom } from "../state";
import { DEFAULT_MAX_SPEED, DEFAULT_MIN_SPEED, SpeedFilterFormValues } from "../types";

export default function useSpeedFilter(key: string) {
  const [state, setState] = useRecoilState(speedFilterAtom(key));

  const isValidNumber = useCallback((newValue: string) => {
    const value = newValue as unknown as number;

    return value >= DEFAULT_MIN_SPEED && value <= DEFAULT_MAX_SPEED;
  }, []);

  const getSliderValue = useCallback((values: SpeedFilterFormValues) => {
    const minSpeed = parseInt(values.minSpeed);
    const maxSpeed = parseInt(values.maxSpeed);

    return [
      isNaN(minSpeed) ? DEFAULT_MIN_SPEED : minSpeed,
      isNaN(maxSpeed) ? DEFAULT_MAX_SPEED : maxSpeed
    ];
  }, []);

  const initialValues = useMemo(
    () => ({
      minSpeed: state.minSpeed === undefined ? `${DEFAULT_MIN_SPEED}` : `${state.minSpeed}`,
      maxSpeed: state.maxSpeed === undefined ? `${DEFAULT_MAX_SPEED}` : `${state.maxSpeed}`
    }),
    [state.maxSpeed, state.minSpeed]
  );

  const handleSubmit = useCallback(
    (values: SpeedFilterFormValues) => {
      const minSpeed = parseInt(values.minSpeed);
      const maxSpeed = parseInt(values.maxSpeed);
      setState((x) => ({
        ...x,
        minSpeed: isNaN(minSpeed) ? undefined : minSpeed,
        maxSpeed: isNaN(maxSpeed) ? undefined : maxSpeed,
        enabled: !!values.minSpeed || !!values.maxSpeed,
        open: false
      }));
    },
    [setState]
  );

  const close = useCallback(() => setState((x) => ({ ...x, open: false })), [setState]);

  const handleChange = useCallback(
    (
      e: React.ChangeEvent<HTMLInputElement>,
      field: string,
      setFieldValue: (field: string, value: any, shouldValidate?: boolean | undefined) => void
    ) => {
      if (isValidNumber(e.target.value)) {
        setFieldValue(field, e.target.value);
      }
    },
    [isValidNumber]
  );

  return { getSliderValue, handleSubmit, close, handleChange, state, initialValues };
}
