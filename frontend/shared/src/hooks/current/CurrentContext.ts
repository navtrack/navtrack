import { createContext } from "react";

export type CurrentContextProps = {
  assetId?: string;
  organizationId?: string;
  teamId?: string;
};

export const CurrentContext = createContext<CurrentContextProps>({});
