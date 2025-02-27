import { useEffect } from "react";
import { useMap } from "./useMap";
// import "leaflet-active-area";

type MapActiveAreaProps = {};

export function MapActiveArea(props: MapActiveAreaProps) {
  const map = useMap();

  useEffect(() => {
    console.log(map.leafletMap.getSize());
    const size = map.leafletMap.getSize();

    const top: number = 230;
    const left: number = 0;
    const right: number = 0;
    const bottom: number = 40;
    const height: number = size.y - top - bottom;

    //@ts-ignore
    // map.leafletMap.setActiveArea({
    //   position: "absolute",
    //   top: `${top}px`,
    //   left: `${left}px`,
    //   right: `${right}px`,
    //   height: `${height}px`
    // });
  }, [map]);

  return null;
}
