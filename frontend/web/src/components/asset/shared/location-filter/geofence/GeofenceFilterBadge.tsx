import { faMapMarkedAlt } from "@fortawesome/free-solid-svg-icons";
import { useRecoilState, useResetRecoilState } from "recoil";
import { FilterBadge } from "../FilterBadge";
import { IconWithText } from "../../../../ui/icon/IconWithText";
import { geofenceFilterAtom } from "../locationFilterState";

type GeofenceFilterBadgeProps = {
  filterKey: string;
};

export function GeofenceFilterBadge(props: GeofenceFilterBadgeProps) {
  const [state, setState] = useRecoilState(geofenceFilterAtom(props.filterKey));
  const reset = useResetRecoilState(geofenceFilterAtom(props.filterKey));

  return (
    <>
      {state.enabled && (
        <FilterBadge
          order={state.order}
          onClick={() => setState((x) => ({ ...x, open: true }))}
          onCloseClick={reset}>
          <IconWithText icon={faMapMarkedAlt}>
            {state.geofence?.latitude}, {state.geofence?.longitude} -{" "}
            {state.geofence?.radius}m radius
          </IconWithText>
        </FilterBadge>
      )}
    </>
  );
}
