import React from "react";

type Props = {
  children: React.ReactNode;
};

export default function PageLayout(props: Props) {
  return <div className="p-5">{props.children}</div>;
}
