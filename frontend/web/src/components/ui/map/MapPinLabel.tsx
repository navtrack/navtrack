import { PinIcon } from "./PinIcon";
import { MapCustomMarker } from "./MapCustomMarker";
import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { FeMapPin } from "@navtrack/shared/maps";

type MapPinProps = {
  pin: FeMapPin;
  onClick?: () => void;
};

export function MapPinLabel(props: MapPinProps) {
  return (
    <MapCustomMarker
      coordinates={props.pin.coordinates}
      follow={props.pin.follow}>
      <div
        className={classNames(
          "flex w-24 flex-col items-center",
          c(props.onClick !== undefined, "cursor-pointer")
        )}
        onClick={props.onClick}>
        <div className="mb-1 rounded-xl bg-white/80 px-2.5 py-1.5 text-xs font-semibold text-gray-900 shadow-md drop-shadow-md">
          {props.pin.label}
        </div>
        <PinIcon color={props.pin.color} />
      </div>
    </MapCustomMarker>
  );
}
