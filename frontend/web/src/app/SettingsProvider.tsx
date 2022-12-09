import { useSettings } from "@navtrack/ui-shared/hooks/settings/useSettings";
import { ReactNode } from "react";

export const SettingsProvider = (props: { children: ReactNode }) => {
  const settingsInitialized = useSettings();

  return <>{settingsInitialized && props.children}</>;
};
