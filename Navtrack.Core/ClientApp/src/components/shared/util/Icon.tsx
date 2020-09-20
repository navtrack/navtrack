import React from "react";
import classNames from "classnames";
import { ClassDictionary } from "classnames/types";

type Props = {
  show?: boolean;
  margin?: number;
  className?: string;
};

export default function Icon(props: Props) {
  let show = props.show === undefined || props.show;

  let classes: ClassDictionary = {};

  if (props.margin && props.margin > 0) {
    classes[`mr-${props.margin}`] = true;
  }

  return show ? <i className={classNames("fa", classes, props.className)} /> : <></>;
}
