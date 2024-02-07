import { faMountain } from "@fortawesome/free-solid-svg-icons";
import { useCurrentUnits } from "@navtrack/shared/hooks/util/useCurrentUnits";
import { useMemo } from "react";
import { useRecoilState, useResetRecoilState } from "recoil";
import { FilterBadge } from "../FilterBadge";
import { IconWithText } from "../../../../ui/icon/IconWithText";
import { altitudeFilterAtom } from "../locationFilterState";

interface IAltitudeFilterBadge {
  filterKey: string;
}

export function AltitudeFilterBadge(props: IAltitudeFilterBadge) {
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
        <FilterBadge
          order={state.order}
          onClick={() => setState((x) => ({ ...x, open: true }))}
          onCloseClick={reset}>
          <IconWithText icon={faMountain}>{text}</IconWithText>
        </FilterBadge>
      )}
    </>
  );
}
