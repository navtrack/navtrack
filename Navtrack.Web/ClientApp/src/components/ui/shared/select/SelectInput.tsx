import {
  faAngleDown,
  faAngleUp,
  faTimes
} from "@fortawesome/free-solid-svg-icons";
import { Popover, Transition } from "@headlessui/react";
import classNames from "classnames";
import { FocusEventHandler, Fragment, useMemo, useRef, useState } from "react";
import { FormattedMessage } from "react-intl";
import useElementSize from "../../../../hooks/util/useElementSize";
import c from "../../../../utils/tailwind";
import Icon from "../icon/Icon";
import TextInput from "../text-input/TextInput";
import TextInputRightAddon from "../text-input/TextInputRightAddon";

export interface ISelectInput<T> {
  name?: string;
  label?: string;
  placeholder?: string;
  value?: T;
  items: T[];
  idKey: keyof T;
  labelKey: keyof T;
  onChange?: (value: string) => void;
  onBlur?: FocusEventHandler<HTMLInputElement>;
  error?: string;
}

export default function SelectInput<T>(props: ISelectInput<T>) {
  const [selectedItem, setSelectedItem] = useState<T | undefined>();
  const [searchText, setSearchText] = useState("");
  const inputEl = useRef(null);
  const inputSize = useElementSize(inputEl);

  const filteredItems = useMemo(
    () =>
      searchText !== ""
        ? props.items.filter((x) =>
            (x[props.labelKey] as unknown as string)
              .toLowerCase()
              .includes(searchText.toLowerCase())
          )
        : props.items,
    [props.items, props.labelKey, searchText]
  );

  return (
    <Popover>
      {({ open, close }) => (
        <>
          <Popover.Button as="div" ref={inputEl}>
            <TextInput
              name={props.name}
              label={props.label}
              placeholder={props.placeholder}
              value={
                searchText !== ""
                  ? searchText
                  : selectedItem !== undefined
                  ? (selectedItem[props.labelKey] as unknown as string)
                  : ""
              }
              disabled
              error={props.error}
              onChange={(e) => {
                setSelectedItem(undefined);
                setSearchText(e.target.value);
              }}
              onBlur={props.onBlur}
              rightAddon={
                <TextInputRightAddon>
                  {selectedItem !== undefined ? (
                    <div
                      className="pointer-events-auto cursor-pointer hover:text-gray-900"
                      onClick={() => {
                        setSelectedItem(undefined);
                        props.onChange?.("");
                      }}>
                      <Icon icon={faTimes} className="mr-4" />
                    </div>
                  ) : null}
                  <Icon icon={open ? faAngleUp : faAngleDown} size="lg" />
                </TextInputRightAddon>
              }
            />
          </Popover.Button>
          <Transition
            as={Fragment}
            enter="transition ease-out duration-200"
            enterFrom="opacity-0 translate-y-1"
            enterTo="opacity-100 translate-y-0"
            leave="transition ease-in duration-0"
            leaveFrom="opacity-100 translate-y-0"
            leaveTo="opacity-0 translate-y-1">
            <Popover.Panel
              as="div"
              className="absolute z-10 mt-2"
              style={{ width: inputSize.width }}>
              <div className="overflow-scroll rounded-lg shadow-lg max-h-72 grid gap-2 bg-white py-2 ring-1 ring-black ring-opacity-5">
                {filteredItems.length > 0 ? (
                  filteredItems.map((item) => (
                    <div
                      key={item[props.idKey] as unknown as string}
                      className={classNames(
                        "px-4 py-2 text-sm text-gray-700 cursor-pointer hover:bg-gray-100",
                        c(item === selectedItem, "bg-gray-100")
                      )}
                      onClick={() => {
                        setSearchText("");
                        setSelectedItem(item);
                        props.onChange?.(
                          `${item[props.idKey] as unknown as string}`
                        );
                        close();
                      }}>
                      {item[props.labelKey]}
                    </div>
                  ))
                ) : (
                  <div className={"px-4 py-2 text-sm text-gray-700"}>
                    <FormattedMessage id="ui.select.no-items" />
                  </div>
                )}
              </div>
            </Popover.Panel>
          </Transition>
        </>
      )}
    </Popover>
  );
}
