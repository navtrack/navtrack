import { atom } from "recoil";

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
  sentry?: {
    dsn: string;
  };
  map: {
    tileUrl: string;
  };
};

export const appConfigAtom = atom<AppConfig | undefined>({
  key: "Navtrack:AppConfig",
  default: undefined
});
