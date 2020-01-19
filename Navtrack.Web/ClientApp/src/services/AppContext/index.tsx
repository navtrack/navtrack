import React from "react";
import { AssetModel } from "../Api/Model/AssetModel";

type AppContextWrapper = {
    appContext: AppContext,
    setAppContext: (newAppContext: AppContext) => unknown;
}

export type AppContext = {
    user: User,
    assets?: AssetModel[]
}

export type User = {
    authenticated: boolean,
    username: string
}

const userKey = "user";

export function GetAppContext(): AppContext {
    const user: User = GetUserFromLocalStorage();

    return {
        user: user
    };
}

export const AppContext = React.createContext<AppContextWrapper>({
    appContext: GetAppContext(),
    setAppContext: () => { }
});

export default AppContext;

export function SetUserInLocalStorage(user: User) {
    localStorage.setItem(userKey, JSON.stringify(user));
}

export function GetUserFromLocalStorage() : User  {
    const localStorageUser = localStorage.getItem(userKey);

    if (localStorageUser) {
        const user = JSON.parse(localStorageUser);

        return user;
    }

    return { 
        authenticated: false,
        username: ''
    };
}