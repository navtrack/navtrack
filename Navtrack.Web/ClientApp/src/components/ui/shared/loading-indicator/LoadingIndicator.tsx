import Icon from "../icon/Icon";
import { faSpinner } from "@fortawesome/free-solid-svg-icons";
import classNames from "classnames";
import { SizeProp } from "@fortawesome/fontawesome-svg-core";

export interface ILoadingIndicator {
  size?: SizeProp;
  className?: string;
}

export default function LoadingIndicator(props: ILoadingIndicator) {
  return (
    <div className={classNames(props.className, "text-center")}>
      <Icon icon={faSpinner} spin size={props.size} />
    </div>
  );
}
