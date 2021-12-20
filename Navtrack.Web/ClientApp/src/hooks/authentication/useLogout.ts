import { useCallback } from "react";
import useAppContext from "../app/useAppContext";

export const useLogout = () => {
  const { setAppContext } = useAppContext();

  const logout = useCallback(() => {
    setAppContext((current) => ({
      ...current,
      isAuthenticated: false,
      token: undefined
    }));
  }, [setAppContext]);

  return logout;
};
