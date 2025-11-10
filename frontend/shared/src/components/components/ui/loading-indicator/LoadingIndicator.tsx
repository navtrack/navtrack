import { Icon } from "../../../../../../web/src/components/ui/icon/Icon";
import { faSpinner } from "@fortawesome/free-solid-svg-icons";
import { SizeProp } from "@fortawesome/fontawesome-svg-core";
import { classNames } from "../../../../utils/tailwind";

type LoadingIndicatorProps = {
  size?: SizeProp;
  className?: string;
  isLoading?: boolean;
};

export function LoadingIndicator(props: LoadingIndicatorProps) {
  return (
    <>
      {(props.isLoading === true || props.isLoading === undefined) && (
        <div className={classNames(props.className, "text-center")}>
          <Icon icon={faSpinner} spin size={props.size} />
        </div>
      )}
    </>
  );
}
