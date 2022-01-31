import { useEffect } from "react";
import { useRecoilState } from "recoil";
import { assetsAtom } from "@navtrack/navtrack-shared";
import useGetAssetsSignalRQuery from "../queries/useGetAssetsSignalRQuery";

export default function useAssets() {
  const query = useGetAssetsSignalRQuery();
  const [assets, setAssets] = useRecoilState(assetsAtom);

  useEffect(() => {
    if (query.data?.items) {
      setAssets(query.data.items);
    }
  }, [query.data?.items, setAssets]);

  return { data: assets, isLoading: query.isLoading };
}
