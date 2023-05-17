import { useDateTime } from "@navtrack/shared/hooks/util/useDateTime";
import { useDistance } from "@navtrack/shared/hooks/util/useDistance";
import { FormattedMessage } from "react-intl";
import { LoadingIndicator } from "../../ui/shared/loading-indicator/LoadingIndicator";
import useLog from "./useLog";
import { classNames } from "@navtrack/shared/utils/tailwind";

export function LogTable() {
  const log = useLog();
  const { showDateTime } = useDateTime();
  const { showSpeed, showAltitude } = useDistance();

  return (
    <div
      className="flex flex-grow flex-col overflow-hidden rounded-lg shadow"
      style={{ flexBasis: 0 }}>
      <div className="flex grid grid-cols-12 border-b border-gray-200 bg-gray-50 text-xs font-medium uppercase tracking-wider text-gray-500">
        <div className="col-span-2 py-2 pl-2">
          <FormattedMessage id="generic.date" />
        </div>
        <div className="py-2 pl-2">
          <FormattedMessage id="generic.latitude" />
        </div>
        <div className="py-2 pl-2">
          <FormattedMessage id="generic.longitude" />
        </div>
        <div className="py-2 pl-2">
          <FormattedMessage id="generic.altitude" />
        </div>
        <div className="py-2 pl-2">
          <FormattedMessage id="generic.speed" />
        </div>
        <div className="py-2 pl-2">
          <FormattedMessage id="generic.heading" />
        </div>
        <div className="py-2 pl-2">
          <FormattedMessage id="generic.satellites" />
        </div>
      </div>
      <div className="flex flex-grow overflow-hidden bg-gray-50">
        <div className="flex-grow flex-col overflow-y-scroll border-b border-gray-200 text-xs text-gray-600">
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
                      "flex grid cursor-pointer grid-cols-12 flex-row",
                      log.selectedLocationIndex === index
                        ? "bg-gray-300 hover:bg-gray-300"
                        : index % 2 === 0
                        ? "bg-white hover:bg-gray-200"
                        : "bg-gray-50 hover:bg-gray-200"
                    )}
                    ref={(el) => (log.locationElements.current[index] = el)}
                    onClick={() => log.setSelectedLocationIndex(index)}>
                    <div className="col-span-2 py-1 pl-2">
                      {showDateTime(location.dateTime)}
                    </div>
                    <div className="py-1 pl-2">{location.latitude}</div>
                    <div className="py-1 pl-2">{location.longitude}</div>
                    <div className="py-1 pl-2">
                      {showAltitude(location.altitude)}
                    </div>
                    <div className="py-1 pl-2">{showSpeed(location.speed)}</div>
                    <div className="py-1 pl-2">{location.heading}</div>
                    <div className="py-1 pl-2">{location.satellites}</div>
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
        <div className="flex grid grid-cols-12 border-b border-gray-200 bg-gray-50 text-xs font-medium text-gray-600">
          <div className="py-1 pl-2">
            <span className="mr-1">{log.data?.items.length ?? 0}</span>
            <FormattedMessage id="assets.log.table.locations" />
          </div>
          <div className="py-1 pl-2"></div>
          <div className="py-1 pl-2"></div>
          <div className="py-1 pl-2">
            <FormattedMessage id="generic.average" />
          </div>
          <div className="py-1 pl-2">
            {showAltitude(log.data.averageAltitude)}
          </div>
          <div className="py-1 pl-2">{showSpeed(log.data.averageSpeed)}</div>
        </div>
      )}
    </div>
  );
}
