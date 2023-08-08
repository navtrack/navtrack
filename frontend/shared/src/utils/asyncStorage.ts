import AsyncStorage from "@react-native-async-storage/async-storage";

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
