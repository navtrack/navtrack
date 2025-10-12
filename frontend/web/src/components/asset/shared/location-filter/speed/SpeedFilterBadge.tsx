import { faTachometerAlt } from "@fortawesome/free-solid-svg-icons";
import { useCurrentUnits } from "@navtrack/shared/hooks/util/useCurrentUnits";
import { useMemo } from "react";
import { useAtom } from "jotai";
import { useResetAtom } from "jotai/utils";
import { FilterBadge } from "../FilterBadge";
import { IconWithText } from "../../../../ui/icon/IconWithText";
import { speedFilterAtom } from "../locationFilterState";

type SpeedFilterBadgeProps = {
  filterKey: string;
};

export function SpeedFilterBadge(props: SpeedFilterBadgeProps) {
  const [state, setState] = useAtom(speedFilterAtom(props.filterKey));
  const reset = useResetAtom(speedFilterAtom(props.filterKey));
  const units = useCurrentUnits();

  const text = useMemo(() => {
    if (state.minSpeed !== undefined && state.maxSpeed !== undefined) {
      return `${state.minSpeed} - ${state.maxSpeed} (${units.speed})`;
    } else if (state.minSpeed !== undefined) {
      return `> ${state.minSpeed} (${units.speed})`;
    } else if (state.maxSpeed !== undefined) {
      return `< ${state.maxSpeed} (${units.speed})`;
    }
  }, [state.maxSpeed, state.minSpeed, units.speed]);

  return (
    <>
      {state.enabled && (
        <FilterBadge
          order={state.order}
          onClick={() => setState((x) => ({ ...x, open: true }))}
          onCloseClick={reset}>
          <IconWithText icon={faTachometerAlt}>{text}</IconWithText>
        </FilterBadge>
      )}
    </>
  );
}
