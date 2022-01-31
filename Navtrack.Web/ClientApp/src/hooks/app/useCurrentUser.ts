import { useMemo } from "react";
import { UnitsType } from "@navtrack/navtrack-shared";
import useGetCurrentUserQuery from "../queries/useGetCurrentUserQuery";

type CurrentUser = {
  id: string;
  email: string;
  units: UnitsType;
};

export default function useCurrentUser() {
  const query = useGetCurrentUserQuery();

  const user: CurrentUser | undefined = useMemo(() => {
    if (query.data) {
      return {
        id: query.data.id,
        email: query.data.email,
        units: query.data.units === 1 ? UnitsType.Imperial : UnitsType.Metric
      };
    }
  }, [query.data]);

  return user;
}
