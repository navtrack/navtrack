import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { FormattedMessage } from "react-intl";
import { Card } from "../../ui/card/Card";
import { Map } from "../../ui/map/Map";
import { LocationBar } from "../shared/location-bar/LocationBar";
import { MapPin } from "../../ui/map/MapPin";
import { MapContainer } from "../../ui/map/MapContainer";

export function AssetLiveTrackingPage() {
  const currentAsset = useCurrentAsset();
  const location = currentAsset.data?.position;

  return (
    <>
      {location ? (
        <>
          <Card className="p-2">
            <LocationBar location={location} />
          </Card>
          <Card className="flex flex-grow">
            <MapContainer>
              <Map center={{ ...location }}>
                <MapPin position={{ ...location }} follow />
              </Map>
            </MapContainer>
          </Card>
        </>
      ) : (
        <Card className="flex p-2 text-sm">
          <FormattedMessage id="no-location-data" />
        </Card>
      )}
    </>
  );
}
