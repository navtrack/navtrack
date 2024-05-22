import { useDateTime } from "@navtrack/shared/hooks/util/useDateTime";
import { useDistance } from "@navtrack/shared/hooks/util/useDistance";
import { PositionModel } from "@navtrack/shared/api/model/generated";
import {
  showCoordinate,
  showHeading,
  showProperty
} from "@navtrack/shared/utils/coordinates";
import { PositionBarItem } from "./PositionBarItem";

type PositionBarProps = {
  position: PositionModel;
};

export function PositionBar(props: PositionBarProps) {
  const { showDateTime } = useDateTime();
  const { showSpeed, showAltitude } = useDistance();

  return (
    <>
      <PositionBarItem
        label="generic.date"
        value={showDateTime(props.position.date)}
      />
      <PositionBarItem
        label="generic.latitude"
        value={showCoordinate(props.position.latitude)}
      />
      <PositionBarItem
        label="generic.longitude"
        value={showCoordinate(props.position.longitude)}
      />
      <PositionBarItem
        label="generic.speed"
        value={showSpeed(props.position.speed)}
      />
      <PositionBarItem
        label="generic.altitude"
        value={showAltitude(props.position.altitude)}
      />
      <PositionBarItem
        label="generic.heading"
        value={showHeading(props.position.heading)}
      />
      <PositionBarItem
        label="generic.satellites"
        value={showProperty(props.position.satellites)}
      />
      <PositionBarItem
        label="generic.hdop"
        value={showProperty(props.position.hdop)}
      />
    </>
  );
}
