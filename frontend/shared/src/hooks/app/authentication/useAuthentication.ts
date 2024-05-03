import { useRecoilState, useSetRecoilState } from "recoil";
import {
  authenticationErrorAtom,
  isAuthenticatedAtom
} from "../../../state/authentication";
import {
  Authentication,
  AuthenticationErrorType,
  AuthenticationState
} from "./authentication";
import { useQueryClient } from "@tanstack/react-query";
import { useEffect } from "react";

export function useAuthentication() {
  const queryClient = useQueryClient();
  const setIsAuthenticated = useSetRecoilState(isAuthenticatedAtom);
  const [error, setAuthenticationError] = useRecoilState(
    authenticationErrorAtom
  );

  useEffect(() => {
    return () => {
      setAuthenticationError(undefined);
    };
  }, [setAuthenticationError]);

  return {
    get: () => Authentication.get(),
    set: async (value: AuthenticationState) => {
      await Authentication.set(value);
      setIsAuthenticated(true);
    },
    clear: async (error?: AuthenticationErrorType) => {
      await Authentication.clear();
      queryClient.clear();
      setAuthenticationError(error);
      setIsAuthenticated(false);
    },
    clearErrors: () => {
      setAuthenticationError(undefined);
    },
    error
  };
}
