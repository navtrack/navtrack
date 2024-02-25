import styled from "@emotion/styled";
import { ReactNode } from "react";
import { MapContainer, TileLayer } from "react-leaflet";
import { VectorTileLayer } from "./VectorTileLayer";
import "maplibre-gl/dist/maplibre-gl.css";
import { MapZoom } from "./MapZoom";
import { DEFAULT_MAP_ZOOM } from "../../../constants";
import { appConfigAtom } from "@navtrack/shared/state/appConfig";
import { useRecoilValue } from "recoil";
import { MapZoomControl } from "./MapZoomControl";
import { LongLat } from "./mapTypes";

type MapProps = {
  center: LongLat;
  initialZoom?: number;
  children?: ReactNode;
  hideZoomControl?: boolean;
  hideAttribution?: boolean;
};

const StyledDiv = styled("div")`
  .leaflet-pane {
    z-index: 0;
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
        zoomControl={false}
        attributionControl={!props.hideAttribution}>
        <MapZoomControl />
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
