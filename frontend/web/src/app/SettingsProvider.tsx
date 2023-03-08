import { useSettings } from "@navtrack/shared/hooks/settings/useSettings";
import { ReactNode } from "react";

export const SettingsProvider = (props: { children: ReactNode }) => {
  const settingsInitialized = useSettings();

  return <>{settingsInitialized && props.children}</>;
};
