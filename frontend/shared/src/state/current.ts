import { atom } from "recoil";
import { getLocalStorageEffect } from "./getLocalStorageEffect";

export const currentAssetIdAtom = atom<string | undefined>({
  key: "Navtrack:Current:AssetId",
  default: undefined,
  effects:
    typeof window === "undefined"
      ? [getLocalStorageEffect<string | undefined>()]
      : undefined
});

export const currentOrganizationIdAtom = atom<string | undefined>({
  key: "Navtrack:Current:OrganizationId",
  default: undefined,
  effects:
    typeof window === "undefined"
      ? [getLocalStorageEffect<string | undefined>()]
      : undefined
});
