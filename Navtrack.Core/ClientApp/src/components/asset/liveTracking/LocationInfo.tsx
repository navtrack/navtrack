import React, { ReactNode } from "react";

export type Props = {
  title: string,
  children: ReactNode | string,
  hideMargin?: boolean
}

export function LocationInfo(props: Props) {
  return (
    <div className={`mr-5`}>
      <div className="text-xs uppercase font-semibold">{props.title}</div>
      <div className="text-sm">{props.children}</div>
    </div>
  );
};