import React from "react";
import { AssetModel } from "../Api/Model/AssetModel";

type AppContextWrapper = {
    appContext: AppContext,
    setAppContext: (newAppContext: AppContext) => unknown;
}

export type AppContext = {
    user?: User,
    assets?: AssetModel[],
    authenticated: boolean
}

const DefaultAppContext : AppContext = {
    authenticated: false
}

export type User = {
    username: string
}

export const DefaultUser : User = {
    username: ""
}

const appContextKey = "navtrack.appContext";

export function GetAppContext(): AppContext {
    const localStorageAppContext: LocalStorageAppContext = GetFromLocalStorage();

    return {
        authenticated: localStorageAppContext.authenticated
    };
}

export const AppContext = React.createContext<AppContextWrapper>({
    appContext: DefaultAppContext,
    setAppContext: () => { }
});

export default AppContext;

type LocalStorageAppContext = {
    authenticated: boolean
}

const DefaultLocalStorageAppContext : LocalStorageAppContext = {
    authenticated: false
}

export function SaveToLocalStorage(appContext: LocalStorageAppContext) {
    localStorage.setItem(appContextKey, JSON.stringify(appContext));
}

export function GetFromLocalStorage() : LocalStorageAppContext  {
    const localStorageAppContextJson = localStorage.getItem(appContextKey);

    if (localStorageAppContextJson) {
        const appContext = JSON.parse(localStorageAppContextJson);

        return appContext;
    }

    return DefaultLocalStorageAppContext;
}