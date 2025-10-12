import { atom } from "jotai";

export type AppConfig = {
  api: {
    url: string;
  };
  reactQueryDevtools?: boolean;
  authentication: {
    clientId: string;
    apple?: {
      clientId: string;
      redirectUri: string;
    };
    google?: {
      clientId: string;
      redirectUri?: string;
    };
    microsoft?: {
      clientId: string;
      authority: string;
      redirectUri?: string;
    };
  };
  map: {
    tileUrl: string;
  };
  captcha?: {
    siteKey: string;
  };
};

export const appConfigAtom = atom<AppConfig | undefined>(undefined);
