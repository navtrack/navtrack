import { faCopy } from "@fortawesome/free-regular-svg-icons";
import { Icon } from "../icon/Icon";
import { classNames } from "@navtrack/shared/utils/tailwind";

type CopyProps = {
  value: string;
  className?: string;
};

export function Copy(props: CopyProps) {
  return (
    <span
      onClick={() => navigator.clipboard.writeText(props.value)}
      className={classNames(props.className, "cursor-pointer")}>
      <Icon icon={faCopy} />
    </span>
  );
}
