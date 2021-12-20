import React, { ReactNode } from "react";

type ITextInputLeftAddon = {
  children: ReactNode;
};

export default function TextInputLeftAddon(props: ITextInputLeftAddon) {
  return (
    <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
      <span className="text-gray-500 text-sm">{props.children}</span>
    </div>
  );
}
