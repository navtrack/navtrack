import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { FormattedMessage } from "react-intl";
import { Card } from "../../ui/card/Card";
import { Map } from "../../ui/map/Map";
import { LocationBar } from "../shared/location-bar/LocationBar";
import { AuthenticatedLayoutTwoColumns } from "../../ui/layouts/authenticated/AuthenticatedLayoutTwoColumns";
import { MapPin } from "../../ui/map/MapPin";

export function AssetLiveTrackingPage() {
  const currentAsset = useCurrentAsset();
  const location = currentAsset.data?.position;

  return (
    <AuthenticatedLayoutTwoColumns>
      {location ? (
        <>
          <Card className="p-2">
            <LocationBar location={location} />
          </Card>
          <Card className="flex flex-grow">
            <Map center={{ ...location }}>
              <MapPin location={{ ...location }} follow />
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
