import { AssetUserModel } from "@navtrack/shared/api/model/generated";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { getError } from "@navtrack/shared/utils/api";
import { FormattedMessage } from "react-intl";
import { DeleteModal } from "../../../ui/modal/DeleteModal";
import { useNotification } from "../../../ui/notification/useNotification";
import { useAssetUserDeleteMutation } from "@navtrack/shared/hooks/mutations/assets/useAssetUserDeleteMutation";

type DeleteAssetModalProps = {
  user?: AssetUserModel;
  show: boolean;
  close: () => void;
  refresh: () => void;
};

export function DeleteUserFromAssetModal(props: DeleteAssetModalProps) {
  const currentAsset = useCurrentAsset();
  const mutation = useAssetUserDeleteMutation();
  const { showNotification } = useNotification();

  return (
    <DeleteModal
      open={props.show}
      close={props.close}
      onConfirm={() => {
        if (currentAsset.data && props.user) {
          mutation.mutate(
            { assetId: currentAsset.data?.id, userId: props.user.userId },
            {
              onSuccess: () => {
                props.refresh();
              },
              onError: (error) => {
                const model = getError(error);
                showNotification({
                  type: "error",
                  description: `${model.message}`
                });
              }
            }
          );
          props.close();
        }
      }}>
      <h3 className="text-lg font-medium leading-6 text-gray-900">
        <FormattedMessage id="shared.delete-modal.title" />
      </h3>
      <p className="mt-2 text-sm">
        <FormattedMessage
          id="assets.settings.access.delete-user.question"
          values={{
            email: <span className="font-bold">{props.user?.email}</span>,
            assetName: (
              <span className="font-bold">{currentAsset.data?.name}</span>
            )
          }}
        />
      </p>
    </DeleteModal>
  );
}
