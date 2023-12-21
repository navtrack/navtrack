import { useValue } from "react-cosmos/client";
import { Slider } from "./Slider";

export default function Fixture() {
  const [value, setValue] = useValue<number>("slider", { defaultValue: 25 });
  return (
    <Slider
      min={0}
      max={100}
      value={value}
      onMouseDown={() => {
        // For some reason onChange only fires when onMouseDown is defined
      }}
      onChange={(_, value) => {
        setValue(value as number);
      }}
    />
  );
}
