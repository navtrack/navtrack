import classNames from "classnames";
import { FormattedMessage } from "react-intl";
import useDateTime from "../../../hooks/util/useDateTime";
import useDistance from "../../../hooks/util/useDistance";
import LoadingIndicator from "../../ui/shared/loading-indicator/LoadingIndicator";
import useLog from "./useLog";

export default function LogTable() {
  const log = useLog();
  const { showDateTime } = useDateTime();
  const { showSpeed, showAltitude } = useDistance();

  return (
    <div
      className="flex flex-col flex-grow overflow-hidden rounded-lg shadow"
      style={{ flexBasis: 0 }}>
      <div className="border-b border-gray-200 flex text-xs font-medium text-gray-500 uppercase tracking-wider bg-gray-50 grid grid-cols-12">
        <div className="pl-2 py-2 col-span-2">
          <FormattedMessage id="generic.date" />
        </div>
        <div className="pl-2 py-2">
          <FormattedMessage id="generic.latitude" />
        </div>
        <div className="pl-2 py-2">
          <FormattedMessage id="generic.longitude" />
        </div>
        <div className="pl-2 py-2">
          <FormattedMessage id="generic.altitude" />
        </div>
        <div className="pl-2 py-2">
          <FormattedMessage id="generic.speed" />
        </div>
        <div className="pl-2 py-2">
          <FormattedMessage id="generic.heading" />
        </div>
        <div className="pl-2 py-2">
          <FormattedMessage id="generic.satellites" />
        </div>
      </div>
      <div className="flex bg-gray-50 flex-grow overflow-hidden">
        <div className="border-b border-gray-200 text-gray-600 text-xs flex-col overflow-y-scroll flex-grow">
          {log.isLoading ? (
            <div className="py-1">
              <LoadingIndicator className="text-base" />
            </div>
          ) : (
            <>
              {log.data?.items.length ? (
                log.data?.items.map((location, index) => (
                  <div
                    key={location.id}
                    className={classNames(
                      "flex flex-row grid grid-cols-12 cursor-pointer",
                      log.selectedLocationIndex === index
                        ? "bg-gray-300 hover:bg-gray-300"
                        : index % 2 === 0
                        ? "bg-white hover:bg-gray-200"
                        : "bg-gray-50 hover:bg-gray-200"
                    )}
                    ref={(el) => (log.locationElements.current[index] = el)}
                    onClick={() => log.setSelectedLocationIndex(index)}>
                    <div className="pl-2 py-1 col-span-2">
                      {showDateTime(location.dateTime)}
                    </div>
                    <div className="pl-2 py-1">{location.latitude}</div>
                    <div className="pl-2 py-1">{location.longitude}</div>
                    <div className="pl-2 py-1">
                      {showAltitude(location.altitude)}
                    </div>
                    <div className="pl-2 py-1">{showSpeed(location.speed)}</div>
                    <div className="pl-2 py-1">{location.heading}</div>
                    <div className="pl-2 py-1">{location.satellites}</div>
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
      {log.data?.items.length && (
        <div className="border-b border-gray-200 flex text-xs text-gray-600 bg-gray-50 grid grid-cols-12 font-medium">
          <div className="pl-2 py-1">
            <span className="mr-1">{log.data?.items.length ?? 0}</span>
            <FormattedMessage id="assets.log.table.locations" />
          </div>
          <div className="pl-2 py-1"></div>
          <div className="pl-2 py-1"></div>
          <div className="pl-2 py-1">
            <FormattedMessage id="generic.average" />
          </div>
          <div className="pl-2 py-1">
            {showAltitude(log.data.averageAltitude)}
          </div>
          <div className="pl-2 py-1">{showSpeed(log.data.averageSpeed)}</div>
        </div>
      )}
    </div>
  );
}
