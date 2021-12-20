import LocationFilter from "../shared/location-filter/LocationFilter";
import Map from "../../ui/shared/map/Map";
import MapPin from "../../ui/shared/map/MapPin";
import LogTable from "./LogTable";
import useLog from "./useLog";

export default function AssetLogPage() {
  const log = useLog();

  return (
    <>
      <LocationFilter
        filterKey="log"
        center={
          log.location
            ? {
                latitude: log.location.latitude,
                longitude: log.location.longitude
              }
            : undefined
        }
      />
      <LogTable />
      {log.location && (
        <div className="flex flex-grow" style={{ flexBasis: 0 }}>
          <div className="rounded-lg shadow flex flex-grow bg-white">
            <Map
              center={{
                latitude: log.location.latitude,
                longitude: log.location.longitude
              }}
              zoom={16}>
              <MapPin
                latitude={log.location?.latitude}
                longitude={log.location?.longitude}
                follow
              />
            </Map>
          </div>
        </div>
      )}
    </>
  );
}
