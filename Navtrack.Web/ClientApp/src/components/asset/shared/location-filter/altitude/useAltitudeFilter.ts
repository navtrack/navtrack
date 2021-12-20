import { useCallback, useMemo } from "react";
import { useRecoilState } from "recoil";
import { altitudeFilterAtom } from "../state";
import { AltitudeFilterFormValues } from "../types";

export default function useAltitudeFilter(key: string) {
  const [state, setState] = useRecoilState(altitudeFilterAtom(key));

  const initialValues = useMemo(
    () => ({
      minAltitude: state.minAltitude ? `${state.minAltitude}` : ``,
      maxAltitude: state.maxAltitude ? `${state.maxAltitude}` : ``
    }),
    [state.maxAltitude, state.minAltitude]
  );

  const handleSubmit = useCallback(
    (values: AltitudeFilterFormValues) => {
      const minAltitude = parseInt(values.minAltitude);
      const maxAltitude = parseInt(values.maxAltitude);
      setState((x) => ({
        ...x,
        minAltitude: isNaN(minAltitude) ? undefined : minAltitude,
        maxAltitude: isNaN(maxAltitude) ? undefined : maxAltitude,
        enabled: !!values.minAltitude || !!values.maxAltitude,
        open: false
      }));
    },
    [setState]
  );

  const close = useCallback(() => setState((x) => ({ ...x, open: false })), [setState]);

  return { handleSubmit, close, initialValues, state };
}
