import classNames from "classnames";
import React, { useState } from "react";
import useClickOutside from "../../../services/hooks/useClickOutside";
import Icon from "../../shared/util/Icon";
import AltitudeFilterModal from "./AltitudeFilterModal";
import CoordinatesFilterModal from "./CoordinatesFilterModal";
import DateFilterModal from "./DateFilterModal";
import SpeedFilterModal from "./SpeedFilterModal";
import { altitudeFilterToString, DefaultAltitudeFilterModel } from "./types/AltitudeFilterModel";
import {
  coordinatesFilterToString,
  DefaultCoordinatesFilterModel
} from "./types/CoordinatesFilterModel";
import { dateFilterToString, DefaultDateFilterModel } from "./types/DateFilterModel";
import { FilterType } from "./types/FilterType";
import { LocationFilterModel } from "./types/LocationFilterModel";
import { speedFilterToString, DefaultSpeedFilterModel } from "./types/SpeedFilterModel";

type Props = {
  filter: LocationFilterModel;
  setFilter: React.Dispatch<React.SetStateAction<LocationFilterModel>>;
};

export default function LocationFilter(props: Props) {
  const [showAddFilterMenu, setShowAddFilterMenu, hideAddFilterMenu] = useClickOutside();
  const [showDateFilter, setShowDateFilter, hideDateFilterModal] = useClickOutside();
  const [
    showCoordinatesFilter,
    setShowCoordinatesFilter,
    hideCoordinatesFilter
  ] = useClickOutside();
  const [showSpeedFilter, setShowSpeedFilter, hideSpeedFilter] = useClickOutside();
  const [showAltitudeFilter, setShowAltitudeFilter, hideAltitudeFilter] = useClickOutside();

  const [filterOrders, setFilterOrders] = useState<FilterType[]>([]);

  const handleAddFilter = (
    value: React.SetStateAction<LocationFilterModel>,
    filterType: FilterType
  ) => {
    props.setFilter(value);
    setFilterOrders([...filterOrders, filterType]);
  };

  const handleFilterDelete = (
    event: React.MouseEvent<HTMLSpanElement, MouseEvent>,
    filter: LocationFilterModel,
    filterType: FilterType
  ) => {
    event.stopPropagation();
    event.nativeEvent.stopPropagation();
    props.setFilter(filter);
    setFilterOrders(filterOrders.filter((x) => x !== filterType).slice());
  };

  const getOrder = (filterType: FilterType): number => {
    return filterOrders.findIndex((x) => x === filterType);
  };

  return (
    <div className="bg-white rounded-t p-3 flex items-center z-20">
      <Icon className="fa-filter" />
      {props.filter.date.enabled && (
        <Filter
          icon="fa-calendar-alt"
          displayText={dateFilterToString(props.filter.date)}
          onClick={setShowDateFilter}
          order={getOrder(FilterType.Date)}
          onClickDelete={(e) =>
            handleFilterDelete(
              e,
              { ...props.filter, date: DefaultDateFilterModel },
              FilterType.Date
            )
          }
        />
      )}
      {props.filter.speed.enabled && (
        <Filter
          icon="fa-tachometer-alt"
          displayText={speedFilterToString(props.filter.speed)}
          onClick={setShowSpeedFilter}
          order={getOrder(FilterType.Speed)}
          onClickDelete={(e) =>
            handleFilterDelete(
              e,
              { ...props.filter, speed: DefaultSpeedFilterModel },
              FilterType.Speed
            )
          }
        />
      )}
      {props.filter.coordinates.enabled && (
        <Filter
          icon="fa-location-arrow"
          displayText={coordinatesFilterToString(props.filter.coordinates)}
          onClick={setShowCoordinatesFilter}
          order={getOrder(FilterType.Coordinates)}
          onClickDelete={(e) =>
            handleFilterDelete(
              e,
              { ...props.filter, coordinates: DefaultCoordinatesFilterModel },
              FilterType.Coordinates
            )
          }
        />
      )}
      {props.filter.altitude.enabled && (
        <Filter
          icon="fa-mountain"
          displayText={altitudeFilterToString(props.filter.altitude)}
          order={getOrder(FilterType.Altitude)}
          onClick={setShowAltitudeFilter}
          onClickDelete={(e) =>
            handleFilterDelete(
              e,
              { ...props.filter, altitude: DefaultAltitudeFilterModel },
              FilterType.Altitude
            )
          }
        />
      )}
      {showDateFilter && (
        <DateFilterModal
          closeModal={hideDateFilterModal}
          dateFilter={props.filter.date}
          setDateFilter={(date) => props.setFilter({ ...props.filter, date: date })}
        />
      )}
      {showCoordinatesFilter && (
        <CoordinatesFilterModal
          closeModal={hideCoordinatesFilter}
          filter={props.filter.coordinates}
          setFilter={(coordinates) =>
            handleAddFilter({ ...props.filter, coordinates: coordinates }, FilterType.Coordinates)
          }
        />
      )}
      {showSpeedFilter && (
        <SpeedFilterModal
          closeModal={hideSpeedFilter}
          filter={props.filter.speed}
          setFilter={(speed) =>
            handleAddFilter({ ...props.filter, speed: speed }, FilterType.Speed)
          }
        />
      )}
      {showAltitudeFilter && (
        <AltitudeFilterModal
          closeModal={hideAltitudeFilter}
          filter={props.filter.altitude}
          setFilter={(altitude) =>
            handleAddFilter({ ...props.filter, altitude: altitude }, FilterType.Altitude)
          }
        />
      )}
      {!(
        props.filter.speed.enabled &&
        props.filter.altitude.enabled &&
        props.filter.coordinates.enabled
      ) && (
        <div
          className="relative inline-block cursor-pointer order-last"
          onClick={(e) => setShowAddFilterMenu(e)}>
          <div className="hover:bg-gray-200 rounded rounded-lg ml-2 flex items-center px-2 py-1 text-sm">
            <Icon className="fa-plus mr-1" />
            <span>Add filter</span>
          </div>
          {showAddFilterMenu && (
            <div className="mt-2 absolute fadeIn animated faster text-sm">
              <div className="bg-white rounded-lg shadow overflow-hidden py-1">
                {!props.filter.date.enabled && (
                  <MenuItem
                    icon="fa-calendar-alt"
                    label="Date"
                    onClick={(e) => {
                      setShowDateFilter(e);
                      hideAddFilterMenu();
                    }}
                  />
                )}
                {/* {!props.filter.coordinates.enabled && (
                  <MenuItem
                    icon="fa-location-arrow"
                    label="Coordinates"
                    onClick={e => {
                      setShowCoordinatesFilter(e);
                      hideAddFilterMenu();
                    }}
                  />
                )} */}
                {!props.filter.speed.enabled && (
                  <MenuItem
                    icon="fa-tachometer-alt"
                    label="Speed"
                    onClick={(e) => {
                      setShowSpeedFilter(e);
                      hideAddFilterMenu();
                    }}
                  />
                )}
                {!props.filter.altitude.enabled && (
                  <MenuItem
                    icon="fa-mountain"
                    label="Altitude"
                    onClick={(e) => {
                      setShowAltitudeFilter(e);
                      hideAddFilterMenu();
                    }}
                  />
                )}
              </div>
            </div>
          )}
        </div>
      )}
    </div>
  );
}

function Filter(props: {
  icon: string;
  displayText: string;
  order: number;
  onClick: (event: React.MouseEvent<HTMLDivElement, MouseEvent>) => void;
  onClickDelete: (event: React.MouseEvent<HTMLDivElement, MouseEvent>) => void;
}) {
  let orderClassName = `order-${props.order}`;

  return (
    <div
      className={classNames(
        "ml-3 cursor-pointer focus:outline-none bg-gray-200 hover:bg-gray-300 text-sm px-2 py-1 rounded rounded-lg",
        orderClassName
      )}
      onClick={props.onClick}>
      <Icon className={props.icon} />
      <span className="ml-2">{props.displayText}</span>
      <span onClick={props.onClickDelete} className="hover:text-gray-700 ml-2">
        <Icon className="fa-times" />
      </span>
    </div>
  );
}

function MenuItem(props: {
  icon: string;
  label: string;
  onClick: (event: React.MouseEvent<HTMLDivElement, MouseEvent>) => void;
}) {
  return (
    <div
      className="block px-4 py-2 leading-tight hover:bg-gray-200 text-gray-600 hover:text-gray-900 flex items-center"
      onClick={props.onClick}>
      <Icon className={props.icon} />
      <span className="ml-2">{props.label}</span>
    </div>
  );
}
