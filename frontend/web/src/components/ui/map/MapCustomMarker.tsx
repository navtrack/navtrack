import L from "leaflet";
import { renderToString } from "react-dom/server";
import { ReactNode, useEffect, useMemo, useRef, useState } from "react";
import { MapCenter } from "./MapCenter";
import { LatLong } from "@navtrack/shared/api/model/generated";
import { Marker } from "react-leaflet";
import { createPortal } from "react-dom";

type MapCustomMarkerProps = {
  coordinates?: LatLong;
  follow?: boolean;
  zIndexOffset?: number;
  children?: ReactNode;
};

type MapCustomMarkerPosition = {
  width: number;
  height: number;
  left: number;
  top: number;
};

export function MapCustomMarker(props: MapCustomMarkerProps) {
  const markerRef = useRef<HTMLDivElement>(null);
  const [position, setPosition] = useState<MapCustomMarkerPosition>();
  const [div, setDiv] = useState<HTMLElement>();

  const id = useMemo(
    () =>
      `id${props.coordinates?.latitude}${props.coordinates?.longitude}`.replaceAll(
        ".",
        ""
      ),
    [props.coordinates?.latitude, props.coordinates?.longitude]
  );

  const divIcon = useMemo(() => {
    return L.divIcon({
      className: undefined,
      html: renderToString(<div id={id}></div>),
      iconAnchor: [0, 0]
    });
  }, [id]);

  useEffect(() => {
    if (props.coordinates !== null) {
      const div = document.getElementById(id);

      if (div !== null) {
        setDiv(div);
      }
    }
  }, [id, props.coordinates]);

  useEffect(() => {
    if (markerRef.current !== null && div !== undefined) {
      setPosition({
        width: markerRef.current.clientWidth,
        height: markerRef.current.clientHeight,
        left: markerRef.current.clientWidth / -2, //markerRef.current.scrollWidth / -3,
        top: markerRef.current.clientHeight * -1 + 6 //markerRef.current.scrollHeight - 12
      });
    }
  }, [div]);

  if (props.coordinates !== undefined) {
    return (
      <>
        {divIcon !== undefined && (
          <Marker
            position={[props.coordinates.latitude, props.coordinates.longitude]}
            icon={divIcon}
          />
        )}
        {div !== undefined &&
          createPortal(
            <div
              style={{ left: position?.left, top: position?.top }}
              className="relative flex">
              <div ref={markerRef}>{props.children}</div>
            </div>,
            div
          )}
        {props.follow && <MapCenter position={props.coordinates} />}
      </>
    );
  }

  return null;
}
