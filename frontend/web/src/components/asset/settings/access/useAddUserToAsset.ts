import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { useAssetUserCreateMutation } from "@navtrack/shared/hooks/mutations/assets/useAssetUserCreateMutation";
import { useAssetUsersQuery } from "@navtrack/shared/hooks/queries/useAssetUsersQuery";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { FormikHelpers } from "formik";
import { useCallback } from "react";
import { useIntl } from "react-intl";
import { object, ObjectSchema, string } from "yup";

export type AddUserToAssetFormValues = {
  email: string;
  role: string;
};

type UseAddUserToAssetProps = {
  close: () => void;
};

export function useAddUserToAsset(props: UseAddUserToAssetProps) {
  const intl = useIntl();
  const mutation = useAssetUserCreateMutation();
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
