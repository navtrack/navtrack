import { useEffect } from "react";
import { useFixtureInput } from "react-cosmos/client";
import { SelectOption } from "../select/Select";
import { Autocomplete } from "./Autocomplete";

const assets: SelectOption[] = Array.from(Array(1000).keys()).map((x) => ({
  value: `${x}`,
  label: `CJ${x}NAV`
}));

export default {
  Basic: () => {
    const [value, setValue] = useFixtureInput("option", assets[10].value);

    return <Autocomplete options={assets} value={value} onChange={setValue} />;
  },
  WithInterval: () => {
    const [value, setValue] = useFixtureInput("option", "");

    useEffect(() => {
      if (!value) {
        setInterval(() => {
          setValue(assets[10].value);
        }, 2000);
      }
    }, [setValue, value]);

    return <Autocomplete options={assets} value={value} onChange={setValue} />;
  }
};
