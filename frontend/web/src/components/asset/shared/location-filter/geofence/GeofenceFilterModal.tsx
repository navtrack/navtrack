import { faMapMarkedAlt } from "@fortawesome/free-solid-svg-icons";
import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import {
  CircleGeofence,
  GeofenceCircle
} from "../../../../ui/map/geofence/GeofenceCircle";
import { Map } from "../../../../ui/map/Map";
import { MapMove } from "../../../../ui/map/MapMove";
import { Modal } from "../../../../ui/modal/Modal";
import { FilterModal } from "../FilterModal";
import { useCallback, useEffect, useMemo, useState } from "react";
import { LatLong } from "@navtrack/shared/api/model";
import { useAtom } from "jotai";
import {
  DEFAULT_MAP_CENTER,
  GEOFENCE_CIRCLE_DEFAULT_MAP_ZOOM
} from "@navtrack/shared/constants";
import { geofenceFilterAtom } from "../locationFilterState";

type GeofenceFilterModalProps = {
  initialMapCenter?: LatLong;
  filterKey: string;
};

type CircleGeofenceFormValues = {
  geofence?: CircleGeofence;
};

export function GeofenceFilterModal(props: GeofenceFilterModalProps) {
  const [state, setState] = useAtom(geofenceFilterAtom(props.filterKey));
  const [renderMap, setRenderMap] = useState(false);

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
        : (props.initialMapCenter ?? DEFAULT_MAP_CENTER),
    [props.initialMapCenter, state]
  );

  const zoom = useMemo(
    () => (state.map?.zoom ? state.map.zoom : GEOFENCE_CIRCLE_DEFAULT_MAP_ZOOM),
    [state]
  );

  useEffect(() => {
    if (state.open) {
      setRenderMap(true);

      return () => {
        setRenderMap(false);
      };
    }
  }, [state]);

  return (
    <Modal
      open={state.open}
      close={() => setState((prev) => ({ ...prev, open: false }))}
      className="flex max-w-(--breakpoint-md) grow">
      <Formik<CircleGeofenceFormValues>
        initialValues={state}
        onSubmit={(values: CircleGeofenceFormValues) => {
          if (values.geofence) {
            setState((current) => ({
              ...current,
              geofence: values.geofence,
              active: true,
              open: false
            }));
          }
        }}>
        {({ values, setValues }) => (
          <Form className="flex grow">
            <FilterModal
              icon={faMapMarkedAlt}
              onCancel={() => setState((prev) => ({ ...prev, open: false }))}
              className="flex min-w-full flex-col">
              <h3 className="text-lg font-medium leading-6 text-gray-900">
                <FormattedMessage id="locations.filter.geofence.title" />
              </h3>
              <div className="mt-4 flex grow" style={{ height: "400px" }}>
                {renderMap && (
                  <Map center={center} initialZoom={zoom}>
                    <MapMove onMove={handleMapMove} />
                    <GeofenceCircle
                      geofence={values.geofence}
                      onChange={(geofence) => setValues({ geofence: geofence })}
                    />
                  </Map>
                )}
              </div>
            </FilterModal>
          </Form>
        )}
      </Formik>
    </Modal>
  );
}
