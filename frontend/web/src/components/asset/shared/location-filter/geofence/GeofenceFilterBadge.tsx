import { faMapMarkedAlt } from "@fortawesome/free-solid-svg-icons";
import { useAtom } from "jotai";
import { useResetAtom } from "jotai/utils";
import { FilterBadge } from "../FilterBadge";
import { IconWithText } from "../../../../ui/icon/IconWithText";
import { geofenceFilterAtom } from "../locationFilterState";
import { FormattedMessage } from "react-intl";
import { showCoordinate } from "@navtrack/shared/utils/coordinates";
import { useShow } from "@navtrack/shared/hooks/util/useShow";

type GeofenceFilterBadgeProps = {
  filterKey: string;
};

export function GeofenceFilterBadge(props: GeofenceFilterBadgeProps) {
  const [state, setState] = useAtom(geofenceFilterAtom(props.filterKey));
  const reset = useResetAtom(geofenceFilterAtom(props.filterKey));
  const show = useShow();

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
                distance: show.distance(state.geofence?.radius)
              }}
            />
          </IconWithText>
        </FilterBadge>
      )}
    </>
  );
}
