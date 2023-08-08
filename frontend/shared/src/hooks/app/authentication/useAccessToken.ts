import { useRecoilValue } from "recoil";
import { useTokenMutation } from "./useTokenMutation";
import { useCallback } from "react";
import { add, isAfter, parseISO, sub } from "date-fns";
import { appConfigAtom } from "../../../state/appConfig";
import { randomInteger } from "../../../utils/numbers";
import {
  getFromAsyncStorage,
  removeFromAsyncStorage,
  setInAsyncStorage
} from "../../../utils/asyncStorage";
import { Authentication } from "./authentication";

type RefreshLock = {
  date: string;
};

const refreshLockKey = "Navtrack:Authentication:RefreshLock";

async function clearRefreshLock(force?: boolean) {
  const refreshLock = await getFromAsyncStorage<RefreshLock>(refreshLockKey);

  if (
    force ||
    (refreshLock !== undefined &&
      isAfter(new Date(), add(parseISO(refreshLock.date), { seconds: 2 })))
  ) {
    await removeFromAsyncStorage(refreshLockKey);
  }
}

function tokenIsExpired(expiryDate: string): boolean {
  const expiryDateAdjusted = sub(parseISO(expiryDate), {
    seconds: randomInteger(120, 300)
  });

  const expired = isAfter(new Date(), expiryDateAdjusted);

  return expired;
}

export function useAccessToken() {
  const tokenMutation = useTokenMutation();
  const appConfig = useRecoilValue(appConfigAtom);

  const getAccessToken = useCallback(async () => {
    await clearRefreshLock();

    const localStorageAuthentication = await Authentication.get();

    if (
      localStorageAuthentication?.token !== undefined &&
      tokenIsExpired(localStorageAuthentication.token.expiryDate) &&
      !(await getFromAsyncStorage<RefreshLock>(refreshLockKey))
    ) {
      try {
        await setInAsyncStorage<RefreshLock>(refreshLockKey, {
          date: new Date().toISOString()
        });

        const data = {
          grant_type: "refresh_token",
          client_id: appConfig?.authentication?.clientId!,
          refresh_token: localStorageAuthentication.token.refreshToken
        };

        const response = await tokenMutation.mutateAsync(data);
        console.log("token refreshed");

        return response.access_token;
      } finally {
        await clearRefreshLock(true);
      }
    }

    return localStorageAuthentication?.token?.accessToken;
  }, [appConfig?.authentication?.clientId, tokenMutation]);

  return { getAccessToken };
}
