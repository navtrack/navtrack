import { AssetStatsPeriod } from "@navtrack/shared/api/model";
import { OrganizationDashboardRow } from "./OrganizationDashboardRow";

const periods = [
  { period: AssetStatsPeriod.Day, labelId: "generic.today" },
  { period: AssetStatsPeriod.Week, labelId: "generic.this-week" },
  { period: AssetStatsPeriod.Month, labelId: "generic.this-month" },
  { period: AssetStatsPeriod.Year, labelId: "generic.this-year" }
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
