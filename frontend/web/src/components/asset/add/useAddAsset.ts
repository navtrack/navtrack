import { useCallback } from "react";
import { useIntl } from "react-intl";
import { object, SchemaOf, string } from "yup";
import { AddAssetFormValues } from "./AddAssetFormValues";
import { FormikHelpers } from "formik";
import { useHistory } from "react-router";
import useScrollToAsset from "../../ui/layouts/admin/useScrollToAsset";
import useNotification from "../../ui/shared/notification/useNotification";
import { useAddAssetMutation } from "@navtrack/ui-shared/hooks/mutations/useAddAssetMutation";
import { useDevicesTypesQuery } from "@navtrack/ui-shared/hooks/queries/useDevicesTypesQuery";
import { useGetAssetsSignalRQuery } from "@navtrack/ui-shared/hooks/queries/useGetAssetsSignalRQuery";
import { mapErrors } from "@navtrack/ui-shared/utils/formik";

export default function useAddAsset() {
  const { deviceTypes } = useDevicesTypesQuery();
  const intl = useIntl();
  const addAssetMutation = useAddAssetMutation();
  const history = useHistory();
  const { showNotification } = useNotification();
  const assetsQuery = useGetAssetsSignalRQuery();
  const { setScrollToAsset } = useScrollToAsset();

  const validationSchema: SchemaOf<AddAssetFormValues> = object({
    name: string()
      .required(intl.formatMessage({ id: "generic.name.required" }))
      .defined(),
    deviceTypeId: string()
      .required(intl.formatMessage({ id: "generic.device-type.required" }))
      .defined(),
    serialNumber: string()
      .required(intl.formatMessage({ id: "generic.serial-number.required" }))
      .defined()
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
            history.push(`/assets/${asset.shortId}/live`);
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
