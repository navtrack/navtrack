import { useEffect, useRef } from "react";
import { useRecoilState } from "recoil";
import { AssetModel } from "../../../../api/model/generated";
import { scrollToAssetAtom } from "../../../../state/assets";

export default function useScrollToAsset(asset?: AssetModel) {
  const [scrollToAsset, setScrollToAsset] = useRecoilState(scrollToAssetAtom);
  const elementRef = useRef<HTMLAnchorElement>(null);

  useEffect(() => {
    if (scrollToAsset && asset && asset.id === scrollToAsset) {
      if (elementRef?.current) {
        elementRef.current.scrollIntoView({
          behavior: "smooth",
          block: "center"
        });
      }
      setScrollToAsset(undefined);
    }
  }, [asset, scrollToAsset, setScrollToAsset]);

  return { setScrollToAsset, elementRef };
}
