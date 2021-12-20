import useCurrentAsset from "./useCurrentAsset";

export default function useCurrentAssetId() {
  const { currentAsset } = useCurrentAsset();

  return currentAsset?.shortId;
}
