import { useCallback } from "react";
import { useSetRecoilState } from "recoil";
import { authenticationAtom } from "@navtrack/navtrack-shared";

export const useLogout = () => {
  const setState = useSetRecoilState(authenticationAtom);

  const logout = useCallback(() => {
    setState((current) => ({
      ...current,
      isAuthenticated: false,
      token: undefined
    }));
  }, [setState]);

  return logout;
};
