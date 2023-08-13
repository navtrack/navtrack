import { atom } from "recoil";
import {
  Authentication,
  AuthenticationErrorType
} from "../hooks/app/authentication/authentication";

export const authenticationErrorAtom = atom<
  AuthenticationErrorType | undefined
>({
  key: "Navtrack:Authentication:Error",
  default: undefined
});

async function isAuthenticatedDefault() {
  const state = await Authentication.get();

  return state?.token !== undefined;
}

export const isAuthenticatedAtom = atom<boolean>({
  key: "Navtrack:Authentication:IsAuthenticated",
  default: isAuthenticatedDefault()
});

export const isLoggingInAtom = atom<boolean>({
  key: "Navtrack:Authentication:IsLoggingIn",
  default: false
});
