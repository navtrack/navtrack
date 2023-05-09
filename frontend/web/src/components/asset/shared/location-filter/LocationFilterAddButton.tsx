import {
  faMapMarkedAlt,
  faMountain,
  faTachometerAlt
} from "@fortawesome/free-solid-svg-icons";
import { Menu, Transition } from "@headlessui/react";
import { Fragment, useCallback, useState } from "react";
import { Button } from "../../../ui/shared/button/Button";
import { FormattedMessage } from "react-intl";
import { useRecoilState, useRecoilValue } from "recoil";
import {
  altitudeFilterAtom,
  geofenceFilterAtom,
  filtersEnabledSelector,
  speedFilterAtom,
  durationFilterAtom
} from "./state";
import { LocationFilterAddButtonMenuItem } from "./LocationFilterAddButtonMenuItem";
import { faClock } from "@fortawesome/free-regular-svg-icons";

interface ILocationFilterAddButton {
  duration?: boolean;
  avgAltitude?: boolean;
  avgSpeed?: boolean;
  filterKey: string;
}

export function LocationFilterAddButton(props: ILocationFilterAddButton) {
  const filtersEnabled = useRecoilValue(
    filtersEnabledSelector(props.filterKey)
  );
  const [altitudeFilter, setAltitudeFilter] = useRecoilState(
    altitudeFilterAtom(props.filterKey)
  );
  const [durationFilter, setDurationFilter] = useRecoilState(
    durationFilterAtom(props.filterKey)
  );
  const [speedFilter, setSpeedFilter] = useRecoilState(
    speedFilterAtom(props.filterKey)
  );
  const [geofenceFilter, setGeofenceFilter] = useRecoilState(
    geofenceFilterAtom(props.filterKey)
  );
  const [filterCount, setFilterCount] = useState(1);

  const getOrder = useCallback(() => {
    setFilterCount((x) => ++x);
    return filterCount;
  }, [filterCount]);

  return (
    <>
      {!filtersEnabled.all && (
        <Menu as="div" className="relative order-last inline-block text-left">
          <Menu.Button as={Fragment}>
            <div>
              <Button color="primary" size="xs">
                <FormattedMessage id="locations.filter.add" />
              </Button>
            </div>
          </Menu.Button>
          <Transition
            as={Fragment}
            enter="transition ease-out duration-100"
            enterFrom="transform opacity-0 scale-95"
            enterTo="transform opacity-100 scale-100"
            leave="transition ease-in duration-75"
            leaveFrom="transform opacity-100 scale-100"
            leaveTo="transform opacity-0 scale-95">
            <Menu.Items className="absolute left-0 z-30 mt-2 w-44 origin-top-left rounded-md bg-white shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
              <div className="py-1">
                {!altitudeFilter.enabled && (
                  <Menu.Item>
                    <LocationFilterAddButtonMenuItem
                      icon={faMountain}
                      labelId={
                        props.avgAltitude
                          ? "locations.filter.avg-altitude"
                          : "locations.filter.altitude"
                      }
                      onClick={() =>
                        setAltitudeFilter((x) => ({
                          ...x,
                          open: true,
                          order: getOrder()
                        }))
                      }
                    />
                  </Menu.Item>
                )}
                {props.duration && !durationFilter.enabled && (
                  <Menu.Item>
                    <LocationFilterAddButtonMenuItem
                      icon={faClock}
                      labelId="locations.filter.duration"
                      onClick={() =>
                        setDurationFilter((x) => ({
                          ...x,
                          open: true,
                          order: getOrder()
                        }))
                      }
                    />
                  </Menu.Item>
                )}
                {!geofenceFilter.enabled && (
                  <Menu.Item>
                    <LocationFilterAddButtonMenuItem
                      icon={faMapMarkedAlt}
                      labelId="locations.filter.geofence"
                      onClick={() =>
                        setGeofenceFilter((x) => ({
                          ...x,
                          open: true,
                          order: getOrder()
                        }))
                      }
                    />
                  </Menu.Item>
                )}
                {!speedFilter.enabled && (
                  <Menu.Item>
                    <LocationFilterAddButtonMenuItem
                      icon={faTachometerAlt}
                      labelId={
                        props.avgSpeed
                          ? "locations.filter.avg-speed"
                          : "locations.filter.speed"
                      }
                      onClick={() =>
                        setSpeedFilter((x) => ({
                          ...x,
                          open: true,
                          order: getOrder()
                        }))
                      }
                    />
                  </Menu.Item>
                )}
              </div>
            </Menu.Items>
          </Transition>
        </Menu>
      )}
    </>
  );
}
