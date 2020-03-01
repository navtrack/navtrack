import React, { useState } from "react";
import Modal from "components/Framework/Elements/Modal";
import Button from "components/Framework/Elements/Button";
import { CoordinatesFilterModel } from "./types/CoordinatesFilterModel";
import Icon from "components/Framework/Util/Icon";

type Props = {
  filter: CoordinatesFilterModel;
  setFilter: (filter: CoordinatesFilterModel) => void;
  closeModal: () => void;
};

export default function CoordinatesFilterModal(props: Props) {
  const [filter, setFilter] = useState(props.filter);

  const saveFilter = () => {
    props.setFilter({ ...filter, enabled: true });
    props.closeModal();
  };

  return (
    <Modal closeModal={props.closeModal}>
      <div className="font-medium text-lg mb-3">
        <Icon className="fa-location-arrow mr-1" />
        Coordinates filter
      </div>
      <div className="flex flex-col cursor-default text-sm">
        <div className="mb-1 flex flex-row items-center">
          <div>
            <div className="text-xs uppercase font-semibold text-gray-700">Latitude</div>
            <div className="text-sm relative">
              <input
                type="number"
                className="mt-1 mb-1 bg-gray-200 text-gray-700 shadow rounded p-1 focus:outline-none cursor-pointer"
                placeholder="Latitude"
                value={filter.latitude}
                onChange={e => setFilter({ ...filter, latitude: parseInt(e.target.value) })}
                style={{ width: "110px" }}
              />
            </div>
          </div>
          <div className="ml-3">
            <div className="text-xs uppercase font-semibold text-gray-700">Longitude</div>
            <div className="text-sm relative">
              <input
                type="number"
                className="mt-1 mb-1 bg-gray-200 text-gray-700 shadow rounded p-1 focus:outline-none cursor-pointer"
                placeholder="Longitude"
                value={filter.longitude}
                onChange={e => setFilter({ ...filter, longitude: parseInt(e.target.value) })}
                style={{ width: "110px" }}
              />
            </div>
          </div>
        </div>
        <div className="mb-1 flex flex-row items-center">
          <div>
            <div className="text-xs uppercase font-semibold text-gray-700">Radius</div>
            <div className="text-sm relative">
              <input
                type="number"
                className="mt-1 mb-1 bg-gray-200 text-gray-700 shadow rounded p-1 focus:outline-none cursor-pointer"
                placeholder="Radius"
                value={filter.radius}
                onChange={e => setFilter({ ...filter, radius: parseInt(e.target.value) })}
                style={{ width: "110px" }}
              />
              <span className="ml-1">meters</span>
            </div>
          </div>
        </div>
      </div>
      <div className="flex justify-end mt-3">
        <Button color="secondary" onClick={props.closeModal} className="mr-3">
          Cancel
        </Button>
        <Button color="primary" onClick={saveFilter}>
          Save
        </Button>
      </div>
    </Modal>
  );
}
