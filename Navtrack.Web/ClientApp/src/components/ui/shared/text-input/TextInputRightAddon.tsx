import classNames from "classnames";
import React, { ReactNode } from "react";

type ITextInputRightAddon = {
  children: ReactNode;
  className?: string;
};

export default function TextInputRightAddon(props: ITextInputRightAddon) {
  return (
    <div
      className={classNames(
        "absolute inset-y-0 right-0 pr-3 flex items-center pointer-events-none",
        props.className
      )}>
      <span className="text-gray-500 text-sm flex items-center">{props.children}</span>
    </div>
  );
}
