import { useCallback, useState } from "react";
import { useIntl } from "react-intl";
import { object, ObjectSchema, string } from "yup";
import { FormikHelpers } from "formik";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { useRenameAssetMutation } from "@navtrack/shared/hooks/mutations/assets/useRenameAssetMutation";
import { useAssetsQuery } from "@navtrack/shared/hooks/queries/useAssetsQuery";
import { mapErrors } from "@navtrack/shared/utils/formik";

export type RenameAssetFormValues = {
  name?: string; // TODO: make required
};

export function useRenameAsset() {
  const currentAsset = useCurrentAsset();
  const renameAssetMutation = useRenameAssetMutation();
  const assetsQuery = useAssetsQuery();
  const [showSuccess, setShowSuccess] = useState(false);

  const submit = useCallback(
    (
      values: RenameAssetFormValues,
      formikHelpers: FormikHelpers<RenameAssetFormValues>
    ) => {
      setShowSuccess(false);
      if (currentAsset.data) {
        renameAssetMutation.mutate(
          { assetId: currentAsset.data?.id, data: { name: values.name } },
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

  const validationSchema: ObjectSchema<{ name: string }> = object({
    name: string().required(intl.formatMessage({ id: "generic.name.required" }))
  }).defined();

  return {
    submit,
    validationSchema,
    showSuccess,
    loading: renameAssetMutation.isLoading
  };
}
