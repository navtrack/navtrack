import { useCallback, useMemo } from "react";
import { useAtom } from "jotai";
import { durationFilterAtom } from "../locationFilterState";
import { DurationFilterFormValues } from "../locationFilterTypes";

export function useDurationFilter(key: string) {
  const [state, setState] = useAtom(durationFilterAtom(key));

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
