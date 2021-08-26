import classNames, { Mapping } from "classnames";

type Props = {
  show?: boolean;
  margin?: number;
  className?: string;
};

export default function Icon(props: Props) {
  let show = props.show === undefined || props.show;

  let classes: Mapping = {};

  if (props.margin && props.margin > 0) {
    classes[`mr-${props.margin}`] = true;
  }

  return show ? <i className={classNames("fa", classes, props.className)} /> : <></>;
}
