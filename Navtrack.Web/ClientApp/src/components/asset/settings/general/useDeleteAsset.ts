import { useCallback } from "react";
import { useIntl } from "react-intl";
import { useHistory } from "react-router";
import { object, SchemaOf, string } from "yup";
import { useDeleteAssetMutation } from "../../../../hooks/mutations/useDeleteAssetMutation";
import useGetAssetsSignalRQuery from "../../../../hooks/queries/useGetAssetsSignalRQuery";
import useCurrentAsset from "../../../../hooks/assets/useCurrentAsset";
import { DeleteAssetFormValues } from "./types";
import useNotification from "../../../ui/shared/notification/useNotification";

export default function useDeleteAsset() {
  const { currentAsset } = useCurrentAsset();
  const intl = useIntl();
  const deleteAssetMutation = useDeleteAssetMutation();
  const history = useHistory();
  const { showNotification } = useNotification();
  const assetsQuery = useGetAssetsSignalRQuery();

  const validationSchema: SchemaOf<DeleteAssetFormValues> = object()
    .shape({
      name: string()
        .required(
          intl.formatMessage({
            id: "generic.name.required"
          })
        )
        .equals(
          [`${currentAsset?.name}`],
          intl.formatMessage({
            id: "assets.settings.general.delete-asset.name-match"
          })
        )
    })
    .defined();

  const handleSubmit = useCallback(() => {
    if (currentAsset) {
      deleteAssetMutation.mutate(
        { assetId: currentAsset.id },
        {
          onSuccess: () => {
            assetsQuery.refetch();
            history.push("/");
            showNotification({
              type: "success",
              description: intl.formatMessage({
                id: "assets.settings.general.delete-asset.success"
              })
            });
          }
        }
      );
    }
  }, [
    assetsQuery,
    currentAsset,
    deleteAssetMutation,
    history,
    intl,
    showNotification
  ]);

  return {
    validationSchema,
    handleSubmit,
    loading: deleteAssetMutation.isLoading
  };
}
