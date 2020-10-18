import React, { useState } from "react";
import { useIntl } from "react-intl";
import { AssetApi } from "../../../apis/AssetApi";
import { AssetModel } from "../../../apis/types/asset/AssetModel";
import { RenameAssetRequestModel } from "../../../apis/types/asset/RenameAssetRequestModel";
import { useNewValidation } from "../../../services/validation/useValidationHook";
import { ValidateAction } from "../../../services/validation/ValidateAction";
import Button from "../../shared/elements/Button";
import TextInput from "../../shared/forms/TextInput";
import { addNotification } from "../../shared/notifications/Notifications";
import AssetSettingsCard from "./AssetSettingsCard";
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
          addNotification("Device renamed successfully.");
          props.refreshAsset();
        })
        .catch(setErrors);
    }
  };

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
                disabled={validationResult.HasErrors()}>
                Rename
              </Button>
            </TextInput>
          </div>
        </div>
      </AssetSettingsCard>
      <AssetSettingsCard title="Delete">
        <span className="text-sm">
          Once you delete an asset, everything related to it will be removed including all its
          history.
        </span>
        <div className="text-right w-full">
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

const validateAsset: ValidateAction<RenameAssetRequestModel> = (object, validationResult, intl) => {
  if (!object.name || object.name.length === 0) {
    validationResult.AddError("name", intl.formatMessage({ id: "assets.add.name.required" }));
  }
};
