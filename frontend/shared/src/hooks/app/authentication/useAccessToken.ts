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
import { log } from "../../../utils/log";

type RefreshLock = {
  date: string;
};

const refreshLockKey = "Navtrack:Authentication:RefreshLock";
let localRefreshLock: RefreshLock | undefined = undefined;

async function clearRefreshLock(force?: boolean) {
  const refreshLock = await getFromAsyncStorage<RefreshLock>(refreshLockKey);

  if (
    force ||
    (refreshLock !== undefined &&
      isAfter(new Date(), add(parseISO(refreshLock.date), { seconds: 2 })))
  ) {
    await removeFromAsyncStorage(refreshLockKey);
    localRefreshLock = undefined;
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
      !(await getFromAsyncStorage<RefreshLock>(refreshLockKey)) &&
      !localRefreshLock
    ) {
      try {
        log("TOKEN", "lock");
        localRefreshLock = {
          date: new Date().toISOString()
        };
        await setInAsyncStorage<RefreshLock>(refreshLockKey, localRefreshLock);

        const data = {
          grant_type: "refresh_token",
          client_id: appConfig?.authentication?.clientId!,
          refresh_token: localStorageAuthentication.token.refreshToken
        };

        const response = await tokenMutation.mutateAsync(data);
        log("TOKEN", "refresh");

        return response.access_token;
      } finally {
        await clearRefreshLock(true);
        log("TOKEN", "unlock");
      }
    }

    return localStorageAuthentication?.token?.accessToken;
  }, [appConfig?.authentication?.clientId, tokenMutation]);

  return { getAccessToken, isLoading: tokenMutation.isLoading };
}
