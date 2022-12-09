import { useCurrentUnits } from "@navtrack/ui-shared/hooks/util/useCurrentUnits";
import { useDateTime } from "@navtrack/ui-shared/hooks/util/useDateTime";
import { useDistance } from "@navtrack/ui-shared/hooks/util/useDistance";
import classNames from "classnames";
import { FormattedMessage } from "react-intl";
import LoadingIndicator from "../../ui/shared/loading-indicator/LoadingIndicator";
import useTrips from "./useTrips";

export default function TripsTable() {
  const trips = useTrips();
  const units = useCurrentUnits();
  const { showDuration, showDateTime } = useDateTime();
  const { showSpeed, showDistance, showAltitude } = useDistance();

  return (
    <div
      className="flex flex-grow flex-col overflow-hidden rounded-lg shadow"
      style={{ flexBasis: 0, minHeight: "190px" }}>
      <div className="flex grid grid-cols-12 border-b border-gray-200 bg-gray-50 text-xs font-medium uppercase tracking-wider text-gray-500">
        <div className="col-span-2 py-2 pl-2">
          <FormattedMessage id="generic.start-date" />
        </div>
        <div className="col-span-2 py-2 pl-2">
          <FormattedMessage id="generic.end-date" />
        </div>
        <div className="col-span-2 py-2 pl-2">
          <FormattedMessage id="generic.duration" />
        </div>
        <div className="col-span-2 py-2 pl-2">
          <FormattedMessage id="generic.distance" />
        </div>
        <div className="col-span-2 py-2 pl-2">
          <FormattedMessage id="generic.avg-speed" />
        </div>
        <div className="col-span-2 py-2 pl-2">
          <FormattedMessage id="generic.avg-altitude" />
        </div>
      </div>
      <div className="flex flex-grow overflow-hidden bg-gray-50">
        <div className="flex-grow flex-col overflow-y-scroll border-b border-gray-200 text-xs text-gray-600">
          {trips.isLoading ? (
            <div className="py-1">
              <LoadingIndicator className="text-base" size="lg" />
            </div>
          ) : (
            <>
              {trips.data?.items.length ? (
                trips.data?.items.map((trip, index) => (
                  <div
                    key={trip.startLocation.id}
                    className={classNames(
                      "flex grid cursor-pointer grid-cols-12 flex-row",
                      trips.selectedTripIndex === index
                        ? "bg-gray-300 hover:bg-gray-300"
                        : index % 2 === 0
                        ? "bg-white hover:bg-gray-200"
                        : "bg-gray-50 hover:bg-gray-200"
                    )}
                    ref={(el) => (trips.tripElements.current[index] = el)}
                    onClick={() => trips.setTripIndex(index)}>
                    <div className="col-span-2 py-1 pl-2">
                      {showDateTime(trip.startLocation.dateTime)}
                    </div>
                    <div className="col-span-2 py-1 pl-2">
                      {showDateTime(trip.endLocation.dateTime)}
                    </div>
                    <div className="col-span-2 py-1 pl-2">
                      {showDuration(trip.duration)}
                    </div>
                    <div className="col-span-2 py-1 pl-2">
                      {showDistance(trip.distance)}
                    </div>
                    <div className="col-span-2 py-1 pl-2">
                      {showSpeed(trip.averageSpeed)}
                    </div>
                    <div className="col-span-2 py-1 pl-2">
                      {showAltitude(trip.averageAltitude)}
                    </div>
                  </div>
                ))
              ) : (
                <div className="py-1 text-center">
                  <FormattedMessage id="no-location-data" />
                </div>
              )}
            </>
          )}
        </div>
      </div>
      {trips.data?.items?.length && (
        <div className="flex grid grid-cols-12 border-b border-gray-200 bg-gray-50 text-xs font-medium text-gray-600">
          <div className="py-1 pl-2"></div>
          <div className="py-1 pl-2"></div>
          <div className="py-1 pl-2"></div>
          <div className="py-1 pl-2"></div>
          <div className="col-span-2 py-1 pl-2">
            {showDuration(trips.data?.totalDuration)}
          </div>
          <div className="col-span-2 py-1 pl-2">
            {showDistance(trips.data?.totalDistance)}
          </div>
          <div className="col-span-2 py-1 pl-2">
            {showSpeed(trips.data?.totalAvgSpeed)}
          </div>
          <div className="py-1 pl-2">
            {Math.round(trips.data?.totalAvgAltitude as number)} {units.length}
          </div>
        </div>
      )}
    </div>
  );
}
