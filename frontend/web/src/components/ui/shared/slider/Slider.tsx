import { Slider as MuiSlider, styled } from "@mui/material";

interface ISlider {
  onChange?: (
    event: Event,
    value: number | number[],
    activeThumb: number
  ) => void;
  value?: number | number[];
  min?: number;
  max?: number;
  step?: number;
  marks?: boolean;
  displayValueLabel?: "on" | "off" | "auto";
  onClick?: React.MouseEventHandler<HTMLSpanElement>;
  onMouseDown?: React.MouseEventHandler<HTMLSpanElement>;
}

const StyledSlider = styled(MuiSlider)`
  color: #1f2937;

  .MuiSlider-thumb {
    box-shadow: none;
  }
  .MuiSlider-thumb.Mui-active {
    box-shadow: none;
  }
  .MuiSlider-thumb.Mui-focusVisible {
    box-shadow: none;
  }

  .MuiSlider-rail {
    background-color: #e5e7eb;
    opacity: 1;
  }
  .MuiSlider-mark {
    background-color: #e5e7eb;
    opacity: 1;
  }
  .MuiSlider-markActive {
    background-color: #1f2937;
    opacity: 1;
  }
`;

export function Slider(props: ISlider) {
  return (
    <StyledSlider
      step={props.step}
      min={props.min}
      max={props.max}
      value={props.value}
      marks={props.marks}
      onChange={props.onChange}
      onMouseDown={props.onMouseDown}
      valueLabelDisplay={
        props.displayValueLabel ? props.displayValueLabel : "off"
      }
      disableSwap
    />
  );
}
