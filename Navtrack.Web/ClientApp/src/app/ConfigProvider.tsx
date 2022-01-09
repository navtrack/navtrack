import useLocalStorage from "../hooks/app/useLocalStorageData";
import { useFetchConfig } from "../hooks/config/useFetchConfig";

const ConfigProvider: React.FC = (props) => {
  const { initialized: configInitialized } = useFetchConfig();
  const { initialized: localStorageInitialized } = useLocalStorage();

  return <>{configInitialized && localStorageInitialized && props.children}</>;
};

export default ConfigProvider;
