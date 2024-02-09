import { classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";

type TextInputRightAddonProps = {
  children: ReactNode;
  className?: string;
};

export function TextInputRightAddon(props: TextInputRightAddonProps) {
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
