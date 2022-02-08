import {
  faAngleDown,
  faAngleUp,
  faTimes
} from "@fortawesome/free-solid-svg-icons";
import { Popover, Transition } from "@headlessui/react";
import { c, useHTMLElementSize } from "@navtrack/navtrack-app-shared";
import classNames from "classnames";
import { FocusEventHandler, Fragment, useMemo, useRef, useState } from "react";
import { FormattedMessage } from "react-intl";
import Icon from "../icon/Icon";
import TextInput from "../text-input/TextInput";
import TextInputRightAddon from "../text-input/TextInputRightAddon";

export interface ISelectInputItem {
  id: string;
  label: string;
}

export interface ISelectInput {
  name?: string;
  label?: string;
  placeholder?: string;
  value?: string;
  items: ISelectInputItem[];
  onChange?: (value: string) => void;
  onBlur?: FocusEventHandler<HTMLInputElement>;
  error?: string;
}

export default function SelectInput(props: ISelectInput) {
  const [selectedItem, setSelectedItem] = useState(
    props.items.find((item) => item.id === props.value)
  );

  const [searchText, setSearchText] = useState(selectedItem?.label);
  const inputEl = useRef(null);
  const inputSize = useHTMLElementSize(inputEl);

  const filteredItems = useMemo(
    () =>
      searchText !== undefined && selectedItem === undefined
        ? props.items.filter((x) =>
            x.label.toLowerCase().includes(searchText.toLowerCase())
          )
        : props.items,
    [props.items, searchText, selectedItem]
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
              value={searchText}
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
                      onClick={(e) => {
                        e.stopPropagation();
                        setSelectedItem(undefined);
                        setSearchText("");
                        props.onChange?.("");
                      }}>
                      <Icon icon={faTimes} className="mr-4" />
                    </div>
                  ) : null}
                  <Icon
                    icon={open ? faAngleUp : faAngleDown}
                    size="lg"
                    className="pointer-events-auto cursor-pointer hover:text-gray-900"
                  />
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
              <div className="grid max-h-72 gap-2 overflow-scroll rounded-lg bg-white py-2 shadow-lg ring-1 ring-black ring-opacity-5">
                {filteredItems.length > 0 ? (
                  filteredItems.map((item) => (
                    <div
                      key={item.id}
                      className={classNames(
                        "cursor-pointer px-4 py-2 text-sm text-gray-700 hover:bg-gray-100",
                        c(item.id === selectedItem?.id, "bg-gray-100")
                      )}
                      onClick={() => {
                        setSearchText(item.label);
                        setSelectedItem(item);
                        props.onChange?.(item.id);
                        close();
                      }}>
                      {item.label}
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
