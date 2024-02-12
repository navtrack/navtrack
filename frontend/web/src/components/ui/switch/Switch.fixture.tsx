import { useValue } from "react-cosmos/client";
import { Switch } from "./Switch";

export default {
  Basic: () => {
    const [checked, setChecked] = useValue<boolean>("checked", { defaultValue: false });

    return (<Switch checked={checked} onChange={setChecked} />);
  }
};
