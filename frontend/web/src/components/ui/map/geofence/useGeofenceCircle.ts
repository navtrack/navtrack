import { circle } from "leaflet";
import { Circle, Layer } from "leaflet";
import { useCallback, useEffect, useMemo, useState } from "react";
import { IGeofenceCircle } from "../types";
import { useMap } from "../useMap";

// TODO refactor this

export function useGeofenceCircle(props: IGeofenceCircle) {
  const map = useMap();
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
    map.leafletMap.pm.setGlobalOptions({
      snappingOrder: ["Circle"],
      panes: {
        vertexPane: "markerPane",
        layerPane: "overlayPane",
        markerPane: "markerPane"
      },
      maxRadiusCircle: 1000
    });
  }, [map.leafletMap]);

  useEffect(() => {
    if (!initialised) {
      setInitialised(true);
      setMapOptions();
      let centerPlaced = false;

      if (initCircle) {
        initCircle.addTo(map.leafletMap);
      }

      map.leafletMap.on("pm:drawstart", function (event) {
        map.leafletMap.on("mousemove", () => {
          if (centerPlaced) {
            setGeofence((current) =>
              current
                ? {
                    ...current,
                    mapCenter: map.leafletMap.getCenter(),
                    mapZoom: map.leafletMap.getZoom(),
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
              mapCenter: map.leafletMap.getCenter(),
              mapZoom: map.leafletMap.getZoom()
            }));

            if (!clicked) {
              setClicked(true);
            }

            const geomanLayers = map.leafletMap.pm.getGeomanDrawLayers() as Layer[];
            if (geomanLayers.length > 0) {
              const circle = geomanLayers[0];
              map.leafletMap.removeLayer(circle);
            }
          }
        });
      });

      map.leafletMap.pm.enableDraw("Circle");

      map.leafletMap.on("pm:drawend", () => {
        centerPlaced = false;
        map.leafletMap.pm.enableDraw("Circle");
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
    map.leafletMap,
    props,
    setGeofence,
    setMapOptions
  ]);

  return { clicked, geofence };
}
