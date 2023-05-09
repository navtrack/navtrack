import classNames from "classnames";
import React, { ReactNode } from "react";

type ITextInputRightAddon = {
  children: ReactNode;
  className?: string;
};

export function TextInputRightAddon(props: ITextInputRightAddon) {
  return (
    <div
      className={classNames(
        "pointer-events-none absolute inset-y-0 right-0 flex items-center pr-3",
        props.className
      )}>
      <span className="flex items-center text-sm text-gray-500">
        {props.children}
      </span>
    </div>
  );
}
