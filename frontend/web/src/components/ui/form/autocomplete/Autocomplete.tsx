import { Popover } from "@headlessui/react";
import { useMemo, useRef, useState } from "react";
import { SelectOption } from "../select/Select";
import { TextInput } from "../text-input/TextInput";
import { Icon } from "../../icon/Icon";
import { faChevronDown, faChevronUp } from "@fortawesome/free-solid-svg-icons";
import { TextInputRightAddon } from "../text-input/TextInputRightAddon";
import { FormattedMessage } from "react-intl";
import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { useOnChange } from "@navtrack/shared/hooks/util/useOnChange";
import { ZINDEX_MENU } from "../../../../constants";

export type AutocompleteProps = {
  name?: string;
  label?: string;
  placeholder?: string;
  value?: string;
  options?: SelectOption[];
  onChange?: (value: string) => void;
  loading?: boolean;
  open?: boolean;
  size?: "xs" | "sm";
  error?: string;
};

export function Autocomplete(props: AutocompleteProps) {
  const [open, setOpen] = useState(props.open);
  const [selectedOption, setSelectedOption] = useState(
    props.options?.find((o) => o.value === props.value)
  );
  const [search, setSearch] = useState("");

  useOnChange(props.value, () => {
    if (props.options !== undefined) {
      setSelectedOption(props.options?.find((o) => o.value === props.value));
    }
  });

  useOnChange(props.options, () => {
    if (props.options !== undefined) {
      setSelectedOption(props.options.find((o) => o.value === props.value));
    }
  });

  const filteredOptions = useMemo(
    () =>
      (search === ""
        ? props.options
        : props.options?.filter((option) =>
            option.label
              .toLowerCase()
              .replace(/\s+/g, "")
              .includes(search.toLowerCase().replace(/\s+/g, ""))
          )) ?? [],
    [props.options, search]
  );

  const ref = useRef<HTMLDivElement>(null);

  return (
    <div className="relative">
      <Popover>
        <TextInput
          size={props.size}
          className={classNames(
            "pr-9",
            c(selectedOption?.label, "placeholder:text-gray-900", "")
          )}
          value={search}
          label={props.label}
          onClick={() => setOpen(true)}
          error={props.error}
          placeholder={
            open
              ? ""
              : (selectedOption?.label ?? props.placeholder ?? "Type to search")
          }
          onChange={(e) => {
            setSearch(e.target.value);
            ref.current?.scrollTo(0, 0);
            if (e.target.value) {
              setOpen(true);
            }
          }}
          onBlur={() => {
            setOpen(false);
            setSearch("");
          }}
          loading={props.loading}
          rightAddon={
            <TextInputRightAddon className="pointer-events-auto mr-0">
              <div
                className="py-1 hover:cursor-pointer"
                onClick={(e) => {
                  e.preventDefault();
                  e.nativeEvent.stopImmediatePropagation();
                  e.nativeEvent.stopPropagation();
                  e.stopPropagation();
                  setOpen(!open);
                }}>
                <Icon icon={open ? faChevronUp : faChevronDown} />
              </div>
            </TextInputRightAddon>
          }
        />
        <Popover.Panel static={open}>
          <div
            className="absolute left-0 right-0 mt-1 max-h-56 overflow-y-scroll rounded-md border border-gray-300 bg-white py-1 text-sm font-medium shadow-md"
            style={{ zIndex: ZINDEX_MENU }}
            ref={ref}>
            {filteredOptions.map((option) => (
              <div
                key={option.value}
                className={classNames(
                  "px-4 py-1.5 ring-inset hover:cursor-pointer",
                  c(
                    option.value === props.value,
                    "bg-gray-800 text-white",
                    "text-gray-900 hover:bg-gray-100"
                  )
                )}
                onMouseDown={() => {
                  setSelectedOption(option);
                  setOpen(false);
                  setSearch("");
                  props.onChange?.(option.value);
                }}>
                {option.label}
              </div>
            ))}
            {filteredOptions.length === 0 && (
              <div className="px-4 py-1.5">
                <FormattedMessage id="ui.autocomplete.no-items" />
              </div>
            )}
          </div>
        </Popover.Panel>
      </Popover>
    </div>
  );
}
