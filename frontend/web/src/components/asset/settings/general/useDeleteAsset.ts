import { useCallback } from "react";
import { useIntl } from "react-intl";
import { useHistory } from "react-router";
import { object, ObjectSchema, string } from "yup";
import { DeleteAssetFormValues } from "./types";
import useNotification from "../../../ui/shared/notification/useNotification";
import { useCurrentAsset } from "@navtrack/shared/newHooks/assets/useCurrentAsset";
import { useDeleteAssetMutation } from "@navtrack/shared/hooks/mutations/useDeleteAssetMutation";
import { useGetAssetsSignalRQuery } from "@navtrack/shared/hooks/queries/useGetAssetsSignalRQuery";

export default function useDeleteAsset() {
  const currentAsset = useCurrentAsset();
  const intl = useIntl();
  const deleteAssetMutation = useDeleteAssetMutation();
  const history = useHistory();
  const { showNotification } = useNotification();
  const assetsQuery = useGetAssetsSignalRQuery();

  const validationSchema: ObjectSchema<DeleteAssetFormValues> = object()
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
