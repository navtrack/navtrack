import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { FormattedMessage } from "react-intl";
import { Card } from "../../ui/card/Card";
import { Map } from "../../ui/map/Map";
import { LocationBar } from "../shared/location-bar/LocationBar";
import { LiveTrackingMapPin } from "./LiveTrackingMapPin";
import { AuthenticatedLayoutTwoColumns } from "../../ui/layouts/authenticated/AuthenticatedLayoutTwoColumns";

export function AssetLiveTrackingPage() {
  const currentAsset = useCurrentAsset();

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
              zoom={16}>
              <LiveTrackingMapPin />
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
