import React, { ReactNode } from "react";

type ITextInputLeftAddon = {
  children: ReactNode;
};

export function TextInputLeftAddon(props: ITextInputLeftAddon) {
  return (
    <div className="pointer-events-none absolute inset-y-0 left-0 flex items-center pl-3">
      <span className="text-sm text-gray-500">{props.children}</span>
    </div>
  );
}
