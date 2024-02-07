import styled from "@emotion/styled";
import { ReactNode } from "react";
import { MapContainer, TileLayer } from "react-leaflet";
import { LongLat } from "./types";
import { VectorTileLayer } from "./VectorTileLayer";
import "maplibre-gl/dist/maplibre-gl.css";
import { MapZoom } from "./MapZoom";
import { DEFAULT_MAP_ZOOM } from "../../../constants";
import { appConfigAtom } from "@navtrack/shared/state/appConfig";
import { useRecoilValue } from "recoil";

type MapProps = {
  center: LongLat;
  initialZoom?: number;
  children?: ReactNode;
  hideZoomControl?: boolean;
  hideAttribution?: boolean;
};

const StyledDiv = styled("div")`
  .leaflet-pane {
    z-index: 10;
  }
  .leaflet-control {
    z-index: 11;
  }
  .leaflet-top {
    z-index: 11;
  }
  .leaflet-bottom {
    z-index: 11;
  }
`;

export function Map(props: MapProps) {
  const appConfig = useRecoilValue(appConfigAtom);

  return (
    <StyledDiv className="relative flex flex-grow">
      <MapContainer
        center={[props.center.latitude, props.center.longitude]}
        zoom={props.initialZoom ?? DEFAULT_MAP_ZOOM}
        className="h-full w-full"
        zoomControl={!props.hideZoomControl}
        attributionControl={!props.hideAttribution}>
        <MapZoom initialZoom={props.initialZoom} />
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
    </StyledDiv>
  );
}
