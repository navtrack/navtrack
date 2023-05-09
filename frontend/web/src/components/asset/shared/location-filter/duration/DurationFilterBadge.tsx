import { faClock } from "@fortawesome/free-regular-svg-icons";
import { useMemo } from "react";
import { useIntl } from "react-intl";
import { useRecoilState, useResetRecoilState } from "recoil";
import { Badge } from "../../../../ui/shared/badge/Badge";
import { IconWithText } from "../../../../ui/shared/icon/IconWithText";
import { durationFilterAtom } from "../state";

interface IDurationFilterBadge {
  filterKey: string;
}

export function DurationFilterBadge(props: IDurationFilterBadge) {
  const [state, setState] = useRecoilState(durationFilterAtom(props.filterKey));
  const reset = useResetRecoilState(durationFilterAtom(props.filterKey));
  const intl = useIntl();

  const text = useMemo(() => {
    if (state.minDuration && state.maxDuration) {
      return `${state.minDuration} - ${state.maxDuration} (${intl.formatMessage(
        {
          id: "generic.mins"
        }
      )})`;
    } else if (state.minDuration) {
      return `> ${state.minDuration} (${intl.formatMessage({
        id: "generic.mins"
      })})`;
    } else if (state.maxDuration) {
      return `< ${state.maxDuration} (${intl.formatMessage({
        id: "generic.mins"
      })})`;
    }
  }, [intl, state.maxDuration, state.minDuration]);

  return (
    <>
      {state.enabled && (
        <Badge
          order={state.order}
          onClick={() => setState((x) => ({ ...x, open: true }))}
          onCloseClick={reset}>
          <IconWithText icon={faClock}>{text}</IconWithText>
        </Badge>
      )}
    </>
  );
}
