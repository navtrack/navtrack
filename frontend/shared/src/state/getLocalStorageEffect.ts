import AsyncStorage from "@react-native-async-storage/async-storage";
import { AtomEffect, DefaultValue } from "recoil";

export function getLocalStorageEffect<T>(): AtomEffect<T> {
  return ({ setSelf, onSet, trigger, node }) => {
    const loadPersisted = async () => {
      const json = await AsyncStorage.getItem(node.key);

      if (json) {
        try {
          const data = JSON.parse(json) as T;

          setSelf(data);
        } catch (e) {
          setSelf(new DefaultValue());
        }
      } else {
        setSelf(new DefaultValue());
      }
    };

    if (trigger === "get") {
      loadPersisted();
    }

    onSet((newValue, _, isReset) => {
      isReset
        ? AsyncStorage.removeItem(node.key)
        : AsyncStorage.setItem(node.key, JSON.stringify(newValue));
    });
  };
}
