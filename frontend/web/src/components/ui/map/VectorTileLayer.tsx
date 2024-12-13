import { ReactNode, useEffect, useRef } from "react";
import { useMap } from "./useMap";
import L from "leaflet";
import "@maplibre/maplibre-gl-leaflet";

type VectorTileLayerProps = {
  styleUrl: string;
  children?: ReactNode;
};

export function VectorTileLayer(props: VectorTileLayerProps) {
  const map = useMap();
  const layer = useRef<L.MaplibreGL>();

  useEffect(() => {
    if (layer.current === undefined) {
      const l = L.maplibreGL({ style: props.styleUrl }).addTo(map.leafletMap);

      layer.current = l;
    }
  }, [layer, map, props.styleUrl]);

  return null;
}
