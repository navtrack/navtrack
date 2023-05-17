import { Icon } from "../icon/Icon";
import { faSpinner } from "@fortawesome/free-solid-svg-icons";
import { SizeProp } from "@fortawesome/fontawesome-svg-core";
import { classNames } from "@navtrack/shared/utils/tailwind";

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
