import { faMapMarkedAlt } from "@fortawesome/free-solid-svg-icons";
import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import {
  CircleGeofence,
  GeofenceCircle
} from "../../../../ui/map/geofence/GeofenceCircle";
import { Map } from "../../../../ui/map/Map";
import { MapMove } from "../../../../ui/map/MapMove";
import { LongLat } from "../../../../ui/map/mapTypes";
import { Modal } from "../../../../ui/modal/Modal";
import { FilterModal } from "../FilterModal";
import { useGeofenceFilter } from "./useGeofenceFilter";
import { useEffect, useState } from "react";

type GeofenceFilterModalProps = {
  initialMapCenter?: LongLat;
  filterKey: string;
};

type CircleGeofenceFormValues = {
  geofence?: CircleGeofence;
};

export function GeofenceFilterModal(props: GeofenceFilterModalProps) {
  const filter = useGeofenceFilter(props.filterKey, props.initialMapCenter);
  const [renderMap, setRenderMap] = useState(false);

  useEffect(() => {
    if (filter.state.open) {
      setRenderMap(true);

      return () => {
        setRenderMap(false);
      };
    }
  }, [filter.state.open]);

  return (
    <Modal
      open={filter.state.open}
      close={filter.close}
      className="flex max-w-screen-md flex-grow">
      <Formik<CircleGeofenceFormValues>
        initialValues={{ geofence: filter.state.geofence }}
        onSubmit={(values) =>
          values.geofence ? filter.handleSubmit(values.geofence) : undefined
        }>
        {({ values, setValues }) => (
          <Form className="flex flex-grow">
            <FilterModal
              icon={faMapMarkedAlt}
              onCancel={filter.close}
              className="flex min-w-full flex-col">
              <h3 className="text-lg font-medium leading-6 text-gray-900">
                <FormattedMessage id="locations.filter.geofence.title" />
              </h3>
              <div className="mt-4 flex flex-grow" style={{ height: "400px" }}>
                {renderMap && (
                  <Map center={filter.center} initialZoom={filter.zoom}>
                    <>{console.log("center", filter.center, filter.zoom)}</>
                    <MapMove onMove={filter.handleMapMove} />
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
