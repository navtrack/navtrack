import React, { useState } from "react";
import Button from "../../shared/elements/Button";
import Modal from "../../shared/elements/Modal";
import Icon from "../../shared/util/Icon";
import TextInput from "../../shared/forms/TextInput";
import {
  DefaultDeleteAssetModel,
  DeleteAssetModel
} from "../../../apis/types/asset/DeleteAssetModel";
import { useNewValidation } from "../../../services/validation/useValidationHook";
import { AssetApi } from "../../../apis/AssetApi";
import { addNotification } from "../../shared/notifications/Notifications";
import { useHistory } from "react-router";
import { AssetModel } from "../../../apis/types/asset/AssetModel";
import { ValidateAction } from "../../../services/validation/ValidateAction";

type Props = {
  asset: AssetModel;
  closeModal: () => void;
};

export default function DeleteAssetModal(props: Props) {
  const [deleteAssetModel, setDeleteAssetModel] = useState(DefaultDeleteAssetModel);
  const history = useHistory();

  const deleteClickHandler = () => {
    if (validate(deleteAssetModel)) {
      AssetApi.delete(props.asset.id)
        .then(() => {
          addNotification("Device deleted successfully.");
          history.push(`/`);
        })
        .catch(setErrors);
    }
    props.closeModal();
    // props.deleteHandler();
  };

  const validateModel: ValidateAction<DeleteAssetModel> = (object, validationResult, intl) => {
    if (!object.name || object.name.length === 0 || object.name !== props.asset.name) {
      validationResult.AddError("name", "The name is not correct.");
    }
  };

  const [validate, validationResult, setErrors] = useNewValidation(validateModel);

  return (
    <Modal closeModal={props.closeModal}>
      <div className="max-w-sm">
        <div className="font-medium text-lg mb-4 flex">
          <div className="border-b pb-4 flex w-full">
            <div className="flex-grow">Delete confirmation</div>
            <div
              className="text-right flex items-center justify-end cursor-pointer"
              onClick={props.closeModal}>
              <Icon className="fa-times" />
            </div>
          </div>
        </div>
        <div className="mb-4 text-sm">Are you sure you want to delete this item?</div>
        <div className="mb-4 text-sm">
          This action cannot be undone. Deleting this asset will remove everything related to it,
          including all its history.
        </div>
        <div className="mb-4 text-sm">
          Please type <span className="font-semibold">{props.asset.name}</span> to confirm.
        </div>
        <TextInput
          value={deleteAssetModel.name}
          className="mb-3"
          onChange={(e) => setDeleteAssetModel({ name: e.target.value })}
          validationResult={validationResult.property.name}
        />
        <div className="border-t pt-4">
          <Button
            color="warn"
            className="w-full"
            onClick={deleteClickHandler}
            disabled={deleteAssetModel.name !== props.asset.name}>
            Delete
          </Button>
        </div>
      </div>
    </Modal>
  );
}
