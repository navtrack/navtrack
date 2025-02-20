import { atom } from "recoil";
import { getLocalStorageEffect } from "./getLocalStorageEffect";
import { IS_WEB } from "../utils/web";

export const currentAssetIdAtom = atom<string | undefined>({
  key: "Navtrack:Current:AssetId",
  default: undefined,
  effects: IS_WEB ? undefined : [getLocalStorageEffect<string | undefined>()]
});

export const currentAssetIdInitializedAtom = atom<boolean>({
  key: "Navtrack:Current:AssetIdInitialized",
  default: false
});

export const currentOrganizationIdAtom = atom<string | undefined | null>({
  key: "Navtrack:Current:OrganizationId",
  default: null,
  effects: IS_WEB
    ? undefined
    : [getLocalStorageEffect<string | undefined | null>()]
});

export const currentOrganizationIdInitializedAtom = atom<boolean>({
  key: "Navtrack:Current:OrganizationIdInitialized",
  default: false
});
