import { useContext, useEffect } from "react";
import { AppContext } from "../../../../services/appContext/AppContext";
import { MapService } from "../../../../services/MapService";

export default function AdminMap() {
  const { appContext } = useContext(AppContext);

  useEffect(() => {
    MapService.initMap();
  }, []);

  return (
    <div
      id="map"
      className="bg-gray-100 min-h-screen fixed w-full"
      style={{
        top: 52,
        filter: appContext.mapIsVisible ? "" : "grayscale(1) blur(3px)",
        opacity: appContext.mapIsVisible ? 1 : 0.3
      }}></div>
  );
}
