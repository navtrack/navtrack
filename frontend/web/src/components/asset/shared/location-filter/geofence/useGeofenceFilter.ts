import { useCallback, useMemo } from "react";
import { useRecoilState } from "recoil";
import {
  DEFAULT_MAP_CENTER,
  GEOFENCE_CIRCLE_DEFAULT_MAP_ZOOM
} from "../../../../../constants";
import { LongLat } from "../../../../ui/map/mapTypes";
import { geofenceFilterAtom } from "../locationFilterState";
import { CircleGeofence } from "../../../../ui/map/geofence/GeofenceCircle";

export function useGeofenceFilter(
  filterKey: string,
  initialMapCenter?: LongLat
) {
  const [state, setState] = useRecoilState(geofenceFilterAtom(filterKey));

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
    (center: LongLat, zoom: number) => {
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
        : initialMapCenter ?? DEFAULT_MAP_CENTER,
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
