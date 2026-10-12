import { AssetStatsPeriod } from "@navtrack/shared/api/model";
import { AssetDashboardRow } from "./AssetDashboardRow";

const periods = [
  { period: AssetStatsPeriod.Day, labelId: "today" },
  { period: AssetStatsPeriod.Week, labelId: "this-week" },
  { period: AssetStatsPeriod.Month, labelId: "this-month" },
  { period: AssetStatsPeriod.Year, labelId: "this-year" }
];

export function AssetDashboardPage() {
  return (
    <>
      {periods.map((item) => (
        <AssetDashboardRow
          key={item.labelId}
          period={item.period}
          labelId={item.labelId}
        />
      ))}
    </>
  );
}
