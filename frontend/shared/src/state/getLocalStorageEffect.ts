import { AtomEffect, DefaultValue } from "recoil";
import {
  getFromAsyncStorage,
  removeFromAsyncStorage,
  setInAsyncStorage
} from "../utils/asyncStorage";

export function getLocalStorageEffect<T>(): AtomEffect<T> {
  return ({ setSelf, onSet, trigger, node }) => {
    const loadPersisted = async () => {
      const value = await getFromAsyncStorage<T>(node.key);

      setSelf(value !== undefined ? value : new DefaultValue());
    };

    if (trigger === "get") {
      loadPersisted();
    }

    onSet((newValue, _, isReset) => {
      isReset
        ? removeFromAsyncStorage(node.key)
        : setInAsyncStorage(node.key, newValue);
    });
  };
}
