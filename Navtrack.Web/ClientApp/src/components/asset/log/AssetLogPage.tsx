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
        filterPage="log"
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
          <div className="flex flex-grow rounded-lg bg-white shadow">
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
