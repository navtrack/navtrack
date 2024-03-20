import { useFixtureInput } from "react-cosmos/client";
import { Switch } from "./Switch";

export default {
  Basic: () => {
    const [checked, setChecked] = useFixtureInput("checked", false);

    return <Switch checked={checked} onChange={setChecked} />;
  }
};
