import { SelectOption } from "./Select";
import { CustomSelect } from "./CustomSelect";

const assets: SelectOption[] = Array.from(Array(100).keys()).map((x) => ({
  value: `${x}`,
  label: `CJ${x}NAV`
}));

export default {
  Basic: () => {
    return <CustomSelect options={assets} />;
  }
};
