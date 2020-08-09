import React, { ReactNode } from "react";

type Props = {
  title: string;
  children: ReactNode;
  className?: string;
};

export default function AssetSettingsCard(props: Props) {
  return (
    <div className={props.className}>
      <div className="text-xl mb-1 pb-2 border-b">{props.title}</div>
      <div className="pt-2 pb-4">{props.children}</div>
    </div>
  );
}
