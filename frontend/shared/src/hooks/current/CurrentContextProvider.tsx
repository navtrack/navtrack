import { ReactNode, useMemo } from "react";
import { useMatch } from "react-router-dom";
import { useAssetQuery } from "../queries/assets/useAssetQuery";
import { useTeamQuery } from "../queries/teams/useTeamQuery";
import { CurrentContext } from "./CurrentContext";

type CurrentContextProviderProps = {
  children?: ReactNode;
};

export function CurrentContextProvider(props: CurrentContextProviderProps) {
  const assetMatch = useMatch("/assets/:id/*");
  const organizationMatch = useMatch("/organizations/:id/*");
  const teamMatch = useMatch("/teams/:id/*");

  const assetQuery = useAssetQuery({ assetId: assetMatch?.params.id });
  const teamQuery = useTeamQuery({ teamId: teamMatch?.params.id });

  const organizationId = useMemo(() => {
    if (organizationMatch?.params.id) {
      return organizationMatch.params.id;
    } else if (assetMatch?.params.id) {
      if (assetQuery.isSuccess) {
        return assetQuery.data?.organizationId;
      }
    } else if (teamMatch?.params.id) {
      if (teamQuery.isSuccess) {
        return teamQuery.data?.organizationId;
      }
    }
    return undefined;
  }, [
    assetMatch?.params.id,
    assetQuery.data?.organizationId,
    assetQuery.isSuccess,
    organizationMatch?.params.id,
    teamMatch?.params.id,
    teamQuery.data?.organizationId,
    teamQuery.isSuccess
  ]);

  return (
    <CurrentContext.Provider
      value={{
        organizationId: organizationId,
        assetId: assetMatch?.params.id,
        teamId: teamMatch?.params.id
      }}>
      {props.children}
    </CurrentContext.Provider>
  );
}
