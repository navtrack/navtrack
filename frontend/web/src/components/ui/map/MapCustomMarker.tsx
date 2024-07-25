import L from "leaflet";
import { renderToString } from "react-dom/server";
import { ReactNode, useEffect, useMemo, useRef, useState } from "react";
import { MapCenter } from "./MapCenter";
import { LatLongModel } from "@navtrack/shared/api/model/generated";
import { Marker } from "react-leaflet";
import { createPortal } from "react-dom";

type MapCustomMarkerProps = {
  coordinates?: LatLongModel;
  follow?: boolean;
  zIndexOffset?: number;
  children?: ReactNode;
};

type MapCustomMarkerPosition = {
  left: number;
  bottom: number;
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
      html: renderToString(<div id={id}></div>)
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
        left: markerRef.current.scrollWidth / -2,
        bottom: markerRef.current.scrollHeight
      });
    }
  }, [div]);

  if (props.coordinates !== undefined) {
    return (
      <>
        <Marker
          position={[props.coordinates.latitude, props.coordinates.longitude]}
          icon={divIcon}
        />
        {div !== undefined &&
          createPortal(
            <div
              ref={markerRef}
              style={{ left: position?.left, bottom: position?.bottom }}
              className="relative flex">
              <div>{props.children}</div>
            </div>,
            div
          )}
        {props.follow && <MapCenter position={props.coordinates} />}
      </>
    );
  }

  return null;
}
