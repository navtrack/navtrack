import { PositionDataModel } from "@navtrack/shared/api/model";
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
  position: PositionDataModel;
};

export function PositionCardItems(props: PositionCardItemsProps) {
  const show = useShow();

  return (
    <>
      <PositionCardItem
        label="date"
        value={show.dateTime(props.position.date)}
      />
      <PositionCardItem
        label="latitude-longitude"
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
        label="speed"
        value={show.speed(props.position.speed)}
      />
      <PositionCardItem
        label="altitude"
        value={show.altitude(props.position.altitude)}
      />
      <PositionCardItem
        label="heading"
        value={showHeading(props.position.heading)}
      />
      <PositionCardItem
        label="satellites"
        value={showProperty(props.position.satellites)}
      />
      <PositionCardItem
        label="hdop"
        value={showNumber(props.position.hdop, 2)}
      />
    </>
  );
}
