import { useAssetsGetList } from "../../../api/index-generated";

type UseAssetsQueryProps = {
  organizationId?: string;
};

export function useAssetsQuery(props: UseAssetsQueryProps) {
  const query = useAssetsGetList(props.organizationId!, {
    query: {
      refetchIntervalInBackground: true,
      refetchInterval: 5000,
      enabled: props.organizationId !== undefined
    }
  });

  return query;
}
