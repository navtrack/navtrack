export const AsyncLocalStorage: {
  getItem: (key: string) => Promise<string | null>;
  setItem: (key: string, value: string) => Promise<void>;
  removeItem: (key: string) => Promise<void>;
} = {
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
