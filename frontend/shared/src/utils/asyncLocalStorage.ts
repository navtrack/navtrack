export const AsyncLocalStorage: {
  getItem: (key: string) => Promise<string | null>;
  setItem: (key: string, value: string) => Promise<void>;
  removeItem: (key: string) => Promise<void>;
  subscribe: (
    key: string,
    callback: (value: string | null) => void
  ) => () => void;
} = {
  subscribe: (key, callback) => {
    const handler = (event: StorageEvent) => {
      if (
        event.storageArea === localStorage &&
        (event.key === key || event.key === null)
      ) {
        callback(event.newValue);
      }
    };
    window.addEventListener("storage", handler);
    return () => window.removeEventListener("storage", handler);
  },

  getItem: async (key) => {
    return Promise.resolve(localStorage.getItem(key));
  },

  setItem: async (key, value) => {
    localStorage.setItem(key, value);
    return Promise.resolve();
  },

  removeItem: async (key) => {
    localStorage.removeItem(key);
    return Promise.resolve();
  }
};
