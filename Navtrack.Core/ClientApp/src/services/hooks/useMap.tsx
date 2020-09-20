import { useEffect } from "react";
import { MapService } from "../MapService";

const useMap = (showMap: boolean = true): void => {
  useEffect(() => {
    MapService.setMapVisibility(showMap);
  }, [showMap]);
};

export default useMap;
