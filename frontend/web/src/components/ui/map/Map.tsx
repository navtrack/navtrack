import styled from "@mui/styled-engine";
import { ReactNode } from "react";
import { MapContainer } from "react-leaflet";
import { LongLat } from "./types";
import { VectorTileLayer } from "./VectorTileLayer";
import "maplibre-gl/dist/maplibre-gl.css";
import { MapZoom } from "./MapZoom";
import { DEFAULT_MAP_ZOOM } from "../../../constants";

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

export const Map = (props: MapProps) => {
  return (
    <StyledDiv className="relative flex flex-grow">
      <MapContainer
        center={[props.center.latitude, props.center.longitude]}
        zoom={props.initialZoom ?? DEFAULT_MAP_ZOOM}
        className="h-full w-full"
        zoomControl={!props.hideZoomControl}
        attributionControl={!props.hideAttribution}>
        <MapZoom initialZoom={props.initialZoom} />
        <VectorTileLayer
          styleUrl="https://tiles.stadiamaps.com/styles/osm_bright.json"
          // attribution={
          //   '&copy; <a href="https://stadiamaps.com/">Stadia Maps</a>, &copy; <a href="https://openmaptiles.org">OpenMapTiles</a>, &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors'
          // }
        />
        {props.children}
      </MapContainer>
    </StyledDiv>
  );
};
