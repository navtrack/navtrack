import { useTeamsAssetsList } from "../../../api";

type UseTeamsQueryProps = {
  teamId?: string;
};

export const useTeamAssetsQuery = (props: UseTeamsQueryProps) => {
  const query = useTeamsAssetsList(props?.teamId!, {
    query: {
      refetchOnWindowFocus: false,
      enabled: !!props?.teamId
    }
  });

  return query;
};
