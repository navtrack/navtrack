import React, { ReactNode } from "react";

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export type Props<T extends object> = {
  children: ReactNode;
};

export default function ScrollableTable<T extends object>(props: Props<T>) {
  return <div>{props.children}</div>;
}
