import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { TableV2 } from "../../ui/table/TableV2";
import { TripModel } from "@navtrack/shared/api/model";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { useAtomValue } from "jotai";
import { locationFiltersSelector } from "../shared/location-filter/locationFilterState";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";
import { useCallback } from "react";
import {
  addDays,
  addHours,
  differenceInMinutes,
  differenceInSeconds,
  endOfDay,
  isBefore,
  max,
  min,
  setHours,
  setMinutes,
  startOfDay
} from "date-fns";
import { useHTMLElementSize } from "@navtrack/shared/hooks/app/util/useHTMLElementSize";
import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { useAssetTripsQueries } from "@navtrack/shared/hooks/queries/assets/useAssetTripsQueries";
import { Tooltip } from "../../ui/tooltip/Tooltip";
import { FormattedMessage } from "react-intl";
import { useOrganizationWorkSchedule } from "./useOrganizationWorkSchedule";

type WorkingHoursTableRow = {
  date: Date;
  trips: TripModel[];
  workHoursDuration: number;
  offHoursDuration: number;
  totalDuration: number;
};

export function AssetReportsWorkingHoursPage() {
  const show = useShow();
  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("reports-trips");
  const filters = useAtomValue(locationFiltersSelector(locationFilterKey));
  const workSchedule = useOrganizationWorkSchedule();

  const query = useAssetTripsQueries({
    assetId: currentAsset.data?.id,
    ...filters
  });

  const tableRows: WorkingHoursTableRow[] = query.queries
    .filter((x) => !!x.query.data)
    .map((tripGroup) => {
      const startTime = workSchedule.getStartTime(tripGroup.date);
      const endTime = workSchedule.getEndTime(tripGroup.date);

      const hours = ComputeTripDurations(
        tripGroup.query.data?.items ?? [],
        startTime?.getHours() ?? 0,
        startTime?.getMinutes() ?? 0,
        endTime?.getHours() ?? 0,
        endTime?.getMinutes() ?? 0
      );

      const result: WorkingHoursTableRow = {
        date: tripGroup.date,
        trips: tripGroup.query.data?.items ?? [],
        workHoursDuration: hours.workHoursMs,
        offHoursDuration: hours.offHoursMs,
        totalDuration: tripGroup.query.data?.totalDuration ?? 0
      };

      return result;
    });

  const elementSize = useHTMLElementSize<HTMLDivElement>();

  const pixelsPerMinute = useCallback(() => {
    return elementSize.width / 1440;
  }, [elementSize.width]);

  const getMinutesFromStartOfDay = (date: Date) => {
    const start = startOfDay(date);

    return differenceInMinutes(date, start);
  };

  const getTripLeftMargin = useCallback(
    (date: Date) => {
      const minutesFromStartOfDay = getMinutesFromStartOfDay(date);

      return Math.round(minutesFromStartOfDay * pixelsPerMinute());
    },
    [pixelsPerMinute]
  );

  const getTripWidth = useCallback(
    (startDate: Date, endDate: Date) => {
      const minutes = differenceInMinutes(endDate, startDate);

      return Math.round(minutes * pixelsPerMinute());
    },
    [pixelsPerMinute]
  );

  return (
    <>
      <LocationFilter filterPage="reports-trips" />
      <TableV2<WorkingHoursTableRow>
        className="h-full"
        columns={[
          {
            labelId: "generic.date",
            row: (item) => (
              <div className="whitespace-nowrap">{show.date(item.date)}</div>
            ),
            value: (item) => item.date.toISOString(),
            sort: "desc"
          },
          {
            labelId: "generic.location",
            header: () => (
              <div className="flex h-full flex-1 bg-gray-100 space-x-px border-x border-gray-200">
                {[...Array(24).keys()].map((_, index) => (
                  <div
                    className="h-full flex-1 bg-gray-50 flex items-center justify-center text-xs"
                    key={index}>
                    {index}
                  </div>
                ))}
              </div>
            ),
            headerClassName: "pt-0 pb-0",
            rowClassName: "w-full pt-0 pb-0",
            row: (item, rowIndex) => (
              <div
                className="flex h-full relative w-full border-x border-gray-200"
                ref={elementSize.ref}>
                {[...Array(24).keys()].map((_, index) => (
                  <div
                    key={index}
                    className={classNames(
                      "h-full flex-1",
                      c(index !== 0, "border-l border-gray-100"),
                      c(rowIndex % 2 !== 0, "bg-gray-50", "bg-white")
                    )}></div>
                ))}
                <div
                  className={classNames(
                    "rounded-md absolute top-1 bottom-1 bg-blue-500/20"
                  )}
                  style={{
                    marginLeft: getTripLeftMargin(
                      workSchedule.getStartTime(item.date)!
                    ),
                    width: getTripWidth(
                      workSchedule.getStartTime(item.date)!,
                      workSchedule.getEndTime(item.date)!
                    ),
                    minWidth: 4
                  }}></div>
                {item.trips.map((trip, index) => (
                  <div
                    key={index}
                    className="rounded-md absolute top-1 bottom-1 bg-blue-500/80 overflow-hidden"
                    style={{
                      marginLeft: getTripLeftMargin(
                        new Date(trip.startPosition.date)
                      ),
                      width: getTripWidth(
                        new Date(trip.startPosition.date),
                        new Date(trip.endPosition.date)
                      ),
                      minWidth: 4
                    }}>
                    <Tooltip
                      content={
                        <table className="text-xs">
                          <tbody>
                            <tr>
                              <td className="pr-1">
                                <FormattedMessage id="generic.start" />
                              </td>
                              <td>{show.time(trip.startPosition.date)}</td>
                            </tr>
                            <tr>
                              <td className="pr-1">
                                <FormattedMessage id="generic.end" />
                              </td>
                              <td>{show.time(trip.endPosition.date)}</td>
                            </tr>
                            <tr>
                              <td className="pr-1">
                                <FormattedMessage id="generic.duration" />
                              </td>
                              <td>{show.duration(trip.duration)}</td>
                            </tr>
                          </tbody>
                        </table>
                      }
                      className="h-full w-full hover:bg-blue-600"></Tooltip>
                  </div>
                ))}
              </div>
            )
          },
          {
            labelId: "generic.during-schedule",
            row: (item) => (
              <div className="whitespace-nowrap">
                {show.duration(item.workHoursDuration)}
              </div>
            ),
            value: (item) => item.workHoursDuration,
            footer: (total) => <>{show.duration(total)}</>
          },
          {
            labelId: "generic.off-schedule",
            row: (item) => (
              <div className="whitespace-nowrap">
                {show.duration(item.offHoursDuration)}
              </div>
            ),
            value: (item) => item.offHoursDuration,
            footer: (total) => <>{show.duration(total)}</>
          },
          {
            labelId: "generic.trips-total",
            row: (item) => (
              <div className="whitespace-nowrap">
                {show.duration(item.totalDuration)}
              </div>
            ),
            value: (item) => item.totalDuration,
            footer: (total) => <>{show.duration(total)}</>
          },
          {
            labelId: "generic.start-time",
            row: (item) => (
              <div className="whitespace-nowrap">
                {show.time(item.trips[0]?.startPosition.date)}
              </div>
            ),
            value: (item) => item.trips[0]?.startPosition.date
          },
          {
            labelId: "generic.end-time",
            row: (item) => (
              <div className="whitespace-nowrap">
                {show.time(item.trips[item.trips.length - 1]?.endPosition.date)}
              </div>
            ),
            value: (item) => item.trips[item.trips.length - 1]?.endPosition.date
          },
          {
            labelId: "generic.total-duration",
            row: (item) => (
              <div className="whitespace-nowrap">
                {show.duration(
                  differenceInSeconds(
                    item.trips[item.trips.length - 1]?.endPosition.date,
                    item.trips[0]?.startPosition.date
                  )
                )}
              </div>
            ),
            value: (item) =>
              differenceInSeconds(
                item.trips[item.trips.length - 1]?.endPosition.date,
                item.trips[0]?.startPosition.date
              ),
            footer: (total) => <>{show.duration(total)}</>
          }
        ]}
        rows={tableRows}
        isLoading={query.isLoading}
      />
    </>
  );
}

type CustomDuration = {
  workHoursMs: number;
  offHoursMs: number;
};

function ComputeOverlapDuration(
  start1: Date,
  end1: Date,
  start2: Date,
  end2: Date
): number {
  const overlapStart = max([start1, start2]);
  const overlapEnd = min([end1, end2]);

  return isBefore(overlapStart, overlapEnd)
    ? differenceInSeconds(overlapEnd, overlapStart)
    : 0;
}

function ComputeTripDurations(
  trips: TripModel[],
  workStartHour: number,
  workStartMinute: number,
  workEndHour: number,
  workEndMinute: number
): CustomDuration {
  console.log;

  let totalWorkMs = 0;
  let totalOffMs = 0;

  trips.forEach((trip) => {
    let currentDay = startOfDay(trip.startPosition.date);

    // Iterate day by day until the trip end
    while (!isBefore(trip.endPosition.date, currentDay)) {
      // Define work hours for the current day
      const workStart = setMinutes(
        setHours(currentDay, workStartHour),
        workStartMinute
      );
      const workEnd = setMinutes(
        setHours(currentDay, workEndHour),
        workEndMinute
      );
      // Compute overlap with this day's work hours
      const workMs = ComputeOverlapDuration(
        new Date(trip.startPosition.date),
        new Date(trip.endPosition.date),
        workStart,
        workEnd
      );
      const dayMs = ComputeOverlapDuration(
        new Date(trip.startPosition.date),
        new Date(trip.endPosition.date),
        startOfDay(currentDay),
        endOfDay(currentDay)
      );

      totalWorkMs += workMs;
      totalOffMs += dayMs - workMs;

      currentDay = addDays(currentDay, 1);
    }
  });

  return { workHoursMs: totalWorkMs, offHoursMs: totalOffMs };
}
