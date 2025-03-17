import { Icon } from "../icon/Icon";
import { classNames } from "@navtrack/shared/utils/tailwind";
import { LatLong } from "@navtrack/shared/api/model";
import { faGoogle } from "@fortawesome/free-brands-svg-icons";

type GoogleMapsIconLinkProps = {
  className?: string;
  coordinates: LatLong;
};

export function GoogleMapsIconLink(props: GoogleMapsIconLinkProps) {
  return (
    <a
      href={`https://google.com/maps?q=${props.coordinates.latitude},${props.coordinates.longitude}`}
      target="_blank"
      rel="noreferrer"
      className={classNames(props.className)}>
      <Icon icon={faGoogle} />
    </a>
  );
}
