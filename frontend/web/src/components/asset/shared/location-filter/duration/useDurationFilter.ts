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
      const minDuration = values.minDuration
        ? parseInt(values.minDuration)
        : undefined;
      const maxDuration = values.maxDuration
        ? parseInt(values.maxDuration)
        : undefined;

      setState((x) => ({
        ...x,
        minDuration:
          minDuration !== undefined && isNaN(minDuration)
            ? undefined
            : minDuration,
        maxDuration:
          maxDuration !== undefined && isNaN(maxDuration)
            ? undefined
            : maxDuration,
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
