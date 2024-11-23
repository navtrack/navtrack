import { atom } from "recoil";
import { getLocalStorageEffect } from "./getLocalStorageEffect";
import { IS_WEB } from "../utils/web";

export const currentAssetIdAtom = atom<string | undefined>({
  key: "Navtrack:Current:AssetId",
  default: undefined,
  effects: IS_WEB ? undefined : [getLocalStorageEffect<string | undefined>()]
});

export const currentOrganizationIdAtom = atom<string | undefined>({
  key: "Navtrack:Current:OrganizationId",
  default: undefined,
  effects: IS_WEB ? undefined : [getLocalStorageEffect<string | undefined>()]
});
