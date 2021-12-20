import SelectInput from "./SelectInput";

type Asset = {
  id: string;
  name: string;
};

const assets: Asset[] = Array.from(Array(100).keys()).map((x) => ({
  id: `${x}`,
  name: `CJ${x}NAV`
}));

export default {
  Basic: () => {
    return <SelectInput items={assets} idKey="id" labelKey="name" />;
  }
};
