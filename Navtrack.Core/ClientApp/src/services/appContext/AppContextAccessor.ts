import { AppContext } from "./AppContext";

let _getAppContext: (() => AppContext) | undefined;
let _setAppContext: ((value: React.SetStateAction<AppContext>) => unknown) | undefined;

export const AppContextAccessor = {
  getAppContext: (): AppContext => {
    if (_getAppContext) {
      return _getAppContext();
    }

    throw new Error("AppContext getter not set");
  },

  setAppContextGetter: (getAppContext: () => AppContext) => {
    _getAppContext = getAppContext;
  },

  setAppContext: (value: React.SetStateAction<AppContext>) => {
    if (_setAppContext) {
      _setAppContext(value);
    } else {
      throw new Error("AppContext setter not set");
    }
  },

  setAppContextSetter: (setAppContext: React.Dispatch<React.SetStateAction<AppContext>>) => {
    _setAppContext = setAppContext;
  }
};
