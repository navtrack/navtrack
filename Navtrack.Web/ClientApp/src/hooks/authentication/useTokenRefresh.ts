import { add, isAfter, sub } from "date-fns";
import { useCallback, useEffect, useState } from "react";
import { AUTHENTICATION } from "../../constants";
import useAppContext from "../app/useAppContext";
import { useGetTokenMutation } from "../mutations/useGetTokenMutation";
import { useLogout } from "./useLogout";

function log(message?: any, ...optionalParams: any[]) {
  // console.log(message, ...optionalParams);
}

export const useTokenRefresh = () => {
  const { appContext, setAppContext } = useAppContext();
  const [checkTokenInterval, setCheckTokenInterval] = useState<
    NodeJS.Timeout | undefined
  >();
  const logout = useLogout();
  const [reinitialize, setReinitialize] = useState(false);

  const refreshTokenMutation = useGetTokenMutation({
    onSuccess: (data) => {
      const expiryDate = add(new Date(), {
        seconds: data.expires_in
      });

      setAppContext((current) => ({
        ...current,
        isAuthenticated: true,
        token: {
          accessToken: data.access_token,
          refreshToken: data.refresh_token,
          expiryDate: expiryDate
        }
      }));
      setReinitialize(true);
    },
    onError: () => {
      logout();
    }
  });

  const checkToken = useCallback(() => {
    if (appContext.token) {
      log("[TOKEN REFRESH] Checking");
      const expiryDate = sub(appContext.token.expiryDate, { minutes: 2 });

      if (isAfter(new Date(), expiryDate)) {
        log(
          "[TOKEN REFRESH] Token expired, refreshing token",
          appContext.token.expiryDate
        );
        const data = {
          grant_type: "refresh_token",
          client_id: AUTHENTICATION.CLIENT_ID,
          refresh_token: appContext.token?.refreshToken
        };

        refreshTokenMutation.mutate(data);
      } else {
        log("[TOKEN REFRESH] Token valid", appContext.token.expiryDate);
      }
    }
  }, [appContext.token, refreshTokenMutation]);

  const setCheckInterval = useCallback(() => {
    const interval = setInterval(() => {
      checkToken();
    }, 10000);

    setCheckTokenInterval(interval);
  }, [checkToken]);

  // If the user is authenticated, set the interval to check the token
  useEffect(() => {
    if (appContext.isAuthenticated && !checkTokenInterval) {
      log("[TOKEN REFRESH] Setting interval");

      setCheckInterval();
    }
  }, [appContext.isAuthenticated, checkTokenInterval, setCheckInterval]);

  // If the user logs out, clear the interval
  useEffect(() => {
    if (!appContext.isAuthenticated && checkTokenInterval) {
      log("[TOKEN REFRESH] Stopping");
      clearInterval(checkTokenInterval);
      setCheckTokenInterval(undefined);
    }
  }, [appContext.isAuthenticated, checkTokenInterval]);

  // If the expiry date changes, clear the interval and set a new one
  useEffect(() => {
    if (reinitialize && checkTokenInterval) {
      log("[TOKEN REFRESH] Reinitializing");
      setReinitialize(false);
      clearInterval(checkTokenInterval);
      setCheckInterval();
    }
  }, [checkTokenInterval, reinitialize, setCheckInterval]);
};
