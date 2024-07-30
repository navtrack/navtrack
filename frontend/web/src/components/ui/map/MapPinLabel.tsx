import { PinIcon } from "./PinIcon";
import { LatLongModel } from "@navtrack/shared/api/model/generated";
import { MapCustomMarker } from "./MapCustomMarker";
import { c, classNames } from "@navtrack/shared/utils/tailwind";

type MapPinProps = {
  coordinates?: LatLongModel;
  follow?: boolean;
  label: string;
  onClick?: () => void;
  color?: "primary" | "green" | "red";
};

export function MapPinLabel(props: MapPinProps) {
  return (
    <MapCustomMarker coordinates={props.coordinates} follow={props.follow}>
      <div
        className={classNames(
          "flex w-24 flex-col items-center",
          c(props.onClick !== undefined, "cursor-pointer")
        )}
        onClick={props.onClick}>
        <div className="mb-1 rounded-xl bg-white/80 px-2.5 py-1.5 text-xs font-semibold text-gray-900 shadow-md drop-shadow-md">
          {props.label}
        </div>
        <PinIcon color={props.color} />
      </div>
    </MapCustomMarker>
  );
}
