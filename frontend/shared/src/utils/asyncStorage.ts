import AsyncStorage from "@react-native-async-storage/async-storage";
import { AsyncStorage as AsyncStorageType } from "jotai/vanilla/utils/atomWithStorage";

export class CustomAsyncStorage<T> implements AsyncStorageType<T> {
  async getItem(key: string, initialValue?: T): Promise<T> {
    const json = await AsyncStorage.getItem(key);

    if (json) {
      try {
        const data = JSON.parse(json) as T;

        return data;
      } catch (e) {
        return undefined as unknown as T;
      }
    }

    return undefined as unknown as T;
  }
  setItem(key: string, newValue: T): Promise<void> {
    return AsyncStorage.setItem(key, JSON.stringify(newValue));
  }
  removeItem(key: string): Promise<void> {
    return AsyncStorage.removeItem(key);
  }
}

export async function getFromAsyncStorage<T>(key: string) {
  const json = await AsyncStorage.getItem(key);

  if (json) {
    try {
      const data = JSON.parse(json) as T;

      return data;
    } catch (e) {
      return undefined;
    }
  }

  return undefined;
}

export async function setInAsyncStorage<T>(key: string, value: T) {
  return AsyncStorage.setItem(key, JSON.stringify(value));
}

export async function removeFromAsyncStorage(key: string) {
  return AsyncStorage.removeItem(key);
}
