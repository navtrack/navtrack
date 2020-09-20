import React, { ReactNode } from "react";

export type Props<T extends object> = {
  children: ReactNode;
};

export default function ScrollableTable<T extends object>(props: Props<T>) {
  return <div>{props.children}</div>;
}
