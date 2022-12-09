import styled from "@mui/styled-engine";
import { ReactNode } from "react";
import { MapContainer } from "react-leaflet";
import { LatLng } from "./types";
import { VectorTileLayer } from "./VectorTileLayer";
import "maplibre-gl/dist/maplibre-gl.css";
import { MapZoom } from "./MapZoom";

type MapProps = {
  center: LatLng;
  zoom?: number;
  children?: ReactNode;
  hideZoomControl?: boolean;
  hideAttribution?: boolean;
};

const StyledDiv = styled("div")`
  .leaflet-pane {
    z-index: 20;
  }
  .leaflet-control {
    z-index: 25;
  }
  .leaflet-top {
    z-index: 30;
  }
  .leaflet-bottom {
    z-index: 30;
  }
`;

export const Map = (props: MapProps) => {
  return (
    <StyledDiv className="relative flex flex-grow">
      {props.center !== undefined && (
        <MapContainer
          center={[props.center.latitude, props.center.longitude]}
          zoom={props.zoom ?? 13}
          className=" h-full w-full"
          zoomControl={!props.hideZoomControl}
          attributionControl={!props.hideAttribution}>
          <MapZoom zoom={props.zoom} />
          <VectorTileLayer
            styleUrl="https://tiles.stadiamaps.com/styles/osm_bright.json"
            // attribution={
            //   '&copy; <a href="https://stadiamaps.com/">Stadia Maps</a>, &copy; <a href="https://openmaptiles.org">OpenMapTiles</a>, &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors'
            // }
          />
          {props.children}
        </MapContainer>
      )}
    </StyledDiv>
  );
};
