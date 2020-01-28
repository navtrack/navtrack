import AppContext from ".";

let _getAppContext: (() => AppContext) | undefined;
let _setAppContext: ((newAppContext: AppContext) => unknown) | undefined;

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

  setAppContext: (newAppContext: AppContext) => {
    if (_setAppContext) {
      _setAppContext(newAppContext);
    }
    else {
      throw new Error("AppContext setter not set");
    }
  },

  setAppContextSetter: (setAppContext: (newAppContext: AppContext) => unknown) => {
    _setAppContext = setAppContext;
  }
};