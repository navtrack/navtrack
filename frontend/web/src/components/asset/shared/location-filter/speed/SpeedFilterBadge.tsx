import { faTachometerAlt } from "@fortawesome/free-solid-svg-icons";
import { useCurrentUnits } from "@navtrack/shared/hooks/util/useCurrentUnits";
import { useMemo } from "react";
import { useRecoilState, useResetRecoilState } from "recoil";
import { Badge } from "../../../../ui/shared/badge/Badge";
import { IconWithText } from "../../../../ui/shared/icon/IconWithText";
import { speedFilterAtom } from "../state";
import { DEFAULT_MAX_SPEED, DEFAULT_MIN_SPEED } from "../types";

interface ISpeedFilterBadge {
  filterKey: string;
}

export function SpeedFilterBadge(props: ISpeedFilterBadge) {
  const [state, setState] = useRecoilState(speedFilterAtom(props.filterKey));
  const reset = useResetRecoilState(speedFilterAtom(props.filterKey));
  const units = useCurrentUnits();

  const text = useMemo(() => {
    if (
      (state.minSpeed !== DEFAULT_MIN_SPEED &&
        state.maxSpeed !== DEFAULT_MAX_SPEED) ||
      (state.minSpeed === DEFAULT_MIN_SPEED &&
        state.maxSpeed === DEFAULT_MAX_SPEED)
    ) {
      return `${state.minSpeed} - ${state.maxSpeed} (${units.speed})`;
    } else if (
      state.minSpeed !== DEFAULT_MIN_SPEED &&
      state.maxSpeed !== DEFAULT_MAX_SPEED
    ) {
      return `${state.minSpeed} - ${state.maxSpeed} (${units.speed})`;
    } else if (state.minSpeed) {
      return `> ${state.minSpeed} (${units.speed})`;
    } else if (state.maxSpeed) {
      return `< ${state.maxSpeed} (${units.speed})`;
    }
  }, [state.maxSpeed, state.minSpeed, units.speed]);

  return (
    <>
      {state.enabled && (
        <Badge
          order={state.order}
          onClick={() => setState((x) => ({ ...x, open: true }))}
          onCloseClick={reset}>
          <IconWithText icon={faTachometerAlt}>{text}</IconWithText>
        </Badge>
      )}
    </>
  );
}
