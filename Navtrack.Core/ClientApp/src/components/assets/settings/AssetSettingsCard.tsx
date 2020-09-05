import React, { ReactNode } from "react";

type Props = {
  title: string;
  children: ReactNode;
  className?: string;
};

export default function AssetSettingsCard(props: Props) {
  return (
    <div className={props.className}>
      <div className="text-xl font-medium mb-2 border-b pb-1">{props.title}</div>
      <div>{props.children}</div>
    </div>
  );
}
