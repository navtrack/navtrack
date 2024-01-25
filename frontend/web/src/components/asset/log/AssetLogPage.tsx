import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { Map } from "../../ui/map/Map";
import { MapPin } from "../../ui/map/MapPin";
import { LogTable } from "./LogTable";
import useLog from "./useLog";
import { AuthenticatedLayoutTwoColumns } from "../../ui/layouts/authenticated/AuthenticatedLayoutTwoColumns";

export function AssetLogPage() {
  const log = useLog();

  console.log(log.location);

  return (
    <AuthenticatedLayoutTwoColumns>
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
              initialZoom={16}>
              <MapPin location={log.location} follow />
            </Map>
          </div>
        </div>
      )}
    </AuthenticatedLayoutTwoColumns>
  );
}
