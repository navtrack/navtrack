import { faCalendarAlt } from "@fortawesome/free-solid-svg-icons";
import { format } from "date-fns";
import { useMemo } from "react";
import { Badge } from "../../../../ui/shared/badge/Badge";
import { dateFilterAtom } from "../state";
import { DateRange } from "../types";
import { useRecoilState } from "recoil";
import { useIntl } from "react-intl";
import { dateOptions } from "./date-options";
import { IconWithText } from "../../../../ui/shared/icon/IconWithText";

interface IDateFilterBadge {
  filterKey: string;
}

export function DateFilterBadge(props: IDateFilterBadge) {
  const [state, setState] = useRecoilState(dateFilterAtom(props.filterKey));
  const intl = useIntl();

  const dateFilterText = useMemo(() => {
    if (state.range === DateRange.Custom && state.startDate && state.endDate) {
      return `${format(state.startDate, "PP")} - ${format(
        state.endDate,
        "PP"
      )}`;
    }
    const dateOption = dateOptions.find((x) => x.range === state.range);

    if (dateOption) {
      return intl.formatMessage({ id: dateOption.name });
    }
  }, [intl, state.endDate, state.range, state.startDate]);

  return (
    <Badge onClick={() => setState((x) => ({ ...x, open: true }))} order={0}>
      <IconWithText icon={faCalendarAlt}>{dateFilterText}</IconWithText>
    </Badge>
  );
}
