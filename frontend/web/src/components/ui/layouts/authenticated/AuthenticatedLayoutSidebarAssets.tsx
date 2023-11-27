import { AuthenticatedLayoutSidebarItem } from "./AuthenticatedLayoutSidebarItem";
import { FormattedMessage } from "react-intl";
import { LoadingIndicator } from "../../loading-indicator/LoadingIndicator";
import { useGetAssetsQuery } from "@navtrack/shared/hooks/queries/useGetAssetsQuery";

export function AuthenticatedLayoutSidebarAssets() {
  const assetsQuery = useGetAssetsQuery();

  return (
    <div className="flex flex-1 flex-col space-y-1 px-2">
      {assetsQuery.isLoading ? (
        <LoadingIndicator className="mt-2 text-gray-300" size="lg" />
      ) : (
        <>
          {assetsQuery.data?.items.length ? (
            assetsQuery.data?.items.map((asset) => (
              <AuthenticatedLayoutSidebarItem key={asset.id} asset={asset} />
            ))
          ) : (
            <div className="text-center text-sm text-white">
              <FormattedMessage id="sidebar.no-assets" />
            </div>
          )}
        </>
      )}
    </div>
  );
}
