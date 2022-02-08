import { useAxiosBaseUrls, useSetConfig } from "@navtrack/navtrack-app-shared";
import useLocalStorage from "../hooks/app/useLocalStorage";

const ConfigProvider: React.FC = (props) => {
  const configInitialized = useSetConfig({
    apiUrl: `${process.env.REACT_APP_API_URL}`
  });
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
