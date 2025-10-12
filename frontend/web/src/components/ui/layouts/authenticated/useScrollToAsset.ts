import { AssetModel } from "@navtrack/shared/api/model";
import { scrollToAssetAtom } from "@navtrack/shared/state/assets";
import { useEffect, useRef } from "react";
import { useAtom } from "jotai";

export function useScrollToAsset(asset?: AssetModel) {
  const [scrollToAsset, setScrollToAsset] = useAtom(scrollToAssetAtom);
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
