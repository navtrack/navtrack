import { useMemo } from "react";
import { useCurrentUserQuery } from "../queries/useCurrentUserQuery";
import { UnitsType } from "../../api/model/generated";

type CurrentUser = {
  id: string;
  email: string;
  units: UnitsType;
};

export function useCurrentUser() {
  const query = useCurrentUserQuery();

  const user: CurrentUser | undefined = useMemo(() => {
    if (query.data) {
      return {
        id: query.data.id,
        email: query.data.email,
        units: query.data.units
      };
    }
  }, [query.data]);

  return user;
}
