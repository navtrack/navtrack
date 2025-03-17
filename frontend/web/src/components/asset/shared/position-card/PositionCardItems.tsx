import { useDistance } from "@navtrack/shared/hooks/util/useDistance";
import { MessagePosition } from "@navtrack/shared/api/model";
import {
  showCoordinate,
  showHeading,
  showNumber,
  showProperty
} from "@navtrack/shared/utils/coordinates";
import { PositionCardItem } from "./PositionCardItem";
import { GoogleMapsIconLink } from "../../../ui/helpers/GoogleMapsIconLink";
import { useShow } from "@navtrack/shared/hooks/util/useShow";

type PositionCardItemsProps = {
  position: MessagePosition;
};

export function PositionCardItems(props: PositionCardItemsProps) {
  const show = useShow();
  const { showSpeed, showAltitude } = useDistance();

  return (
    <>
      <PositionCardItem
        label="generic.date"
        value={show.dateTime(props.position.date)}
      />
      <PositionCardItem
        label="generic.latitude-longitude"
        value={`${showCoordinate(props.position.coordinates.latitude)}, ${showCoordinate(props.position.coordinates.longitude)}`}
        copyable
        labelExtra={
          <GoogleMapsIconLink
            coordinates={props.position.coordinates}
            className="ml-2"
          />
        }
      />
      <PositionCardItem
        label="generic.speed"
        value={showSpeed(props.position.speed)}
      />
      <PositionCardItem
        label="generic.altitude"
        value={showAltitude(props.position.altitude)}
      />
      <PositionCardItem
        label="generic.heading"
        value={showHeading(props.position.heading)}
      />
      <PositionCardItem
        label="generic.satellites"
        value={showProperty(props.position.satellites)}
      />
      <PositionCardItem
        label="generic.hdop"
        value={showNumber(props.position.hdop, 2)}
      />
    </>
  );
}
