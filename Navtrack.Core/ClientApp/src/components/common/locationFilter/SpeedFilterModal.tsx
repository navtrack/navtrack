import React, { useState } from "react";
import Button from "../../shared/elements/Button";
import Checkbox from "../../shared/elements/Checkbox";
import Modal from "../../shared/elements/Modal";
import Icon from "../../shared/util/Icon";
import { ComparisonType } from "./types/ComparisonType";
import { NumberFilterType } from "./types/NumberFilterType";
import { SpeedFilterModel } from "./types/SpeedFilterModel";

type Props = {
  filter: SpeedFilterModel;
  setFilter: (filter: SpeedFilterModel) => void;
  closeModal: () => void;
};

export default function SpeedFilterModal(props: Props) {
  const [filter, setFilter] = useState(props.filter);

  const saveFilter = () => {
    props.setFilter({ ...filter, enabled: true });
    props.closeModal();
  };

  const setMin = (speed: string) => {
    let min = parseInt(speed);
    setFilter({ ...filter, min: min, max: min > filter.max ? min : filter.max });
  };
  const setMax = (speed: string) => {
    let max = parseInt(speed);
    setFilter({ ...filter, max: max, min: max < filter.min ? max : filter.min });
  };

  return (
    <Modal closeModal={props.closeModal}>
      <div className="font-medium text-lg mb-3">
        <Icon className="fa-tachometer-alt mr-1" />
        Speed filter
      </div>
      <div className="flex flex-col cursor-default text-sm">
        <Checkbox
          className="mb-1"
          checked={filter.numberFilterType === NumberFilterType.Single}
          readOnly
          onClick={() => setFilter({ ...filter, numberFilterType: NumberFilterType.Single })}>
          Single
        </Checkbox>
        <div
          className="mb-1 ml-5 flex flex-row items-center"
          onClick={() => setFilter({ ...filter, numberFilterType: NumberFilterType.Single })}>
          <div className="text-sm relative" style={{ width: "110px" }}>
            <div className="relative shadow rounded w-full">
              <select
                className="block appearance-none bg-white p-1 cursor-pointer focus:outline-none bg-gray-200 w-full text-gray-700"
                value={filter.comparisonType}
                onChange={(e) =>
                  setFilter({ ...filter, comparisonType: parseInt(e.target.value) })
                }>
                <option value={ComparisonType.GreaterThan} key={0}>
                  Greater Than
                </option>
                <option value={ComparisonType.Equals} key={1}>
                  Equals
                </option>
                <option value={ComparisonType.LessThan} key={2}>
                  Less Than
                </option>
              </select>
              <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 pt-1">
                <i className="fas fa-chevron-down" />
              </div>
            </div>
          </div>
          <input
            type="number"
            className="ml-3 w-12 mt-1 mb-1 bg-gray-200 text-gray-700 shadow rounded p-1 focus:outline-none cursor-pointer"
            onClick={() => setFilter({ ...filter, numberFilterType: NumberFilterType.Interval })}
            value={filter.single}
            onChange={(e) => setFilter({ ...filter, single: parseInt(e.target.value) })}
          />
          <span className="ml-1">km/h</span>
        </div>
        <Checkbox
          className="mb-1"
          checked={filter.numberFilterType === NumberFilterType.Interval}
          readOnly
          onClick={() => setFilter({ ...filter, numberFilterType: NumberFilterType.Interval })}>
          Interval
        </Checkbox>
        <div
          className="mb-1 flex flex-row items-center"
          onClick={() => setFilter({ ...filter, numberFilterType: NumberFilterType.Interval })}>
          <div className="ml-5">
            <div className="text-xs uppercase font-semibold text-gray-700">Min</div>
            <div className="text-sm relative">
              <input
                type="number"
                className="w-12 mt-1 mb-1 bg-gray-200 text-gray-700 shadow rounded p-1 focus:outline-none cursor-pointer"
                onClick={() =>
                  setFilter({ ...filter, numberFilterType: NumberFilterType.Interval })
                }
                value={filter.min}
                onChange={(e) => setMin(e.target.value)}
              />
              <span className="ml-1">-</span>
            </div>
          </div>
          <div className="ml-1">
            <div className="text-xs uppercase font-semibold text-gray-700">Max</div>
            <div className="text-sm relative">
              <input
                type="number"
                className="w-12 mt-1 mb-1 bg-gray-200 text-gray-700 shadow rounded p-1 focus:outline-none cursor-pointer"
                onClick={() =>
                  setFilter({ ...filter, numberFilterType: NumberFilterType.Interval })
                }
                value={filter.max}
                onChange={(e) => setMax(e.target.value)}
              />
              <span className="ml-1">km/h</span>
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
