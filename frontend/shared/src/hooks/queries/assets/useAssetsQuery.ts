import { useAssetsGetList } from "../../../api";

export type AssetsQueryProps = {
  organizationId?: string | null;
};

export function useAssetsQuery(props: AssetsQueryProps) {
  const query = useAssetsGetList(props.organizationId!, {
    query: {
      refetchIntervalInBackground: true,
      refetchInterval: 5000,
      enabled: !!props.organizationId
    }
  });

  return query;
}
