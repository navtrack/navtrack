import { useInput } from "react-cosmos/client";
import { Slider } from "./Slider";

export default function Fixture() {
  const [value, setValue] = useInput("slider", 25);

  return (
    <Slider
      min={0}
      max={100}
      value={value}
      onChange={(value) => setValue(value)}
    />
  );
}
