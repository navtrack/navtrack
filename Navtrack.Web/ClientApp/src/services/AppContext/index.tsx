import React from "react";
import { UserModel } from "services/Api/Model/UserModel";
import { AssetModel } from "services/Api/Model/AssetModel";
import { AuthenticationInfo, DefaultAuthenticationInfo } from "services/Authentication/AuthenticationInfo";

export type AppContext = {
  user?: UserModel;
  assets?: AssetModel[];
  authenticationInfo: AuthenticationInfo;
};

const DefaultAppContext: AppContext = {
  authenticationInfo: DefaultAuthenticationInfo
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

export const AppContext = React.createContext<AppContextWrapper>({
  appContext: DefaultAppContext,
  setAppContext: () => {}
});

export default AppContext;

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
