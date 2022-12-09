import { useCallback } from "react";
import { useSetRecoilState } from "recoil";
import { authenticationAtom } from "../../state/authentication";

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
