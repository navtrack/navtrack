import AdminLayoutSideBarItem from "./AdminLayoutSideBarItem";
import { FormattedMessage } from "react-intl";
import LoadingIndicator from "../../shared/loading-indicator/LoadingIndicator";
import { useGetAssetsSignalRQuery } from "@navtrack/ui-shared/hooks/queries/useGetAssetsSignalRQuery";

export default function AdminLayoutSideBarAssets() {
  const assetsQuery = useGetAssetsSignalRQuery();

  return (
    <div className="flex flex-1 flex-col space-y-1 overflow-y-scroll px-2">
      {assetsQuery.isLoading ? (
        <LoadingIndicator className="mt-2 text-gray-300" size="lg" />
      ) : (
        <>
          {assetsQuery.data?.items.length ? (
            assetsQuery.data?.items.map((asset) => (
              <AdminLayoutSideBarItem key={asset.id} asset={asset} />
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
