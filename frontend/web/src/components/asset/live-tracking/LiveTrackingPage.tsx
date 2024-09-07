import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { FormattedMessage } from "react-intl";
import { Card } from "../../ui/card/Card";
import { Map } from "../../ui/map/Map";
import { PositionCardItems } from "../shared/position-card/PositionCardItems";
import { MapPin } from "../../ui/map/MapPin";
import { CardMapWrapper } from "../../ui/map/CardMapWrapper";
import { MapFollowControl } from "../../ui/map/MapFollowControl";
import { useContext } from "react";
import { SlotContext } from "../../../app/SlotContext";

export function AssetLiveTrackingPage() {
  const currentAsset = useCurrentAsset();
  const position = currentAsset.data?.lastPositionMessage?.position;
  const slots = useContext(SlotContext);

  return (
    <>
      {position ? (
        <>
          <Card className="space-y-2 px-3 py-2">
            <div className="flex justify-between">
              <PositionCardItems position={position} />
            </div>
            {slots?.assetLiveTrackingPositionCardExtraItems?.(position)}
          </Card>
          <Card className="flex flex-grow">
            <CardMapWrapper>
              <Map center={position.coordinates}>
                <MapPin pin={{ coordinates: position.coordinates }} />
                <MapFollowControl position={position.coordinates} />
              </Map>
            </CardMapWrapper>
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
