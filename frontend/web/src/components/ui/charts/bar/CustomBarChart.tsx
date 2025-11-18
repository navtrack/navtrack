import { TailwindColors } from "@navtrack/shared/utils/tailwind";
import { useIntl } from "react-intl";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend
} from "recharts";
import { Card } from "../../card/Card";

export type BarConfig<T> = {
  key: keyof T;
  color?: string;
  labelId: string;
};

type AxisConfig<T> = {
  dataKey: keyof T;
  unit?: string;
  formatter?: (value: number) => string;
};

export type CustomBarChartProps<T> = {
  data?: T[];
  bars: BarConfig<T>[];
  xAxis?: AxisConfig<T>;
  yAxis?: AxisConfig<T>;
  hideLegend?: boolean;
};

export function CustomBarChart<T>(props: CustomBarChartProps<T>) {
  const intl = useIntl();

  return (
    <BarChart
      style={{
        width: "100%",
        height: "100%"
      }}
      responsive
      data={props.data}
      margin={{
        top: 5,
        right: 0,
        left: 0,
        bottom: 5
      }}>
      <CartesianGrid vertical={false} stroke={TailwindColors.gray[200]} />
      <XAxis
        dataKey={props.xAxis?.dataKey as string}
        tickLine={false}
        axisLine={{ stroke: TailwindColors.gray[300] }}
        tick={{ fill: TailwindColors.gray[500], fontSize: 14 }}
        tickMargin={10}
        unit={props.xAxis?.unit}
      />
      <YAxis
        dataKey={props.yAxis?.dataKey as string}
        width="auto"
        tickLine={false}
        axisLine={{ stroke: TailwindColors.gray[300] }}
        tick={{ fill: TailwindColors.gray[500], fontSize: 14 }}
        unit={props.yAxis?.unit}
        tickFormatter={props.yAxis?.formatter}
      />
      <Tooltip
        cursor={{ fill: TailwindColors.gray[200], opacity: 0.5 }}
        content={(val) => (
          <Card className="p-2 drop-shadow-md">
            <div className="text-gray-900 text-sm mb-2">{val.label}</div>
            {val.payload.map((entry, index) => (
              <div
                key={`item-${index}`}
                className="text-gray-700 text-sm flex items-center">
                <div
                  className="w-3 h-3 mr-2 rounded-sm"
                  style={{ backgroundColor: entry.color }}></div>
                <div>
                  {entry.name}: {entry.value}
                  {props.yAxis?.unit}
                </div>
              </div>
            ))}
          </Card>
        )}
      />
      {!props.hideLegend && (
        <Legend
          content={(val) => (
            <div className="flex justify-center pt-4">
              {val.payload?.map((entry, index) => (
                <div
                  key={entry.value}
                  className="text-gray-700 text-sm flex items-center">
                  <div
                    className="w-3 h-3 mr-2 rounded-sm"
                    style={{ backgroundColor: entry.color }}></div>
                  <div>{entry.value}</div>
                </div>
              ))}
            </div>
          )}
        />
      )}
      {props.bars.map((bar) => (
        <Bar
          key={bar.key as string}
          name={intl.formatMessage({ id: bar.labelId })}
          dataKey={bar.key as string}
          fill={bar.color ?? TailwindColors.blue[500]}
        />
      ))}
    </BarChart>
  );
}
