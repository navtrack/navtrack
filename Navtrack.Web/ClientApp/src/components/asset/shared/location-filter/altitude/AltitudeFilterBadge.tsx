import { faMountain } from "@fortawesome/free-solid-svg-icons";
import { useCurrentUnits } from "@navtrack/navtrack-app-shared";
import { useMemo } from "react";
import { useRecoilState, useResetRecoilState } from "recoil";
import Badge from "../../../../ui/shared/badge/Badge";
import IconWithText from "../../../../ui/shared/icon/IconWithText";
import { altitudeFilterAtom } from "../state";

interface IAltitudeFilterBadge {
  filterKey: string;
}

export default function AltitudeFilterBadge(props: IAltitudeFilterBadge) {
  const [state, setState] = useRecoilState(altitudeFilterAtom(props.filterKey));
  const units = useCurrentUnits();
  const reset = useResetRecoilState(altitudeFilterAtom(props.filterKey));

  const text = useMemo(() => {
    if (state.minAltitude && state.maxAltitude) {
      return `${state.minAltitude} - ${state.maxAltitude} (${units.length})`;
    } else if (state.minAltitude) {
      return `> ${state.minAltitude} (${units.length})`;
    } else if (state.maxAltitude) {
      return `< ${state.maxAltitude} (${units.length})`;
    }
  }, [state.maxAltitude, state.minAltitude, units.length]);

  return (
    <>
      {state.enabled && (
        <Badge
          order={state.order}
          onClick={() => setState((x) => ({ ...x, open: true }))}
          onCloseClick={reset}>
          <IconWithText icon={faMountain}>{text}</IconWithText>
        </Badge>
      )}
    </>
  );
}
