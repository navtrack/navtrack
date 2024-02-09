import { Popover } from "@headlessui/react";
import { useMemo, useRef, useState } from "react";
import { SelectOption } from "../select/Select";
import { TextInput } from "../text-input/TextInput";
import { Icon } from "../../icon/Icon";
import { faChevronDown, faChevronUp } from "@fortawesome/free-solid-svg-icons";
import { TextInputRightAddon } from "../text-input/TextInputRightAddon";
import { FormattedMessage } from "react-intl";
import { c, classNames } from "@navtrack/shared/utils/tailwind";

export type AutocompleteProps = {
  name?: string;
  label?: string;
  placeholder?: string;
  value?: string;
  options: SelectOption[];
  onChange?: (value: string) => void;
  loading?: boolean;
};

export function Autocomplete(props: AutocompleteProps) {
  const [open, setOpen] = useState(false);
  const [selectedOption, setSelectedOption] = useState(
    props.options.find((o) => o.value === props.value)
  );
  const [search, setSearch] = useState("");

  const filteredOptions = useMemo(
    () =>
      search === ""
        ? props.options
        : props.options.filter((option) =>
            option.label
              .toLowerCase()
              .replace(/\s+/g, "")
              .includes(search.toLowerCase().replace(/\s+/g, ""))
          ),
    [props.options, search]
  );

  const ref = useRef<HTMLDivElement>(null);

  return (
    <div className="relative">
      <Popover>
        <TextInput
          className={classNames(
            "pr-9",
            c(selectedOption?.label, "placeholder:text-gray-900", "")
          )}
          value={search}
          label={props.label}
          onClick={() => setOpen(true)}
          placeholder={open ? "" : selectedOption?.label || "Type to search"}
          onChange={(e) => {
            setSearch(e.target.value);
            ref.current?.scrollTo(0, 0);
          }}
          onBlur={() => {
            setOpen(false);
            setSearch("");
          }}
          loading={props.loading}
          rightAddon={
            <TextInputRightAddon className="pointer-events-auto pr-0">
              <div
                className="py-1 pl-1 pr-3 hover:cursor-pointer"
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
            className="absolute left-0 right-0 z-10 mt-1 max-h-60 overflow-y-scroll rounded-md bg-white py-1 text-sm font-medium shadow-md ring-1 ring-inset ring-gray-300"
            ref={ref}>
            {filteredOptions.map((option) => (
              <div
                key={option.value}
                className="px-4 py-1.5 hover:cursor-pointer hover:bg-blue-500 hover:text-white"
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
