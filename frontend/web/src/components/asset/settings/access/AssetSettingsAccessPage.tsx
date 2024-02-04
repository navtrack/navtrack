import { UsersTable } from "./UsersTable";
import { FormattedMessage } from "react-intl";
import { useState } from "react";
import { AddUserToAssetModal } from "./AddUserToAssetModal";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { useAssetUsersQuery } from "@navtrack/shared/hooks/queries/useAssetUsersQuery";
import { Card } from "../../../ui/card/Card";
import { CardBody } from "../../../ui/card/CardBody";
import { Heading } from "../../../ui/heading/Heading";
import { NewButton } from "../../../ui/button/NewButton";

export function AssetSettingsAccessPage() {
  const currentAsset = useCurrentAsset();
  const assetUsers = useAssetUsersQuery({
    assetId: currentAsset.data?.id ?? ""
  });

  const [showModal, setShowModal] = useState(false);

  return (
    <>
      {currentAsset && (
        <Card>
          <CardBody>
            <div className="mb-4 flex justify-between">
              <Heading type="h2">
                <FormattedMessage id="assets.settings.access.manage" />
              </Heading>
              <NewButton onClick={() => setShowModal(true)}>
                <FormattedMessage id="generic.add-user" />
              </NewButton>
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
          </CardBody>
        </Card>
      )}
    </>
  );
}
