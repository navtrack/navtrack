import {
  faMapMarkedAlt,
  faMountain,
  faTachometerAlt
} from "@fortawesome/free-solid-svg-icons";
import {
  Menu,
  MenuButton,
  MenuItem,
  MenuItems,
  Transition
} from "@headlessui/react";
import { Fragment, useCallback, useState } from "react";
import { FormattedMessage } from "react-intl";
import { useAtom, useAtomValue } from "jotai";
import {
  altitudeFilterAtom,
  geofenceFilterAtom,
  filtersEnabledSelector,
  speedFilterAtom,
  durationFilterAtom
} from "./locationFilterState";
import { LocationFilterAddButtonMenuItem } from "./LocationFilterAddButtonMenuItem";
import { faClock } from "@fortawesome/free-regular-svg-icons";
import { Button } from "../../../ui/button/Button";
import { ZINDEX_MENU } from "../../../../constants";

type LocationFilterAddButtonProps = {
  duration?: boolean;
  altitude?: boolean;
  avgSpeed?: boolean;
  filterKey: string;
};

export function LocationFilterAddButton(props: LocationFilterAddButtonProps) {
  const filtersEnabled = useAtomValue(filtersEnabledSelector(props.filterKey));
  const [altitudeFilter, setAltitudeFilter] = useAtom(
    altitudeFilterAtom(props.filterKey)
  );
  const [durationFilter, setDurationFilter] = useAtom(
    durationFilterAtom(props.filterKey)
  );
  const [speedFilter, setSpeedFilter] = useAtom(
    speedFilterAtom(props.filterKey)
  );
  const [geofenceFilter, setGeofenceFilter] = useAtom(
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
          <MenuButton as={Fragment}>
            <div>
              <Button color="secondary" size="xs">
                <FormattedMessage id="locations.filter.add" />
              </Button>
            </div>
          </MenuButton>
          <Transition
            as={Fragment}
            enter="transition ease-out duration-100"
            enterFrom="transform opacity-0 scale-95"
            enterTo="transform opacity-100 scale-100"
            leave="transition ease-in duration-75"
            leaveFrom="transform opacity-100 scale-100"
            leaveTo="transform opacity-0 scale-95">
            <MenuItems
              className="absolute left-0 mt-2 w-44 origin-top-left rounded-md bg-white shadow-lg ring-1 ring-black/5 focus:outline-none"
              style={{ zIndex: ZINDEX_MENU }}>
              <div className="py-1">
                {!altitudeFilter.enabled && (
                  <MenuItem>
                    <LocationFilterAddButtonMenuItem
                      icon={faMountain}
                      labelId="locations.filter.altitude"
                      onClick={() =>
                        setAltitudeFilter((x) => ({
                          ...x,
                          open: true,
                          order: getOrder()
                        }))
                      }
                    />
                  </MenuItem>
                )}
                {props.duration && !durationFilter.enabled && (
                  <MenuItem>
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
                  </MenuItem>
                )}
                {!geofenceFilter.enabled && (
                  <MenuItem>
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
                  </MenuItem>
                )}
                {!speedFilter.enabled && (
                  <MenuItem>
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
                  </MenuItem>
                )}
              </div>
            </MenuItems>
          </Transition>
        </Menu>
      )}
    </>
  );
}
