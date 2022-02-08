import { useSettings } from "@navtrack/navtrack-app-shared";

const SettingsProvider: React.FC = (props) => {
  const settingsInitialized = useSettings();

  return <>{settingsInitialized && props.children}</>;
};

export default SettingsProvider;
