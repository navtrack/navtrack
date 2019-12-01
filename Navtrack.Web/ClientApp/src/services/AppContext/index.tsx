import React from "react";

export type AppContext = {
    isAuthenticated: boolean,
    username: string
}

export const AppContext = React.createContext<AppContext>({
    isAuthenticated: false,
    username: ''
});