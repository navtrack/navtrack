import React, { useState } from "react";
import Button from "../../shared/elements/Button";
import Checkbox from "../../shared/elements/Checkbox";
import Modal from "../../shared/elements/Modal";
import SelectInput from "../../shared/forms/SelectInput";
import TextInput from "../../shared/forms/TextInput";
import Icon from "../../shared/util/Icon";
import { AltitudeFilterModel } from "./types/AltitudeFilterModel";
import { ComparisonType } from "./types/ComparisonType";
import { NumberFilterType } from "./types/NumberFilterType";

type Props = {
  filter: AltitudeFilterModel;
  setFilter: (speedFilter: AltitudeFilterModel) => void;
  closeModal: () => void;
};

export default function AltitudeFilterModal(props: Props) {
  const [filter, setFilter] = useState(props.filter);

  const saveFilter = () => {
    const filters: Record<NumberFilterType, AltitudeFilterModel> = {
      [NumberFilterType.Single]: { ...filter, single: filter.single, enabled: true },
      [NumberFilterType.Interval]: { ...filter, min: filter.min, max: filter.max, enabled: true }
    };

    props.setFilter({ ...filters[filter.numberFilterType] });
    props.closeModal();
  };

  const setMin = (value: string) => {
    let min = parseInt(value);
    setFilter({ ...filter, min: min, max: min > filter.max ? min : filter.max });
  };
  const setMax = (value: string) => {
    let max = parseInt(value);
    setFilter({ ...filter, max: max, min: max < filter.min ? max : filter.min });
  };

  return (
    <Modal closeModal={props.closeModal}>
      <div className="font-medium text-lg mb-3">
        <Icon className="fa-mountain mr-1" />
        Altitude filter
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
          <div style={{ minWidth: 140 }}>
            <SelectInput
              value={filter.comparisonType}
              onChange={(e) => setFilter({ ...filter, comparisonType: parseInt(e.target.value) })}>
              <option value={ComparisonType.GreaterThan} key={0}>
                Greater Than
              </option>
              <option value={ComparisonType.Equals} key={1}>
                Equals
              </option>
              <option value={ComparisonType.LessThan} key={2}>
                Less Than
              </option>
            </SelectInput>
          </div>
          <div className="flex ml-1 items-center">
            <TextInput
              type="number"
              value={filter.single}
              onChange={(e) => setFilter({ ...filter, single: parseInt(e.target.value) })}>
              meters
            </TextInput>
          </div>
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
            <TextInput
              title="MIN"
              type="number"
              value={filter.min}
              onChange={(e) => setMin(e.target.value)}>
              meters
            </TextInput>
          </div>
          <div className="ml-1">
            <TextInput
              title="MAX"
              type="number"
              value={filter.max}
              onChange={(e) => setMax(e.target.value)}>
              meters
            </TextInput>
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
