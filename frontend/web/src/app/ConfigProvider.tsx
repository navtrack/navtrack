import { useAxiosBaseUrls } from "@navtrack/ui-shared/hooks/axios/useAxiosBaseUrls";
import { useSetConfig } from "@navtrack/ui-shared/hooks/config/useSetConfig";
import { ReactNode } from "react";
import useLocalStorage from "../hooks/app/useLocalStorage";

const ConfigProvider = (props: { children: ReactNode }) => {
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
