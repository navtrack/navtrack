import { useCallback, useMemo } from "react";
import { useAtom } from "jotai";
import { altitudeFilterAtom } from "../locationFilterState";
import { AltitudeFilterFormValues } from "../locationFilterTypes";

export function useAltitudeFilter(key: string) {
  const [state, setState] = useAtom(altitudeFilterAtom(key));

  const initialValues = useMemo(
    () => ({
      minAltitude: state.minAltitude ? `${state.minAltitude}` : ``,
      maxAltitude: state.maxAltitude ? `${state.maxAltitude}` : ``
    }),
    [state.maxAltitude, state.minAltitude]
  );

  const handleSubmit = useCallback(
    (values: AltitudeFilterFormValues) => {
      const minAltitude =
        values.minAltitude !== undefined
          ? parseInt(values.minAltitude)
          : undefined;
      const maxAltitude =
        values.maxAltitude !== undefined
          ? parseInt(values.maxAltitude)
          : undefined;
      setState((x) => ({
        ...x,
        minAltitude:
          minAltitude !== undefined && isNaN(minAltitude)
            ? undefined
            : minAltitude,
        maxAltitude:
          maxAltitude !== undefined && isNaN(maxAltitude)
            ? undefined
            : maxAltitude,
        enabled: !!values.minAltitude || !!values.maxAltitude,
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
