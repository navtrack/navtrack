import { Switch as HeadlessSwitch } from "@headlessui/react";
import { classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode, useState } from "react";

type SwitchProps = {
  checked: boolean;
  onChange?: (checked: boolean) => void;
  children?: ReactNode;
};

export function Switch(props: SwitchProps) {
  const [checked, setChecked] = useState(props.checked);

  return (
    <HeadlessSwitch.Group as="div" className="flex items-center">
      <HeadlessSwitch
        checked={checked}
        onChange={(checked) => {
          setChecked(checked);
          props.onChange?.(checked);
        }}
        className={classNames(
          checked ? "bg-blue-700" : "bg-gray-200",
          "relative inline-flex h-6 w-11 flex-shrink-0 cursor-pointer rounded-full border-2 border-transparent transition-colors duration-200 ease-in-out focus:outline-none focus:ring-2 focus:ring-blue-700 focus:ring-offset-2"
        )}>
        <span
          aria-hidden="true"
          className={classNames(
            checked ? "translate-x-5" : "translate-x-0",
            "pointer-events-none inline-block h-5 w-5 transform rounded-full bg-white shadow ring-0 transition duration-200 ease-in-out"
          )}
        />
      </HeadlessSwitch>
      <HeadlessSwitch.Label as="span" className="ml-3 text-sm">
        {props.children}
      </HeadlessSwitch.Label>
    </HeadlessSwitch.Group>
  );
}
