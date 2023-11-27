import { IconProp, SizeProp } from "@fortawesome/fontawesome-svg-core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

interface IIconsProps {
  icon: IconProp;
  spin?: boolean;
  hidden?: boolean;
  size?: SizeProp;
  onClick?: React.MouseEventHandler<SVGSVGElement>;
  className?: string;
}

export function Icon(props: IIconsProps) {
  return !props.hidden ? (
    <FontAwesomeIcon {...props} onClick={props.onClick} />
  ) : null;
}
