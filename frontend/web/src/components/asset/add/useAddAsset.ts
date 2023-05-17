import { useCallback } from "react";
import { useIntl } from "react-intl";
import { object, ObjectSchema, string } from "yup";
import { AddAssetFormValues } from "./AddAssetFormValues";
import { FormikHelpers } from "formik";
import { useHistory } from "react-router-dom";
import { useScrollToAsset } from "../../ui/layouts/admin/useScrollToAsset";
import { useNotification } from "../../ui/shared/notification/useNotification";
import { useAddAssetMutation } from "@navtrack/shared/hooks/mutations/useAddAssetMutation";
import { useDevicesTypesQuery } from "@navtrack/shared/hooks/queries/useDevicesTypesQuery";
import { useGetAssetsSignalRQuery } from "@navtrack/shared/hooks/queries/useGetAssetsSignalRQuery";
import { mapErrors } from "@navtrack/shared/utils/formik";

export function useAddAsset() {
  const { deviceTypes } = useDevicesTypesQuery();
  const intl = useIntl();
  const addAssetMutation = useAddAssetMutation();
  const history = useHistory();
  const { showNotification } = useNotification();
  const assetsQuery = useGetAssetsSignalRQuery();
  const { setScrollToAsset } = useScrollToAsset();

  const validationSchema: ObjectSchema<AddAssetFormValues> = object({
    name: string().required(
      intl.formatMessage({ id: "generic.name.required" })
    ),
    deviceTypeId: string().required(
      intl.formatMessage({ id: "generic.device-type.required" })
    ),
    serialNumber: string().required(
      intl.formatMessage({ id: "generic.serial-number.required" })
    )
  }).defined();

  const handleSubmit = useCallback(
    (
      values: AddAssetFormValues,
      formikHelpers: FormikHelpers<AddAssetFormValues>
    ) => {
      addAssetMutation.mutate(
        {
          data: {
            name: values.name,
            serialNumber: values.serialNumber,
            deviceTypeId: values.deviceTypeId
          }
        },
        {
          onSuccess: (asset) => {
            showNotification({
              type: "success",
              description: intl.formatMessage({ id: "assets.add.success" })
            });
            assetsQuery.refetch().then(() => {
              setScrollToAsset(asset.id);
            });
            history.push(`/assets/${asset.id}/live`);
          },
          onError: (error) => mapErrors(error, formikHelpers)
        }
      );
    },
    [
      addAssetMutation,
      assetsQuery,
      history,
      intl,
      setScrollToAsset,
      showNotification
    ]
  );

  return {
    deviceTypes,
    validationSchema,
    handleSubmit,
    loading: addAssetMutation.isLoading
  };
}
