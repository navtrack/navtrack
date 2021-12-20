import classNames from "classnames";
import { FormattedMessage } from "react-intl";
import useCurrentUnits from "../../../hooks/util/useCurrentUnits";
import useDateTime from "../../../hooks/util/useDateTime";
import useDistance from "../../../hooks/util/useDistance";
import { showDateTime } from "../../../utils/dates";
import LoadingIndicator from "../../ui/shared/loading-indicator/LoadingIndicator";
import useTrips from "./useTrips";

export default function TripsTable() {
  const trips = useTrips();
  const units = useCurrentUnits();
  const { showDuration } = useDateTime();
  const { showSpeed, showDistance, showAltitude } = useDistance();

  return (
    <div
      className="flex flex-col flex-grow overflow-hidden rounded-lg shadow"
      style={{ flexBasis: 0, minHeight: "190px" }}>
      <div className="border-b border-gray-200 flex text-xs font-medium text-gray-500 uppercase tracking-wider bg-gray-50 grid grid-cols-12">
        <div className="pl-2 py-2 col-span-2">
          <FormattedMessage id="generic.start-date" />
        </div>
        <div className="pl-2 py-2 col-span-2">
          <FormattedMessage id="generic.end-date" />
        </div>
        <div className="pl-2 py-2 col-span-2">
          <FormattedMessage id="generic.duration" />
        </div>
        <div className="pl-2 py-2 col-span-2">
          <FormattedMessage id="generic.distance" />
        </div>
        <div className="pl-2 py-2 col-span-2">
          <FormattedMessage id="generic.avg-speed" />
        </div>
        <div className="pl-2 py-2 col-span-2">
          <FormattedMessage id="generic.avg-altitude" />
        </div>
      </div>
      <div className="flex bg-gray-50 flex-grow overflow-hidden">
        <div className="border-b border-gray-200 text-gray-600 text-xs flex-col overflow-y-scroll flex-grow">
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
                      "flex flex-row grid grid-cols-12 cursor-pointer",
                      trips.selectedTripIndex === index
                        ? "bg-gray-300 hover:bg-gray-300"
                        : index % 2 === 0
                        ? "bg-white hover:bg-gray-200"
                        : "bg-gray-50 hover:bg-gray-200"
                    )}
                    ref={(el) => (trips.tripElements.current[index] = el)}
                    onClick={() => trips.setTripIndex(index)}>
                    <div className="pl-2 py-1 col-span-2">
                      {showDateTime(trip.startLocation.dateTime)}
                    </div>
                    <div className="pl-2 py-1 col-span-2">
                      {showDateTime(trip.endLocation.dateTime)}
                    </div>
                    <div className="pl-2 py-1 col-span-2">
                      {showDuration(trip.duration)}
                    </div>
                    <div className="pl-2 py-1 col-span-2">
                      {showDistance(trip.distance)}
                    </div>
                    <div className="pl-2 py-1 col-span-2">
                      {showSpeed(trip.averageSpeed)}
                    </div>
                    <div className="pl-2 py-1 col-span-2">
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
        <div className="border-b border-gray-200 flex text-xs text-gray-600 bg-gray-50 grid grid-cols-12 font-medium">
          <div className="pl-2 py-1"></div>
          <div className="pl-2 py-1"></div>
          <div className="pl-2 py-1"></div>
          <div className="pl-2 py-1"></div>
          <div className="pl-2 py-1 col-span-2">
            {showDuration(trips.data?.totalDuration)}
          </div>
          <div className="pl-2 py-1 col-span-2">
            {showDistance(trips.data?.totalDistance)}
          </div>
          <div className="pl-2 py-1 col-span-2">
            {showSpeed(trips.data?.totalAvgSpeed)}
          </div>
          <div className="pl-2 py-1">
            {Math.round(trips.data?.totalAvgAltitude as number)} {units.length}
          </div>
        </div>
      )}
    </div>
  );
}
