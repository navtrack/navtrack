import { useCallback, useState } from "react";
import { useIntl } from "react-intl";
import { object, SchemaOf, string } from "yup";
import { RenameAssetFormValues } from "./types";
import { FormikHelpers } from "formik";
import {
  mapErrors,
  useCurrentAsset,
  useGetAssetsSignalRQuery,
  useRenameAssetMutation
} from "@navtrack/navtrack-app-shared";

export default function useRenameAsset() {
  const currentAsset = useCurrentAsset();
  const renameAssetMutation = useRenameAssetMutation();
  const assetsQuery = useGetAssetsSignalRQuery();
  const [showSuccess, setShowSuccess] = useState(false);

  const submit = useCallback(
    (
      values: RenameAssetFormValues,
      formikHelpers: FormikHelpers<RenameAssetFormValues>
    ) => {
      setShowSuccess(false);
      if (currentAsset) {
        renameAssetMutation.mutate(
          { assetId: currentAsset?.id, data: { name: values.name } },
          {
            onSuccess: () => {
              assetsQuery.refetch();
              setShowSuccess(true);
              setInterval(() => setShowSuccess(false), 5000);
            },
            onError: (error) => mapErrors(error, formikHelpers)
          }
        );
      }
    },
    [assetsQuery, currentAsset, renameAssetMutation]
  );

  const intl = useIntl();

  const validationSchema: SchemaOf<{ name: string }> = object({
    name: string()
      .required(intl.formatMessage({ id: "generic.name.required" }))
      .defined()
  }).defined();

  return {
    submit,
    validationSchema,
    showSuccess,
    loading: renameAssetMutation.isLoading
  };
}
