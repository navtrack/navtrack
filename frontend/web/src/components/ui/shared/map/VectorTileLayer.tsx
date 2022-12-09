import { useEffect, useState } from "react";
import useMap from "./useMap";
import L from "leaflet";
import "@maplibre/maplibre-gl-leaflet";

type VectorTileLayerProps = {
  styleUrl: string;
};

export const VectorTileLayer = (props: VectorTileLayerProps) => {
  const { map } = useMap();
  const [layer, setLayer] = useState<L.MaplibreGL>();

  useEffect(() => {
    if (!layer) {
      const l = L.maplibreGL({ style: props.styleUrl }).addTo(map);

      setLayer(l);
    }
  }, [layer, map, props.styleUrl]);

  return null;
};
