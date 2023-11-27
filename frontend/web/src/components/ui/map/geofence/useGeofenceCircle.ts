import { circle } from "leaflet";
import { Circle, Layer } from "leaflet";
import { useCallback, useEffect, useMemo, useState } from "react";
import { IGeofenceCircle } from "../types";
import { useMap } from "../useMap";

// TODO refactor this

export function useGeofenceCircle(props: IGeofenceCircle) {
  const { map } = useMap();
  const [initialised, setInitialised] = useState(false);
  const [clicked, setClicked] = useState(false);
  const [geofence, setGeofence] = useState(props.geofence);
  const [drawing, setDrawing] = useState(false);
  const [onChangeCalled, setOnChangeCalled] = useState(false);

  const initCircle = useMemo(
    () =>
      props.geofence
        ? circle([props.geofence.latitude, props.geofence.longitude], {
            radius: props.geofence.radius
          })
        : undefined,
    [props.geofence]
  );

  useEffect(() => {
    if (
      initialised &&
      clicked &&
      !drawing &&
      geofence &&
      props.onChange &&
      !onChangeCalled
    ) {
      props.onChange(geofence);
      setOnChangeCalled(true);
    }
  }, [clicked, drawing, geofence, initialised, onChangeCalled, props]);

  const setMapOptions = useCallback(() => {
    map.pm.setGlobalOptions({
      snappingOrder: ["Circle"],
      panes: {
        vertexPane: "markerPane",
        layerPane: "overlayPane",
        markerPane: "markerPane"
      },
      maxRadiusCircle: 1000
    });
  }, [map.pm]);

  useEffect(() => {
    if (!initialised) {
      setInitialised(true);
      setMapOptions();
      let centerPlaced = false;

      if (initCircle) {
        initCircle.addTo(map);
      }

      map.on("pm:drawstart", function (event) {
        map.on("mousemove", () => {
          if (centerPlaced) {
            setGeofence((current) =>
              current
                ? {
                    ...current,
                    mapCenter: map.getCenter(),
                    mapZoom: map.getZoom(),
                    radius: Math.round(
                      Math.min(
                        (event.workingLayer as Circle).getRadius() + 1,
                        1000
                      )
                    )
                  }
                : undefined
            );
          }
        });

        event.workingLayer.on("pm:centerplaced", function (event) {
          if (event.workingLayer && event.workingLayer instanceof Circle) {
            if (initCircle) {
              initCircle.remove();
            }
            const circleLayer = event.workingLayer as Circle;
            const center = circleLayer.getLatLng();
            centerPlaced = true;
            setDrawing(true);

            setGeofence(() => ({
              latitude: parseFloat(center.lat.toFixed(6)),
              longitude: parseFloat(center.lng.toFixed(6)),
              radius: 0,
              mapCenter: map.getCenter(),
              mapZoom: map.getZoom()
            }));

            if (!clicked) {
              setClicked(true);
            }

            const geomanLayers = map.pm.getGeomanDrawLayers() as Layer[];
            if (geomanLayers.length > 0) {
              const circle = geomanLayers[0];
              map.removeLayer(circle);
            }
          }
        });
      });

      map.pm.enableDraw("Circle");

      map.on("pm:drawend", () => {
        centerPlaced = false;
        map.pm.enableDraw("Circle");
        setDrawing(false);
        setOnChangeCalled(false);
      });
    }
  }, [
    clicked,
    geofence,
    initCircle,
    initialised,
    map,
    map.pm,
    props,
    setGeofence,
    setMapOptions
  ]);

  return { clicked, geofence };
}
