import { IconProp,  } from "@fortawesome/fontawesome-svg-core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

type NtwIconProps ={
  icon: IconProp;
  className?: string;
}

export function NtwIcon(props: NtwIconProps) {
  return <FontAwesomeIcon {...props}  />
}
