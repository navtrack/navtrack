import { useCallback, useMemo } from "react";
import { useRecoilState } from "recoil";
import { speedFilterAtom } from "../locationFilterState";
import { SpeedFilterFormValues } from "../locationFilterTypes";
import { isNumeric } from "@navtrack/shared/utils/numbers";

export function useSpeedFilter(key: string) {
  const [state, setState] = useRecoilState(speedFilterAtom(key));

  const initialValues = useMemo(
    () => ({
      minSpeed: state.minSpeed === undefined ? `` : `${state.minSpeed}`,
      maxSpeed: state.maxSpeed === undefined ? `` : `${state.maxSpeed}`
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

  const close = useCallback(
    () => setState((x) => ({ ...x, open: false })),
    [setState]
  );

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

  return {
    handleSubmit,
    close,
    handleChange,
    state,
    initialValues
  };
}
