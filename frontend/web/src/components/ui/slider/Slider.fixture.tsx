import { useValue } from "react-cosmos/client";
import { Slider } from "./Slider";

export default function Fixture() {
  const [value, setValue] = useValue<number>("slider", { defaultValue: 25 });

  return (
    <Slider
      min={0}
      max={100}
      value={value}
      onChange={(value) => setValue(value)}
    />
  );
}
