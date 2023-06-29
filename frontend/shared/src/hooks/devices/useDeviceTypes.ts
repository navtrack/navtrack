import { useCallback } from "react";
import { useDevicesTypesQuery } from "../queries/useDevicesTypesQuery";

export function useDeviceTypes() {
  const deviceTypes = useDevicesTypesQuery();

  const getDeviceType = useCallback(
    (id?: string) => {
      return (deviceTypes.data?.items ?? []).find((x) => x.id === id)
        ?.displayName;
    },
    [deviceTypes.data?.items]
  );

  return { deviceTypes: deviceTypes.data?.items ?? [], getDeviceType };
}
