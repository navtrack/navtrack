import { useAxiosBaseUrls } from "@navtrack/navtrack-shared";
import useLocalStorage from "../hooks/app/useLocalStorageData";
import { useFetchConfig } from "../hooks/config/useFetchConfig";

const ConfigProvider: React.FC = (props) => {
  const configInitialized = useFetchConfig();
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
