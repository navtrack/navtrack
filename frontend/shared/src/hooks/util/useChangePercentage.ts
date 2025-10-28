import { useMemo } from "react";

export function useChangePercentage(
  current?: number | null,
  previous?: number | null
) {
  const change = useMemo(() => {
    if (
      current === null ||
      previous === null ||
      current === undefined ||
      previous === undefined ||
      current === 0 ||
      previous === 0
    ) {
      return undefined;
    }

    return Math.round(((current - previous) / previous) * 100);
  }, [current, previous]);

  const direction = useMemo(
    () =>
      change === undefined ? undefined : change < 0 ? "decrease" : "increase",
    [change]
  );

  return {
    change,
    direction
  };
}
