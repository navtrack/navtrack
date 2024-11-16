import { useDateTime } from "@navtrack/shared/hooks/util/useDateTime";
import { useDistance } from "@navtrack/shared/hooks/util/useDistance";
import { MessagePosition } from "@navtrack/shared/api/model/generated";
import {
  showCoordinate,
  showHeading,
  showProperty
} from "@navtrack/shared/utils/coordinates";
import { PositionCardItem } from "./PositionCardItem";

type PositionCardItemsProps = {
  position: MessagePosition;
};

export function PositionCardItems(props: PositionCardItemsProps) {
  const { showDateTime } = useDateTime();
  const { showSpeed, showAltitude } = useDistance();

  return (
    <>
      <PositionCardItem
        label="generic.date"
        value={showDateTime(props.position.date)}
      />
      <PositionCardItem
        label="generic.latitude"
        value={showCoordinate(props.position.coordinates.latitude)}
      />
      <PositionCardItem
        label="generic.longitude"
        value={showCoordinate(props.position.coordinates.longitude)}
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
        value={showProperty(props.position.hdop)}
      />
    </>
  );
}
