import { faTachometerAlt } from "@fortawesome/free-solid-svg-icons";
import { useCurrentUnits } from "@navtrack/shared/hooks/util/useCurrentUnits";
import { useMemo } from "react";
import { useAtom } from "jotai";
import { useResetAtom } from "jotai/utils";
import { FilterBadge } from "../FilterBadge";
import { IconWithText } from "../../../../ui/icon/IconWithText";
import { averageSpeedFilterAtom, speedFilterAtom } from "../locationFilterState";

type SpeedFilterBadgeProps = {
  filterKey: string;
};

export function SpeedFilterBadge(props: SpeedFilterBadgeProps) {
  return (
    <SpeedFilterBadgeBase
      filterAtom={speedFilterAtom}
      filterKey={props.filterKey}
    />
  );
}

export function AverageSpeedFilterBadge(props: SpeedFilterBadgeProps) {
  return (
    <SpeedFilterBadgeBase
      filterAtom={averageSpeedFilterAtom}
      filterKey={props.filterKey}
    />
  );
}

type SpeedFilterBadgeBaseProps = SpeedFilterBadgeProps & {
  filterAtom: typeof speedFilterAtom;
};

function SpeedFilterBadgeBase(props: SpeedFilterBadgeBaseProps) {
  const [state, setState] = useAtom(props.filterAtom(props.filterKey));
  const reset = useResetAtom(props.filterAtom(props.filterKey));
  const units = useCurrentUnits();

  const text = useMemo(() => {
    if (state.minSpeed !== undefined && state.maxSpeed !== undefined) {
      return `${state.minSpeed} - ${state.maxSpeed} (${units.speed})`;
    } else if (state.minSpeed !== undefined) {
      return `> ${state.minSpeed} (${units.speed})`;
    } else if (state.maxSpeed !== undefined) {
      return `< ${state.maxSpeed} (${units.speed})`;
    }
  }, [state, units.speed]);

  return (
    <>
      {state.active && (
        <FilterBadge
          order={state.order}
          onClick={() => setState((prev) => ({ ...prev, open: true }))}
          onCloseClick={reset}>
          <IconWithText icon={faTachometerAlt}>{text}</IconWithText>
        </FilterBadge>
      )}
    </>
  );
}
