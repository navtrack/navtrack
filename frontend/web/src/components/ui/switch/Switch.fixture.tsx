import { useInput } from "react-cosmos/client";
import { Switch } from "./Switch";

export default {
  Basic: () => {
    const [checked, setChecked] = useInput("checked", false);

    return <Switch checked={checked} onChange={setChecked} />;
  }
};
