import { isAfter, sub } from "date-fns";
import { useCallback, useEffect, useState } from "react";
import { AUTHENTICATION } from "../../constants";
import useAppContext from "../app/useAppContext";
import { useGetTokenMutation } from "../mutations/useGetTokenMutation";
import { useLogout } from "./useLogout";

export const useTokenRefresh = () => {
  const { appContext, setAppContext } = useAppContext();
  const [checkTokenInterval, setCheckTokenInterval] = useState<
    NodeJS.Timeout | undefined
  >();
  const [currentRefreshToken, setCurrentRefreshToken] = useState<
    string | undefined
  >();
  const logout = useLogout();

  const refreshTokenMutation = useGetTokenMutation({
    onSuccess: (data) => {
      setAppContext((current) => ({
        ...current,
        isAuthenticated: true,
        token: {
          accessToken: data.access_token,
          refreshToken: data.refresh_token,
          expiryDate: new Date(new Date().getTime() + data.expires_in * 1000)
        }
      }));
    },
    onError: () => {
      logout();
    }
  });

  const checkToken = useCallback(() => {
    if (appContext.token) {
      const expiryDate = sub(appContext.token.expiryDate, { minutes: 2 });

      if (isAfter(new Date(), expiryDate)) {
        const data = {
          grant_type: "refresh_token",
          client_id: AUTHENTICATION.CLIENT_ID,
          refresh_token: appContext.token?.refreshToken
        };

        refreshTokenMutation.mutate(data);
      }
    }
  }, [appContext.token, refreshTokenMutation]);

  const clearCheck = useCallback(() => {
    if (checkTokenInterval) {
      clearInterval(checkTokenInterval);
      setCurrentRefreshToken(undefined);
    }
  }, [checkTokenInterval]);

  const setCheck = useCallback(() => {
    const interval = setInterval(() => {
      checkToken();
    }, 1000);
    setCheckTokenInterval(interval);
    setCurrentRefreshToken(appContext.token?.refreshToken);
  }, [appContext.token?.refreshToken, checkToken]);

  // If the user is authenticated, set the interval to check the token
  useEffect(() => {
    if (appContext.isAuthenticated && !checkTokenInterval) {
      setCheck();
    }
  }, [appContext.isAuthenticated, checkTokenInterval, setCheck]);

  // If the user logs out, clear the interval
  useEffect(() => {
    if (!appContext.isAuthenticated) {
      clearCheck();
    }
  }, [appContext.isAuthenticated, clearCheck]);

  // If the refresh token changes, clear the interval and set the interval again
  useEffect(() => {
    if (
      appContext.isAuthenticated &&
      checkTokenInterval !== undefined &&
      currentRefreshToken &&
      currentRefreshToken !== appContext.token?.refreshToken
    ) {
      clearCheck();
      setCheck();
    }
  }, [
    appContext.isAuthenticated,
    appContext.token?.refreshToken,
    checkTokenInterval,
    clearCheck,
    currentRefreshToken,
    setCheck
  ]);
};
