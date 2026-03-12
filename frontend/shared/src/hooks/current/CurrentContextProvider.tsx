import { createContext, ReactNode, useEffect, useState } from "react";
import { useMatch } from "react-router-dom";
import { useAssetQuery } from "../queries/assets/useAssetQuery";

type CurrentIdRouteProviderProps = {
  children?: ReactNode;
};

export type CurrentIds = {
  assetId?: string;
  organizationId?: string;
  teamId?: string;
};

export const CurrentContext = createContext<CurrentIds>({});

export function CurrentContextProvider(props: CurrentIdRouteProviderProps) {
  const assetMatch = useMatch("/assets/:id/*");
  const organizationMatch = useMatch("/organizations/:id/*");
  const teamMatch = useMatch("/teams/:id/*");

  const assetQuery = useAssetQuery({ assetId: assetMatch?.params.id });
  const teamQuery = useAssetQuery({ assetId: teamMatch?.params.id });

  const [organizationId, setOrganizationId] = useState<string | undefined>();

  useEffect(() => {
    if (organizationMatch?.params.id) {
      setOrganizationId(organizationMatch.params.id);
    } else if (assetMatch?.params.id) {
      if (assetQuery.isSuccess) {
        setOrganizationId(assetQuery.data?.organizationId);
      }
    } else if (teamMatch?.params.id) {
      if (teamQuery.isSuccess) {
        setOrganizationId(teamQuery.data?.organizationId);
      }
    } else {
      setOrganizationId(undefined);
    }
  }, [
    assetMatch?.params.id,
    assetQuery.data?.organizationId,
    assetQuery.isLoading,
    assetQuery.isSuccess,
    organizationMatch?.params.id,
    teamMatch?.params.id,
    teamQuery.data?.organizationId,
    teamQuery.isLoading,
    teamQuery.isSuccess
  ]);

  return (
    <CurrentContext.Provider
      value={{
        assetId: assetMatch?.params.id,
        organizationId: organizationId,
        teamId: teamMatch?.params.id
      }}>
      {props.children}
    </CurrentContext.Provider>
  );
}
