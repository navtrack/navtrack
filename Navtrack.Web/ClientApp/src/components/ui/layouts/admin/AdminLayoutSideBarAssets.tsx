import AdminLayoutSideBarItem from "./AdminLayoutSideBarItem";
import useAssets from "../../../../hooks/assets/useAssets";
import { FormattedMessage } from "react-intl";
import LoadingIndicator from "../../shared/loading-indicator/LoadingIndicator";

export default function AdminLayoutSideBarAssets() {
  const assets = useAssets();

  return (
    <div className="flex flex-1 flex-col px-2 overflow-y-scroll space-y-1">
      {assets.isLoading ? (
        <LoadingIndicator className="text-gray-300 mt-2" size="lg" />
      ) : (
        <>
          {assets.data?.length ? (
            assets.data?.map((asset) => (
              <AdminLayoutSideBarItem key={asset.id} asset={asset} />
            ))
          ) : (
            <div className="text-white text-center text-sm">
              <FormattedMessage id="sidebar.no-assets" />
            </div>
          )}
        </>
      )}
    </div>
  );
}
