import { LatLong } from "../../../api/model";
import { useGeocodeReverseQuery } from "../../../hooks/queries/geocode/useGeocodeReverseQuery";
import { Skeleton } from "../ui/skeleton/Skeleton";
import { c, classNames } from "../../../utils/tailwind";
import { useOnScreen } from "./useOnScreen";

type GeocodeReverseProps = {
  coordinates?: LatLong;
};

export function GeocodeReverse(props: GeocodeReverseProps) {
  const onScreen = useOnScreen();
  const location = useGeocodeReverseQuery({
    lat: props.coordinates?.latitude,
    lon: props.coordinates?.longitude,
    isEnabled: onScreen.isVisible
  });

  return (
    <div ref={onScreen.ref}>
      <Skeleton isLoading={location.isLoading}>
        <div className={classNames(c(location.isLoading, "h-4"))}>
          {location.data?.displayName ?? ""}
        </div>
      </Skeleton>
    </div>
  );
}
