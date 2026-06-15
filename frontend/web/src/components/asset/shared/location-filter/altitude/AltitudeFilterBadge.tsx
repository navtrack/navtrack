import { faMountain } from "@fortawesome/free-solid-svg-icons";
import { useCurrentUnits } from "@navtrack/shared/hooks/util/useCurrentUnits";
import { useMemo } from "react";
import { FilterBadge } from "../FilterBadge";
import { IconWithText } from "../../../../ui/icon/IconWithText";
import { useAtom } from "jotai";
import { altitudeFilterAtom } from "../locationFilterState";
import { useResetAtom } from "jotai/utils";

type AltitudeFilterBadgeProps = {
  filterKey: string;
};

export function AltitudeFilterBadge(props: AltitudeFilterBadgeProps) {
  const [state, setState] = useAtom(altitudeFilterAtom(props.filterKey));
  const reset = useResetAtom(altitudeFilterAtom(props.filterKey));
  const units = useCurrentUnits();

  const text = useMemo(() => {
    if (state.minAltitude && state.maxAltitude) {
      return `${state.minAltitude} - ${state.maxAltitude} (${units.length})`;
    } else if (state.minAltitude) {
      return `> ${state.minAltitude} (${units.length})`;
    } else if (state.maxAltitude) {
      return `< ${state.maxAltitude} (${units.length})`;
    }
  }, [state, units.length]);

  return (
    <>
      {state.active && (
        <FilterBadge
          order={state.order}
          onClick={() => setState((prev) => ({ ...prev, open: true }))}
          onCloseClick={reset}>
          <IconWithText icon={faMountain}>{text}</IconWithText>
        </FilterBadge>
      )}
    </>
  );
}
