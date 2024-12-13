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
  return (
    <div className="w-full">
      <div>
        <input
          className="my-2 h-2 w-full appearance-none rounded-md bg-gray-200 accent-gray-900"
          type="range"
          min={props.min}
          max={props.max}
          step={props.step}
          value={props.value}
          onChange={(e) => {
            props.onChange?.(e.target.value as unknown as number);
          }}
          onMouseUp={() =>
            props.value !== undefined
              ? props.onMouseUp?.(props.value)
              : undefined
          }
          onMouseDown={() =>
            props.value !== undefined
              ? props.onMouseDown?.(props.value)
              : undefined
          }
        />
      </div>
    </div>
  );
}
