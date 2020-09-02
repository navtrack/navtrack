import React, { ReactNode } from "react";

type Props = {
  title: string;
  children: ReactNode;
  className?: string;
};

export default function AssetSettingsCard(props: Props) {
  return (
    <div className={props.className}>
      <div className="text-lg font-medium mb-2 border-b">{props.title}</div>
      <div>{props.children}</div>
    </div>
  );
}
