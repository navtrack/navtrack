import { AssetStatsPeriod } from "@navtrack/shared/api/model";
import { OrganizationDashboardRow } from "./OrganizationDashboardRow";

const periods = [
  { period: AssetStatsPeriod.Day, labelId: "today" },
  { period: AssetStatsPeriod.Week, labelId: "this-week" },
  { period: AssetStatsPeriod.Month, labelId: "this-month" },
  { period: AssetStatsPeriod.Year, labelId: "this-year" }
];

export function OrganizationDashboardPage() {
  return (
    <>
      {periods.map((item) => (
        <OrganizationDashboardRow
          key={item.labelId}
          period={item.period}
          labelId={item.labelId}
        />
      ))}
    </>
  );
}
