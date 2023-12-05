import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { FormattedMessage } from "react-intl";
import { Card } from "../../ui/card/Card";
import { Map } from "../../ui/map/Map";
import { LocationBar } from "../shared/location-bar/LocationBar";
import { AuthenticatedLayoutTwoColumns } from "../../ui/layouts/authenticated/AuthenticatedLayoutTwoColumns";
import { useLiveTracking } from "./useLiveTracking";
import { MapPin } from "../../ui/map/MapPin";
import { MapCenter } from "../../ui/map/MapCenter";

export function AssetLiveTrackingPage() {
  const currentAsset = useCurrentAsset();
  // const { location } = useLiveTracking();

  return (
    <AuthenticatedLayoutTwoColumns>
      {currentAsset.data?.location ? (
        <>
          <Card className="p-2">
            <LocationBar location={currentAsset.data.location} />
          </Card>
          <Card className="flex flex-grow">
            <Map
              center={{
                latitude: 0,
                longitude: 0
              }}
              initialZoom={16}>
              {currentAsset.data.location && (
                <>
                  <MapPin
                    latitude={currentAsset.data.location.latitude}
                    longitude={currentAsset.data.location.longitude}
                  />
                  <MapCenter
                    latitude={currentAsset.data.location.latitude}
                    longitude={currentAsset.data.location.longitude}
                  />
                </>
              )}
            </Map>
          </Card>
        </>
      ) : (
        <Card className="flex p-2 text-sm">
          <FormattedMessage id="no-location-data" />
        </Card>
      )}
    </AuthenticatedLayoutTwoColumns>
  );
}
