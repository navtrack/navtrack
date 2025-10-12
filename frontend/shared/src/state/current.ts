import { atom } from "jotai";
import { atomWithStorage } from "jotai/utils";
import { IS_WEB } from "../utils/web";

export const currentAssetIdAtom = IS_WEB
  ? atom<string | undefined>(undefined)
  : atomWithStorage<string | undefined>("Navtrack:Current:AssetId", undefined);

export const currentAssetIdInitializedAtom = atom<boolean>(false);

export const currentOrganizationIdAtom = IS_WEB
  ? atom<string | undefined | null>(null)
  : atomWithStorage<string | undefined | null>(
      "Navtrack:Current:OrganizationId",
      null
    );

export const currentOrganizationIdInitializedAtom = atom<boolean>(false);
