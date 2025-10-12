import { getTeamsAssetsListQueryKey, useTeamsAssetsList } from "../../../api";

type UseTeamsQueryProps = {
  teamId?: string;
};

export const useTeamAssetsQuery = (props: UseTeamsQueryProps) => {
  const query = useTeamsAssetsList(props?.teamId!, {
    query: {
      queryKey: getTeamsAssetsListQueryKey(`${props?.teamId}`),
      refetchOnWindowFocus: false,
      enabled: !!props?.teamId
    }
  });

  return query;
};
