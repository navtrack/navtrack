import { faCalendarAlt } from "@fortawesome/free-solid-svg-icons";
import { format } from "date-fns";
import { useMemo } from "react";
import { FilterBadge } from "../FilterBadge";
import { dateFilterAtom } from "../locationFilterState";
import { DateRange } from "../locationFilterTypes";
import { useAtom } from "jotai";
import { useIntl } from "react-intl";
import { dateOptions } from "./dateOptions";
import { IconWithText } from "../../../../ui/icon/IconWithText";

type DateFilterBadgeProps = {
  filterKey: string;
};

export function DateFilterBadge(props: DateFilterBadgeProps) {
  const [state, setState] = useAtom(dateFilterAtom(props.filterKey));
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
    <FilterBadge
      onClick={() => setState((x) => ({ ...x, open: true }))}
      order={0}>
      <IconWithText icon={faCalendarAlt}>{dateFilterText}</IconWithText>
    </FilterBadge>
  );
}
