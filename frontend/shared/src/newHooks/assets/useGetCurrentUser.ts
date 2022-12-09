import { useGetCurrentUserQuery } from "../../hooks/queries/useGetCurrentUserQuery";

export const useGetCurrentUser = () => {
  const query = useGetCurrentUserQuery();

  return query.data;
};
