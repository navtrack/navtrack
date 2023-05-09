import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { useAddUserToAssetMutation } from "@navtrack/shared/hooks/mutations/useAddUserToAssetMutation";
import { useAssetUsersQuery } from "@navtrack/shared/hooks/queries/useAssetUsersQuery";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { FormikHelpers } from "formik";
import { useCallback } from "react";
import { useIntl } from "react-intl";
import { object, ObjectSchema, string } from "yup";
import { AddUserToAssetFormValues } from "./types";

interface IUseAddUserToAsset {
  close: () => void;
}

export function useAddUserToAsset(props: IUseAddUserToAsset) {
  const intl = useIntl();
  const mutation = useAddUserToAssetMutation();
  const currentAsset = useCurrentAsset();
  const assetUsers = useAssetUsersQuery({
    assetId: currentAsset.data?.id ?? ""
  });

  const validationSchema: ObjectSchema<AddUserToAssetFormValues> = object({
    email: string()
      .email(intl.formatMessage({ id: "generic.email.invalid" }))
      .required(intl.formatMessage({ id: "generic.email.required" })),
    role: string().required(
      intl.formatMessage({ id: "generic.password.required" })
    )
  }).defined();

  const handleSubmit = useCallback(
    (
      values: AddUserToAssetFormValues,
      formikHelpers: FormikHelpers<AddUserToAssetFormValues>
    ) => {
      if (currentAsset.data) {
        mutation.mutate(
          {
            assetId: currentAsset.data.id,
            data: {
              email: values.email,
              role: values.role
            }
          },
          {
            onSuccess: () => {
              assetUsers.refetch();
              props.close();
            },
            onError: (error) => mapErrors(error, formikHelpers)
          }
        );
      }
    },
    [assetUsers, currentAsset, mutation, props]
  );

  return { validationSchema, handleSubmit, loading: mutation.isLoading };
}
