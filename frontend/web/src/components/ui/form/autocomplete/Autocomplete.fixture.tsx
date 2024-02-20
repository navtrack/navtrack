import { SelectOption } from "../select/Select";
import { useValue } from "react-cosmos/client";
import { Autocomplete } from "./Autocomplete";
import { useEffect } from "react";

const assets: SelectOption[] = Array.from(Array(1000).keys()).map((x) => ({
  value: `${x}`,
  label: `CJ${x}NAV`
}));

export default {
  Basic: () => {
    const [value, setValue] = useValue<string>("option", {
      defaultValue: assets[10].value
    });

    return <Autocomplete options={assets} value={value} onChange={setValue} />;
  },
  WithInterval: () => {
    const [value, setValue] = useValue<string>("option", {
      defaultValue: ""
    });

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
