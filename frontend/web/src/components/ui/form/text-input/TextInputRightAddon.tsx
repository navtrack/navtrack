import { ReactNode } from "react";

type TextInputRightAddonProps = {
  children: ReactNode;
};

export function TextInputRightAddon(props: TextInputRightAddonProps) {
  return (
    <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center pr-3">
      <span className="flex items-center text-sm text-gray-500">
        {props.children}
      </span>
    </div>
  );
}
