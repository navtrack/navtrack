import React from "react";
import { AssetModel } from "../../apis/types/asset/AssetModel";
import { UserModel } from "../../apis/types/user/UserModel";
import {
  AuthenticationInfo,
  InitialAuthenticationInfo
} from "../authentication/AuthenticationInfo";

export type AppContext = {
  user?: UserModel;
  assets?: AssetModel[];
  authenticationInfo: AuthenticationInfo;
  mapIsVisible: boolean;
};

const DefaultAppContext: AppContext = {
  authenticationInfo: InitialAuthenticationInfo,
  mapIsVisible: false
};

type AppContextWrapper = {
  appContext: AppContext;
  setAppContext: (newAppContext: AppContext) => unknown;
};

export function CreateAppContext(): AppContext {
  const appContext = DefaultAppContext;

  const localStorageAppContext: LocalStorageAppContext | null = GetFromLocalStorage();

  if (localStorageAppContext) {
    MapFromLocalStorage(localStorageAppContext, appContext);
  }

  return appContext;
}

// eslint-disable-next-line @typescript-eslint/no-redeclare
export const AppContext = React.createContext<AppContextWrapper>({
  appContext: DefaultAppContext,
  setAppContext: () => {}
});

const appContextKey = "navtrack.appContext";

export type LocalStorageAppContext = {
  authenticationInfo: AuthenticationInfo;
};

export function SaveToLocalStorage(appContext: AppContext) {
  localStorage.setItem(appContextKey, JSON.stringify(MapToLocalStorage(appContext)));
}

function GetFromLocalStorage(): LocalStorageAppContext | null {
  const localStorageAppContextJson = localStorage.getItem(appContextKey);

  if (localStorageAppContextJson) {
    const localStorageAppContext = JSON.parse(localStorageAppContextJson);

    return localStorageAppContext;
  }

  return null;
}

function MapToLocalStorage(source: AppContext): LocalStorageAppContext {
  return {
    authenticationInfo: source.authenticationInfo
  };
}

function MapFromLocalStorage(source: LocalStorageAppContext, destination: AppContext) {
  destination.authenticationInfo = source.authenticationInfo;
}
