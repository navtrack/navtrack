import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { useState } from "react";
import { useIntl } from "react-intl";

export type ButtonGroupOption = {
  label: string;
  value: string;
};

type ButtonGroupProps = {
  options: ButtonGroupOption[];
  onChange?: (value: string) => void;
};

export function ButtonGroup(props: ButtonGroupProps) {
  const intl = useIntl();
  const [selectedIndex, setSelectedIndex] = useState(0);

  return (
    <div>
      <div className="border border-gray-300 space-x-px bg-gray-300 rounded-md text-sm overflow-hidden inline-flex text-gray-900 font-semibold shadow-xs">
        {props.options.map((option, index) => (
          <button
            onClick={() => {
              setSelectedIndex(index);
              props.onChange?.(option.value);
            }}
            key={option.value}
            type="button"
            className={classNames(
              "bg-gray-50 px-3 py-1 hover:bg-gray-200 cursor-pointer",
              c(selectedIndex === index, "bg-gray-200")
            )}>
            {intl.formatMessage({ id: option.label })}
          </button>
        ))}
      </div>
    </div>
  );
}
