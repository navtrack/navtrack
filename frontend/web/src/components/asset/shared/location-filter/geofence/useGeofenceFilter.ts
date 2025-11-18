import { useCallback, useMemo } from "react";
import { useAtom } from "jotai";
import { geofenceFilterAtom } from "../locationFilterState";
import { CircleGeofence } from "../../../../ui/map/geofence/GeofenceCircle";
import { LatLong } from "@navtrack/shared/api/model";
import {
  DEFAULT_MAP_CENTER,
  GEOFENCE_CIRCLE_DEFAULT_MAP_ZOOM
} from "@navtrack/shared/constants";

export function useGeofenceFilter(
  filterKey: string,
  initialMapCenter?: LatLong
) {
  const [state, setState] = useAtom(geofenceFilterAtom(filterKey));

  const close = useCallback(
    () => setState((x) => ({ ...x, open: false })),
    [setState]
  );

  const handleSubmit = useCallback(
    (values: CircleGeofence) => {
      setState((current) => ({
        ...current,
        geofence: values,
        enabled: true,
        open: false
      }));
    },
    [setState]
  );

  const handleMapMove = useCallback(
    (center: LatLong, zoom: number) => {
      setState((current) => ({
        ...current,
        map: {
          center: center,
          zoom: zoom
        }
      }));
    },
    [setState]
  );

  const center = useMemo(
    () =>
      state.map?.center
        ? state.map.center
        : (initialMapCenter ?? DEFAULT_MAP_CENTER),
    [initialMapCenter, state.map?.center]
  );

  const zoom = useMemo(
    () =>
      state.map?.zoom ? state.map?.zoom : GEOFENCE_CIRCLE_DEFAULT_MAP_ZOOM,
    [state.map?.zoom]
  );

  return {
    state,
    center,
    zoom,
    close,
    handleSubmit,
    handleMapMove
  };
}
