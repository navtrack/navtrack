import { useCallback, useMemo } from "react";
import { useRecoilState } from "recoil";
import { DEFAULT_MAP_CENTER } from "../../../../../constants";
import { CircleGeofence, LatLng } from "../../../../ui/shared/map/types";
import { geofenceFilterAtom } from "../state";

export default function useGeofenceFilter(
  filterKey: string,
  initialMapCenter?: LatLng
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

  const initialValues = useMemo<CircleGeofence>(
    () =>
      state.geofence ?? {
        latitude: 0,
        longitude: 0,
        radius: 0
      },
    [state.geofence]
  );

  const handleMapMove = useCallback(
    (center: LatLng, zoom: number) => {
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
      state.map?.zoom
        ? state.map?.zoom
        : center === DEFAULT_MAP_CENTER
        ? 2
        : undefined,
    [center, state.map?.zoom]
  );

  return {
    state,
    initialValues,
    center,
    zoom,
    close,
    handleSubmit,
    handleMapMove
  };
}
