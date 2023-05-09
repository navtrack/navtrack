import { atom, selector } from "recoil";

export type EnvironmentSetting =
  | "AppleAuthentication.ClientId"
  | "AppleAuthentication.RedirectUri"
  | "GoogleAuthentication.ClientId"
  | "MicrosoftAuthentication.ClientId"
  | "MicrosoftAuthentication.Authority"
  | "Sentry.Dsn"
  | "Application.Environment"
  | "Map.TileUrl";

type Environment = {
  settings: Record<string, string>;
  initialized: boolean;
};

export const environmentAtom = atom<Environment>({
  key: "Navtrack:Environment",
  default: {
    settings: {},
    initialized: false
  }
});

export const environmentSettingsSelector = selector({
  key: "Navtrack:Environment:Selector",
  get: ({ get }) => {
    const state = get(environmentAtom);

    return state.settings as Record<EnvironmentSetting, string>;
  }
});
