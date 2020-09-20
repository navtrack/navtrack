import React, { ReactNode } from "react";

export type Props = {
  children: ReactNode;
};

export default function ScrollableTableBody(props: Props) {
  return (
    <div className="overflow-scroll" style={{ height: "135px" }} id="scrollableTableContent">
      {props.children}
    </div>
  );
}
