import {
  isDevEnv,
  useAxiosBaseUrls,
  useFetchConfig
} from "@navtrack/navtrack-app-shared";
import { LocalConfig } from "../app.config";
import useLocalStorage from "../hooks/app/useLocalStorage";

const ConfigProvider: React.FC = (props) => {
  const configInitialized = useFetchConfig(isDevEnv ? LocalConfig : undefined);
  const localStorageInitialized = useLocalStorage();
  const interceptorInitialised = useAxiosBaseUrls();

  return (
    <>
      {configInitialized &&
        localStorageInitialized &&
        interceptorInitialised &&
        props.children}
    </>
  );
};

export default ConfigProvider;
