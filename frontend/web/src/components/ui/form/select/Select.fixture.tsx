import { useState } from "react";
import { Select, SelectOption } from "./Select";

const assets: SelectOption[] = Array.from(Array(100).keys()).map((x) => ({
  value: `${x}`,
  label: `CJ${x}NAV`
}));

const fixture = {
  Basic: () => {
    const [value, setValue] = useState(assets[10].value);

    return (
      <Select
        options={assets}
        value={value}
        onChange={(value) => setValue(value)}
      />
    );
  }
};

export default fixture;
