import { AppContext } from "./";

let _appContext: AppContext | undefined;
let _setAppContext: ((newAppContext: AppContext) => unknown) | undefined;

export const AppContextAccessor = {
    getAppContext: (): AppContext => {
        if (_appContext) {
            return _appContext;
        }

        throw new Error("appContext not set");
    },

    setNewAppContext: (appContext: AppContext) => {
        _appContext = appContext;
    },

    setAppContext: (newAppContext: AppContext) => {
        if (_setAppContext) {
            _setAppContext(newAppContext);
        }
        else {
            throw new Error("setAppContext not set");
        }
    },

    setNewSetAppContext: (setAppContext: (newAppContext: AppContext) => unknown) => {
        _setAppContext = setAppContext;
    }
}