import { faMapMarkedAlt } from "@fortawesome/free-solid-svg-icons";
import { useRecoilState, useResetRecoilState } from "recoil";
import { FilterBadge } from "../FilterBadge";
import { IconWithText } from "../../../../ui/icon/IconWithText";
import { geofenceFilterAtom } from "../locationFilterState";
import { useDistance } from "@navtrack/shared/hooks/util/useDistance";
import { FormattedMessage } from "react-intl";
import { showCoordinate } from "@navtrack/shared/utils/coordinates";

type GeofenceFilterBadgeProps = {
  filterKey: string;
};

export function GeofenceFilterBadge(props: GeofenceFilterBadgeProps) {
  const [state, setState] = useRecoilState(geofenceFilterAtom(props.filterKey));
  const reset = useResetRecoilState(geofenceFilterAtom(props.filterKey));
  const distance = useDistance();

  return (
    <>
      {state.enabled && (
        <FilterBadge
          order={state.order}
          onClick={() => setState((x) => ({ ...x, open: true }))}
          onCloseClick={reset}>
          <IconWithText icon={faMapMarkedAlt}>
            <FormattedMessage
              id="locations.filter.geofence.badge"
              values={{
                latitude: showCoordinate(state.geofence?.latitude),
                longitude: showCoordinate(state.geofence?.longitude),
                distance: distance.showDistance(state.geofence?.radius)
              }}
            />
          </IconWithText>
        </FilterBadge>
      )}
    </>
  );
}
