import { createContext } from "react";

export type CurrentIds = {
  assetId?: string;
  organizationId?: string;
  teamId?: string;
};

export const CurrentContext = createContext<CurrentIds>({});
