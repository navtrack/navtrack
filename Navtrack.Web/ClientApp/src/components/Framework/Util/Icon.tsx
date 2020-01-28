import React from "react";

type Props = {
  icon: string,
  show?: boolean
}

export default function Icon(props: Props) {
  let show = props.show === undefined || props.show;

  return (show ? <i className={`fas ${props.icon} mr-2`} /> : <></>);
}