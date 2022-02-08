import AdminLayoutSideBarItem from "./AdminLayoutSideBarItem";
import { FormattedMessage } from "react-intl";
import LoadingIndicator from "../../shared/loading-indicator/LoadingIndicator";
import {
  useAssets,
  useGetAssetsSignalRQuery
} from "@navtrack/navtrack-app-shared";

export default function AdminLayoutSideBarAssets() {
  const assetsQuery = useGetAssetsSignalRQuery();
  const assets = useAssets();

  return (
    <div className="flex flex-1 flex-col space-y-1 overflow-y-scroll px-2">
      {assetsQuery.isLoading ? (
        <LoadingIndicator className="mt-2 text-gray-300" size="lg" />
      ) : (
        <>
          {assets.length ? (
            assets.map((asset) => (
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
