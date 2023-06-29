import { useCallback } from "react";
import { FormikHelpers } from "formik";
import { mapErrors } from "../../../utils/formik";
import { useAddAssetMutation } from "../../mutations/assets/useAddAssetMutation";
import { useGetAssetsQuery } from "../../queries/useGetAssetsQuery";
import { AddAssetFormValues } from "./AddAssetFormValues";
import { AssetModel } from "../../../api/model/generated";

type UseAddAssetProps = {
  onSuccess: (data: AssetModel) => void;
};

export function useAddAsset(props: UseAddAssetProps) {
  const addAssetMutation = useAddAssetMutation();
  const assetsQuery = useGetAssetsQuery();

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
          onSuccess: (asset) => props.onSuccess(asset),
          onError: (error) => mapErrors(error, formikHelpers)
        }
      );
    },
    [addAssetMutation, props]
  );

  return {
    handleSubmit,
    isLoading: addAssetMutation.isLoading
  };
}
