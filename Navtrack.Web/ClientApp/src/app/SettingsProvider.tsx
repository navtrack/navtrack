import { useSettings } from "../hooks/settings/useSettings";

const SettingsProvider: React.FC = (props) => {
  const settingsInitialized = useSettings();

  return <>{settingsInitialized && props.children}</>;
};

export default SettingsProvider;
