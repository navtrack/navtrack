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
import { Fragment, useCallback, useMemo, useState } from "react";
import { FormattedMessage } from "react-intl";
import { useAtomValue, useSetAtom } from "jotai";
import {
  locationFilterOpenSelector,
  locationFiltersActiveListSelector
} from "./locationFilterState";
import { LocationFilterAddButtonMenuItem } from "./LocationFilterAddButtonMenuItem";
import { faClock } from "@fortawesome/free-regular-svg-icons";
import { Button } from "../../../ui/button/Button";
import { ZINDEX_MENU } from "../../../../constants";
import {
  LocationFilterConfiguration,
  LocationFilterType
} from "./locationFilterTypes";

const filters = [
  {
    icon: faMountain,
    labelId: "locations.filter.altitude",
    type: LocationFilterType.Altitude
  },
  {
    icon: faClock,
    labelId: "locations.filter.duration",
    type: LocationFilterType.Duration
  },
  {
    icon: faMapMarkedAlt,
    labelId: "locations.filter.geofence",
    type: LocationFilterType.Geofence
  },
  {
    icon: faTachometerAlt,
    labelId: "locations.filter.speed",
    type: LocationFilterType.Speed
  },
  {
    icon: faTachometerAlt,
    labelId: "locations.filter.avg-speed",
    type: LocationFilterType.AvgSpeed
  }
];

type LocationFilterAddButtonProps = {
  configuration: LocationFilterConfiguration;
};

export function LocationFilterAddButton(props: LocationFilterAddButtonProps) {
  const activeFilters = useAtomValue(
    locationFiltersActiveListSelector(props.configuration.filterKey)
  );

  const [filterCount, setFilterCount] = useState(1);

  const getOrder = useCallback(() => {
    setFilterCount((x) => ++x);

    return filterCount;
  }, [filterCount]);

  const openFilter = useSetAtom(
    locationFilterOpenSelector(props.configuration.filterKey)
  );

  const filterMenuItems = useMemo(() => {
    return filters.filter(
      (x) =>
        props.configuration.filters.includes(x.type) &&
        !activeFilters.includes(x.type)
    );
  }, [activeFilters, props.configuration.filters]);

  return (
    <>
      {filterMenuItems.length > 0 && (
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
                {filterMenuItems.map((item) => (
                  <MenuItem>
                    <LocationFilterAddButtonMenuItem
                      icon={item.icon}
                      labelId={item.labelId}
                      onClick={() => {
                        const order = getOrder();

                        openFilter({ type: item.type, order });
                      }}
                    />
                  </MenuItem>
                ))}
              </div>
            </MenuItems>
          </Transition>
        </Menu>
      )}
    </>
  );
}
