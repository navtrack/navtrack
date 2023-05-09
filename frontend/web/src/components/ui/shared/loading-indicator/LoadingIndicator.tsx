import { Icon } from "../icon/Icon";
import { faSpinner } from "@fortawesome/free-solid-svg-icons";
import classNames from "classnames";
import { SizeProp } from "@fortawesome/fontawesome-svg-core";

type LoadingIndicatorProps = {
  size?: SizeProp;
  className?: string;
};

export function LoadingIndicator(props: LoadingIndicatorProps) {
  return (
    <div className={classNames(props.className, "text-center")}>
      <Icon icon={faSpinner} spin size={props.size} />
    </div>
  );
}
