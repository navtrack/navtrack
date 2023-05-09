import { SelectInput, ISelectInputItem } from "./SelectInput";

const assets: ISelectInputItem[] = Array.from(Array(100).keys()).map((x) => ({
  id: `${x}`,
  label: `CJ${x}NAV`
}));

export default {
  Basic: () => {
    return <SelectInput items={assets} />;
  }
};
