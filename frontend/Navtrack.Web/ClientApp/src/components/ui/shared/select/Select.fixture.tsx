import { useState } from "react";
import Select from "./Select";
import { ISelectOption } from "./types";

const assets: ISelectOption[] = Array.from(Array(100).keys()).map((x) => ({
  value: `${x}`,
  label: `CJ${x}NAV`
}));

export default {
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
