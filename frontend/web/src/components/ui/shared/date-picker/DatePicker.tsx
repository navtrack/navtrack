import { Popover } from "@headlessui/react";
import { LocalizationProvider, StaticDatePicker } from "@mui/lab";
import AdapterDateFns from "@mui/lab/AdapterDateFns";
import { styled, TextField, TextFieldProps } from "@mui/material";
import { c } from "@navtrack/ui-shared/utils/tailwind";
import classNames from "classnames";
import { format } from "date-fns";
import { useState } from "react";
import { usePopper } from "react-popper";
import TextInput from "../text-input/TextInput";

interface IDatePicker {
  value: Date | null;
  onChange: (date: Date | null) => void;
  disabled?: boolean;
  placement?: "top-start" | "bottom-start";
  label?: string;
}

const StyledDatePickerContainer = styled("div")`
  .PrivatePickersSlideTransition-root {
    min-height: 244px;
  }

  .MuiTouchRipple-root {
    display: none;
  }

  .MuiPickersDay-root {
    border-radius: 10%;
    font-family: inherit;
    -webkit-transition: none;
    transition: none;

    &:active,
    &:hover,
    &:focus {
      background-color: rgba(55, 65, 81, 1);
      color: white;
    }
  }
  .Mui-selected {
    border-radius: 10%;
    background-color: rgba(55, 65, 81, 1);
  }
  .Mui-selected:hover {
    border-radius: 10%;
    background-color: rgba(55, 65, 81, 1);
  }
`;

export default function DatePicker(props: IDatePicker) {
  const [referenceElement, setReferenceElement] =
    useState<HTMLButtonElement | null>();
  const [popperElement, setPopperElement] = useState<HTMLDivElement | null>();
  const { styles, attributes } = usePopper(referenceElement, popperElement, {
    placement: props.placement ? props.placement : "bottom-start"
  });

  return (
    <Popover>
      <Popover.Button ref={setReferenceElement} disabled={props.disabled}>
        <TextInput
          label={props.label}
          value={format(props.value as Date, "PP")}
          readOnly
          className={c(!props.disabled, "cursor-pointer")}
          disabled={props.disabled}
        />
      </Popover.Button>
      <Popover.Panel
        ref={setPopperElement}
        style={styles.popper}
        {...attributes.popper}
        className={classNames(
          "overflow-hidden rounded-lg shadow-lg",
          c(
            props.placement === "bottom-start" || props.placement === undefined,
            "mt-2"
          ),
          c(props.placement === "top-start", "mb-2")
        )}>
        {({ close }) => (
          <LocalizationProvider dateAdapter={AdapterDateFns}>
            <StyledDatePickerContainer>
              <StaticDatePicker
                displayStaticWrapperAs="desktop"
                value={props.value}
                onChange={(newValue: Date | null) => {
                  if (newValue?.getDate() !== props.value?.getDate()) {
                    close();
                  }
                  props.onChange(newValue);
                }}
                renderInput={(
                  params: JSX.IntrinsicAttributes & TextFieldProps
                ) => <TextField {...params} />}
              />
            </StyledDatePickerContainer>
          </LocalizationProvider>
        )}
      </Popover.Panel>
    </Popover>
  );
}
