import React, { useCallback, useState } from "react";
import Button from "components/library/elements/Button";
import { useIntl } from "react-intl";
import TextInput from "components/library/forms/TextInput";
import { useNewValidation } from "framework/validation/useValidationHook";
import { Validator } from "framework/validation/Validator";
import { AssetApi } from "apis/AssetApi";
import { addNotification } from "components/library/notifications/Notifications";
import AssetSettingsCard from "./AssetSettingsCard";
import { RenameAssetRequestModel } from "apis/types/asset/RenameAssetRequestModel";
import { AssetModel } from "apis/types/asset/AssetModel";
import DeleteAssetModal from "./DeleteAssetModal";

type Props = {
  asset: AssetModel;
  refreshAsset: () => void;
};

export default function AssetSettingsGeneral(props: Props) {
  const [renameModel, setRenameAsset] = useState<RenameAssetRequestModel>({
    name: props.asset.name
  });
  const [validate, validationResult, setErrors] = useNewValidation(validateAsset);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const intl = useIntl();

  const handleRenameClick = async () => {
    if (validate(renameModel)) {
      AssetApi.rename(props.asset.id, renameModel)
        .then(() => {
          // history.push(`/assets/${response.id}/live`);
          addNotification("Device renamed successfully.");
          props.refreshAsset();
        })
        .catch(setErrors);
    }
  };

  const handleDeleteClick = useCallback(() => {
    setShowDeleteModal(true);
    // setShowDeleteModal(true);
    // setHandleDelete(() => () => deleteAsset(id, assets));
  }, []);

  return (
    <>
      <AssetSettingsCard title="General">
        <div className="w-1/2 flex">
          <div className="flex-grow">
            <TextInput
              name={intl.formatMessage({ id: "assets.add.name" })}
              value={renameModel.name}
              validationResult={validationResult.property.name}
              className="mb-3"
              onChange={(e) => {
                setRenameAsset({ ...renameModel, name: e.target.value });
              }}>
              <Button
                color="basic"
                onClick={handleRenameClick}
                disabled={validationResult.HasErrors()}
                className="ml-2">
                Rename
              </Button>
            </TextInput>
          </div>
        </div>
      </AssetSettingsCard>
      <AssetSettingsCard title="Delete">
        <div>
          <h1 className="text-sm font-semibold">Delete this asset</h1>
          <span className="text-sm">
            Once you delete an asset, everything related to it will be removed including all its
            history.
          </span>
        </div>
        <div className="text-right flex-grow">
          <Button color="warn" onClick={() => setShowDeleteModal(true)}>
            Delete asset
          </Button>
        </div>
      </AssetSettingsCard>
      {showDeleteModal && (
        <DeleteAssetModal asset={props.asset} closeModal={() => setShowDeleteModal(false)} />
      )}
    </>
  );
}

const validateAsset: Validator<RenameAssetRequestModel> = (object, validationResult, intl) => {
  if (!object.name || object.name.length === 0) {
    validationResult.AddError("name", intl.formatMessage({ id: "assets.add.name.required" }));
  }
};
