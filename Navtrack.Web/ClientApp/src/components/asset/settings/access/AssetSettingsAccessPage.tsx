import AssetSettingsLayout from "../layout/AssetSettingsLayout";
import useCurrentAsset from "../../../../hooks/assets/useCurrentAsset";
import useAssetUsersQuery from "../../../../hooks/queries/useAssetUsersQuery";
import UsersTable from "./UsersTable";
import { FormattedMessage } from "react-intl";
import Button from "../../../ui/shared/button/Button";
import { useState } from "react";
import AddUserToAssetModal from "./AddUserToAssetModal";

export default function AssetSettingsAccessPage() {
  const { currentAsset } = useCurrentAsset();
  const assetUsers = useAssetUsersQuery({
    assetId: !!currentAsset ? currentAsset.id : ""
  });

  const [showModal, setShowModal] = useState(false);

  return (
    <>
      {currentAsset && (
        <AssetSettingsLayout>
          <div className="flex justify-between pb-4 items-center">
            <h2 className="text-lg leading-6 font-medium text-gray-900">
              <FormattedMessage id="assets.settings.access.manage" />
            </h2>
            <Button color="green" onClick={() => setShowModal(true)}>
              Add user
            </Button>
          </div>
          <UsersTable
            rows={assetUsers.data?.items}
            loading={assetUsers.isLoading}
            refresh={assetUsers.refetch}
          />
          <AddUserToAssetModal
            show={showModal}
            close={() => setShowModal(false)}
          />
        </AssetSettingsLayout>
      )}
    </>
  );
}
