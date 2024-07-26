import { useState } from "react";

type SliderProps = {
  onChange?: (value: number) => void;
  value?: number;
  min?: number;
  max?: number;
  step?: number;
  onMouseDown?: (value: number) => void;
  onMouseUp?: (value: number) => void;
};

export function Slider(props: SliderProps) {
  const [value, setValue] = useState(props.value);

  return (
    <div className="w-full">
      <div>
        <input
          className="my-2 h-2 w-full appearance-none rounded-md bg-gray-200 accent-gray-900"
          type="range"
          min={props.min}
          max={props.max}
          step={props.step}
          value={value}
          onChange={(e) => {
            setValue(e.target.value as unknown as number);
            props.onChange?.(e.target.value as unknown as number);
          }}
          onMouseUp={() =>
            value !== undefined ? props.onMouseUp?.(value) : undefined
          }
          onMouseDown={() =>
            value !== undefined ? props.onMouseDown?.(value) : undefined
          }
        />
      </div>
    </div>
  );
}
