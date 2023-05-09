import { atom } from "recoil";
import AsyncStorage from "@react-native-async-storage/async-storage";

type Token = {
  accessToken: string;
  refreshToken: string;
  expiryDate: string;
};

type AuthenticationState = {
  isAuthenticated: boolean;
  initialized: boolean;
  token?: Token;
  email?: string;
};

export const authenticationAtom = atom<AuthenticationState>({
  key: "Navtrack:Authentication",
  default: { isAuthenticated: false, initialized: false },
  effects: [
    ({ setSelf, onSet, trigger, node }) => {
      const loadPersisted = async () => {
        const json = await AsyncStorage.getItem(node.key);

        if (json) {
          try {
            const data = JSON.parse(json) as AuthenticationState;

            setSelf({ ...data, initialized: true });
          } catch (e) {
            setSelf({ isAuthenticated: false, initialized: true });
          }
        } else {
          setSelf({ isAuthenticated: false, initialized: true });
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
    }
  ]
});

export const checkTokenIntervalAtom = atom<NodeJS.Timeout | undefined>({
  key: "Navtrack:Authentication:CheckTokenInterval",
  default: undefined
});
