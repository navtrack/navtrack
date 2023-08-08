import {
  getFromAsyncStorage,
  removeFromAsyncStorage,
  setInAsyncStorage
} from "../../../utils/asyncStorage";

export type Token = {
  accessToken: string;
  refreshToken: string;
  expiryDate: string;
  date: string;
};

export type AuthenticationState = {
  token?: Token;
};

export enum AuthenticationErrorType {
  Internal,
  External,
  Other
}

export const authenticationAtomKey = "Navtrack:Authentication";

export const Authentication = {
  get: () => getFromAsyncStorage<AuthenticationState>(authenticationAtomKey),
  set: (value: AuthenticationState) =>
    setInAsyncStorage(authenticationAtomKey, value),
  clear: () => removeFromAsyncStorage(authenticationAtomKey)
};
