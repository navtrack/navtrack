import { FormattedMessage } from "react-intl";
import { showCoordinate } from "@navtrack/shared/utils/coordinates";
import { useEffect, useState } from "react";
import { useMap } from "../useMap";
import { Circle, circle } from "leaflet";
import { GEOFENCE_CIRCLE_MAX_RADIUS_METERS as GEOFENCE_CIRCLE_MAX_RADIUS } from "@navtrack/shared/constants";
import { useShow } from "@navtrack/shared/hooks/util/useShow";

export type CircleGeofence = {
  latitude: number;
  longitude: number;
  radius: number;
};

export type GeofenceCircleProps = {
  geofence?: CircleGeofence;
  onChange?: (geofence: CircleGeofence) => void;
};

export function GeofenceCircle(props: GeofenceCircleProps) {
  const map = useMap();
  const show = useShow();
  const [initialised, setInitialised] = useState(false);
  const [clicked, setClicked] = useState(false);
  const [geofence, setGeofence] = useState(props.geofence);

  useEffect(() => {
    if (!initialised) {
      setInitialised(true);

      map.leafletMap.pm.setGlobalOptions({
        snappingOrder: ["Circle"],
        maxRadiusCircle: GEOFENCE_CIRCLE_MAX_RADIUS
      });

      if (props.geofence !== undefined) {
        map.leafletMap.addLayer(
          circle([props.geofence.latitude, props.geofence.longitude], {
            radius: props.geofence.radius
          })
        );
      }

      let centerPlaced = false;

      map.leafletMap.on("pm:drawstart", (event) => {
        event.workingLayer.on("pm:centerplaced", (innerE) => {
          if (innerE.shape === "Circle") {
            centerPlaced = true;
            const center = (event.workingLayer as Circle).getLatLng();

            setGeofence(() => ({
              latitude: parseFloat(center.lat.toFixed(6)),
              longitude: parseFloat(center.lng.toFixed(6)),
              radius: 0
            }));

            if (!clicked) {
              setClicked(true);
            }

            map.leafletMap.pm.getGeomanLayers().forEach((layer) => {
              map.leafletMap.removeLayer(layer);
            });
          }
        });

        event.workingLayer.on("pm:change", (innerE) => {
          if (innerE.shape === "Circle" && centerPlaced) {
            setGeofence((current) =>
              current
                ? {
                    ...current,
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
      });

      map.leafletMap.on("pm:drawend", (e) => {
        if (e.shape === "Circle") {
          const circle = map.leafletMap.pm
            .getGeomanLayers()
            .find((x) => x instanceof Circle) as Circle;

          if (circle && centerPlaced) {
            centerPlaced = false;

            map.leafletMap.off("pm:centerplaced");
            map.leafletMap.off("pm:change");

            const state = {
              latitude: parseFloat(circle.getLatLng().lat.toFixed(6)),
              longitude: parseFloat(circle.getLatLng().lng.toFixed(6)),
              radius: Math.round(Math.min(circle.getRadius() + 1, 1000))
            };

            setGeofence(state);
            props.onChange?.(state);

            map.leafletMap.pm.enableDraw("Circle");
          }
        }
      });

      map.leafletMap.pm.enableDraw("Circle");
    }
  }, [clicked, geofence, initialised, map, map.leafletMap, props, setGeofence]);

  return (
    <div className="relative mx-auto flex justify-center p-2 font-sans text-xs">
      <div className="flex space-x-2 rounded bg-white px-2 py-1 text-left">
        {clicked && geofence !== undefined ? (
          <>
            <div>
              <span className="flex font-semibold">
                <FormattedMessage id="generic.latitude" />
              </span>
              <span>{showCoordinate(geofence.latitude)}</span>
            </div>
            <div>
              <span className="flex font-semibold">
                <FormattedMessage id="generic.longitude" />
              </span>
              <span>{showCoordinate(geofence.longitude)}</span>
            </div>
            <div>
              <span className="flex font-semibold">
                <FormattedMessage id="generic.radius" />
              </span>
              <span>{show.distance(geofence.radius, false)}</span>
            </div>
          </>
        ) : (
          <FormattedMessage id="locations.filter.geofence.select-center" />
        )}
      </div>
    </div>
  );
}
