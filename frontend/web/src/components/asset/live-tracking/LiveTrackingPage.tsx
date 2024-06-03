import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { FormattedMessage } from "react-intl";
import { Card } from "../../ui/card/Card";
import { Map } from "../../ui/map/Map";
import { PositionBar } from "../shared/position-bar/PositionBar";
import { MapPin } from "../../ui/map/MapPin";
import { CardMapWrapper } from "../../ui/map/CardMapWrapper";
import { MapFollowControl } from "../../ui/map/MapFollowControl";
import { useContext } from "react";
import { SlotContext } from "../../../app/SlotContext";

export function AssetLiveTrackingPage() {
  const currentAsset = useCurrentAsset();
  const position = currentAsset.data?.position;
  const slots = useContext(SlotContext);

  return (
    <>
      {position ? (
        <>
          <Card className="flex justify-between px-3 py-2">
            <PositionBar position={position} />
            {slots?.assetLiveTrackingPositionCardExtraItems?.(position)}
          </Card>
          <Card className="flex flex-grow">
            <CardMapWrapper>
              <Map center={{ ...position }}>
                <MapPin position={{ ...position }} />
                <MapFollowControl position={{ ...position }} />
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
