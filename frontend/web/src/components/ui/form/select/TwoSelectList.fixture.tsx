import { useFixtureInput } from "react-cosmos/client";
import { SelectOption } from "./Select";
import { TwoSelectList } from "./TwoSelectList";

const assets: SelectOption[] = Array.from(Array(100).keys()).map((x) => ({
  value: `${x}`,
  label: `CJ${x}NAV`
}));

const fixture = {
  Basic: () => {
    const [value, setValue] = useFixtureInput<string[]>("value", []);

    return (
      <TwoSelectList
        options={assets}
        values={value}
        onChange={(value) => setValue(value)}
        size={10}
      />
    );
  },
  Selected: () => {
    const [value, setValue] = useFixtureInput<string[]>("value", [
      assets[2].value,
      assets[5].value,
      assets[8].value,
      assets[10].value
    ]);

    return (
      <TwoSelectList
        options={assets}
        values={value}
        onChange={(value) => setValue(value)}
        size={10}
      />
    );
  }
};

export default fixture;
