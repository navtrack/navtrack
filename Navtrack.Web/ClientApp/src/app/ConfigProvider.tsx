import { useFetchConfig } from "../hooks/config/useFetchConfig";

const ConfigProvider: React.FC = (props) => {
  const loaded = useFetchConfig();

  return <>{loaded && props.children}</>;
};

export default ConfigProvider;
