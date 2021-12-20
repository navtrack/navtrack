import { useCallback } from "react";
import { useIntl } from "react-intl";
import { object, SchemaOf, string } from "yup";
import useDevicesTypesQuery from "../../../hooks/queries/useDevicesTypesQuery";
import { AddAssetFormValues } from "./AddAssetFormValues";
import { useAddAssetMutation } from "../../../hooks/mutations/useAddAssetMutation";
import { FormikHelpers } from "formik";
import { mapErrors } from "../../../utils/formik";
import { useHistory } from "react-router";
import useGetAssetsSignalRQuery from "../../../hooks/queries/useGetAssetsSignalRQuery";
import useScrollToAsset from "../../ui/layouts/admin/useScrollToAsset";
import useNotification from "../../ui/shared/notification/useNotification";

export default function useAddAsset() {
  const deviceTypes = useDevicesTypesQuery();
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
    deviceTypes: deviceTypes.data?.items ?? [],
    validationSchema,
    handleSubmit,
    loading: addAssetMutation.isLoading
  };
}
