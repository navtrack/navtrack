import styled from "@mui/styled-engine";
import { settingsSelector } from "@navtrack/navtrack-app-shared";
import { ReactNode } from "react";
import { MapContainer } from "react-leaflet";
import { useRecoilValue } from "recoil";
import { LatLng } from "./types";
import VectorTileLayer from "react-leaflet-vector-tile-layer";

interface IMap {
  center: LatLng;
  zoom?: number;
  children?: ReactNode;
  hideZoomControl?: boolean;
  hideAttribution?: boolean;
}

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

export default function Map(props: IMap) {
  const settings = useRecoilValue(settingsSelector);

  return (
    <StyledDiv className="relative flex flex-grow">
      {props.center !== undefined && (
        <MapContainer
          center={[props.center.latitude, props.center.longitude]}
          zoom={props.zoom ?? 13}
          className="absolute top-0 left-0 h-full w-full rounded-lg"
          zoomControl={!props.hideZoomControl}
          attributionControl={!props.hideAttribution}>
          <VectorTileLayer
            styleUrl={settings["Map.TileUrl"]}
            attribution={
              '&copy; <a href="https://stadiamaps.com/">Stadia Maps</a>, &copy; <a href="https://openmaptiles.org">OpenMapTiles</a>, &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors'
            }
          />
          {props.children}
        </MapContainer>
      )}
    </StyledDiv>
  );
}
