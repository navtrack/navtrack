import { useCallback, useMemo } from "react";
import { useRecoilState } from "recoil";
import { durationFilterAtom } from "../state";
import { DurationFilterFormValues } from "../types";

export function useDurationFilter(key: string) {
  const [state, setState] = useRecoilState(durationFilterAtom(key));

  const initialValues: DurationFilterFormValues = useMemo(
    () => ({
      minDuration: state.minDuration ? `${state.minDuration}` : ``,
      maxDuration: state.maxDuration ? `${state.maxDuration}` : ``
    }),
    [state.maxDuration, state.minDuration]
  );

  const handleSubmit = useCallback(
    (values: DurationFilterFormValues) => {
      const minDuration = parseInt(values.minDuration);
      const maxDuration = parseInt(values.maxDuration);

      setState((x) => ({
        ...x,
        minDuration: isNaN(minDuration) ? undefined : minDuration,
        maxDuration: isNaN(maxDuration) ? undefined : maxDuration,
        enabled: !!values.minDuration || !!values.maxDuration,
        open: false
      }));
    },
    [setState]
  );

  const close = useCallback(
    () => setState((x) => ({ ...x, open: false })),
    [setState]
  );

  return { handleSubmit, close, initialValues, state };
}
