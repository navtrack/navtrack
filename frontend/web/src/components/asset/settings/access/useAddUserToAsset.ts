import { AssetUserRole } from "@navtrack/shared/api/model/generated";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { useAssetUserCreateMutation } from "@navtrack/shared/hooks/queries/assets/useAssetUserCreateMutation";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { FormikHelpers } from "formik";
import { useCallback } from "react";
import { object, ObjectSchema, string } from "yup";

export type AddUserToAssetFormValues = {
  email: string;
  role: string;
};

type UseAddUserToAssetProps = {
  close: () => void;
};

export function useAddUserToAsset(props: UseAddUserToAssetProps) {
  const mutation = useAssetUserCreateMutation();
  const currentAsset = useCurrentAsset();

  const validationSchema: ObjectSchema<AddUserToAssetFormValues> = object({
    email: string()
      .email("generic.email.invalid")
      .required("generic.email.required"),
    role: string().required("generic.password.required").defined()
  });
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
              userRole: values.role as AssetUserRole // TODO: Fix this
            }
          },
          {
            onSuccess: () => {
              props.close();
            },
            onError: (error) => mapErrors(error, formikHelpers)
          }
        );
      }
    },
    [currentAsset, mutation, props]
  );

  return { validationSchema, handleSubmit, loading: mutation.isLoading };
}
