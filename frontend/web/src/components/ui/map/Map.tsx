import { ReactNode, useRef } from "react";
import { MapContainer, TileLayer } from "react-leaflet";
import { VectorTileLayer } from "./VectorTileLayer";
import "maplibre-gl/dist/maplibre-gl.css";
import { DEFAULT_MAP_CENTER, DEFAULT_MAP_ZOOM } from "../../../constants";
import { appConfigAtom } from "@navtrack/shared/state/appConfig";
import { useRecoilValue } from "recoil";
import { MapZoomControl } from "./MapZoomControl";
import { LatLong } from "@navtrack/shared/api/model";
import { MapResizeObserver } from "./MapResizeObserver";

type MapProps = {
  center?: LatLong;
  initialZoom?: number;
  children?: ReactNode;
  hideZoomControl?: boolean;
  hideAttribution?: boolean;
};

export function Map(props: MapProps) {
  const mapContainerRef = useRef<HTMLDivElement>(null);
  const appConfig = useRecoilValue(appConfigAtom);

  const center = props.center ?? DEFAULT_MAP_CENTER;

  return (
    <div className="relative flex flex-grow" ref={mapContainerRef}>
      <MapContainer
        center={[center.latitude, center.longitude]}
        zoom={props.initialZoom ?? DEFAULT_MAP_ZOOM}
        className="h-full w-full"
        zoomControl={false}
        maxZoom={20}
        attributionControl={!props.hideAttribution}>
        <MapResizeObserver mapContainerRef={mapContainerRef} />
        {!props.hideZoomControl && <MapZoomControl />}
        {appConfig?.map.tileUrl ? (
          <VectorTileLayer styleUrl={appConfig.map.tileUrl} />
        ) : (
          <TileLayer
            attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
            url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          />
        )}
        {props.children}
      </MapContainer>
    </div>
  );
}
