import { faMapMarkedAlt } from "@fortawesome/free-solid-svg-icons";
import { useRecoilState, useResetRecoilState } from "recoil";
import Badge from "../../../../ui/shared/badge/Badge";
import IconWithText from "../../../../ui/shared/icon/IconWithText";
import { geofenceFilterAtom } from "../state";

interface IGeofenceFilterBadge {
  filterKey: string;
}

export default function GeofenceFilterBadge(props: IGeofenceFilterBadge) {
  const [state, setState] = useRecoilState(geofenceFilterAtom(props.filterKey));
  const reset = useResetRecoilState(geofenceFilterAtom(props.filterKey));

  return (
    <>
      {state.enabled && (
        <Badge
          order={state.order}
          onClick={() => setState((x) => ({ ...x, open: true }))}
          onCloseClick={reset}>
          <IconWithText icon={faMapMarkedAlt}>
            {state.geofence?.latitude}, {state.geofence?.longitude} -{" "}
            {state.geofence?.radius}m radius
          </IconWithText>
        </Badge>
      )}
    </>
  );
}
