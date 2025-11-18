import { DistanceReportModel } from "@navtrack/shared/api/model";
import { Card } from "../../card/Card";
import { BarConfig, CustomBarChart } from "./CustomBarChart";
import { format } from "date-fns";
import { useConvert } from "@navtrack/shared/hooks/util/useConvert";
import { useCurrentUnits } from "@navtrack/shared/hooks/util/useCurrentUnits";

const distanceReport: DistanceReportModel = {
  totalDistance: 139733,
  totalDuration: 27816,
  averageSpeed: 19.370744602427866,
  maxSpeed: 88,
  items: [
    {
      date: "2025-11-06T00:00:00Z",
      distance: 20308,
      duration: 6229,
      averageSpeed: 14.440776699029126,
      maxSpeed: 74
    },
    {
      date: "2025-11-07T00:00:00Z",
      distance: 14661,
      duration: 2512,
      averageSpeed: 20.529182879377434,
      maxSpeed: 71
    },
    {
      date: "2025-11-08T00:00:00Z",
      distance: 18601,
      duration: 2865,
      averageSpeed: 25.257246376811594,
      maxSpeed: 86
    },
    {
      date: "2025-11-09T00:00:00Z",
      distance: 18333,
      duration: 2461,
      averageSpeed: 25.8450184501845,
      maxSpeed: 88
    },
    {
      date: "2025-11-10T00:00:00Z",
      distance: 17287,
      duration: 3360,
      averageSpeed: 18.252100840336134,
      maxSpeed: 70
    },
    {
      date: "2025-11-11T00:00:00Z",
      distance: 19923,
      duration: 4571,
      averageSpeed: 15.602197802197802,
      maxSpeed: 75
    },
    {
      date: "2025-11-12T00:00:00Z",
      distance: 22642,
      duration: 4036,
      averageSpeed: 19.1046511627907,
      maxSpeed: 71
    },
    {
      date: "2025-11-13T00:00:00Z",
      distance: 7978,
      duration: 1782,
      averageSpeed: 15.934782608695652,
      maxSpeed: 74
    }
  ]
};

type ChartItem = {
  date: string;
  distance: number;
};

export default {
  Default: () => {
    const units = useCurrentUnits();
    const bars: BarConfig<ChartItem>[] = [
      { key: "distance", labelId: "generic.distance" }
    ];
    const convert = useConvert();

    const items = distanceReport.items.map((item) => ({
      date: format(new Date(item.date), "MMM dd"),
      distance: convert.distance(item.distance) ?? 0
    })) as ChartItem[];

    return (
      <Card style={{ width: "100%", height: 300 }} className="p-4">
        <CustomBarChart<ChartItem>
          data={items}
          bars={bars}
          xAxis={{
            dataKey: "date"
          }}
          yAxis={{ dataKey: "distance", unit: units.lengthK }}
        />
      </Card>
    );
  }
};
