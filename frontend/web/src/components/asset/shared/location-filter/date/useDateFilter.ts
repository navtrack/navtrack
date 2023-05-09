import { useCallback } from "react";
import { useRecoilState } from "recoil";
import { dateFilterAtom } from "../state";
import { DateFilter, DateRange } from "../types";
import { dateOptions } from "./date-options";

interface IDateFilter {
  filterKey: string;
}

export function useDateFilter(props: IDateFilter) {
  const [state, setState] = useRecoilState(dateFilterAtom(props.filterKey));

  const close = useCallback(
    () => setState((x) => ({ ...x, open: false })),
    [setState]
  );

  const handleSubmit = useCallback(
    (values: DateFilter) => {
      const dateOption = dateOptions.find((x) => x.range === values.range);

      if (dateOption !== undefined) {
        const isCustom = dateOption.range === DateRange.Custom;

        setState((x) => ({
          startDate: isCustom ? values.startDate : dateOption.startDate,
          endDate: isCustom ? values.endDate : dateOption.endDate,
          range: values.range,
          open: false
        }));
      }
    },
    [setState]
  );

  return {
    state,
    close,
    handleSubmit
  };
}
