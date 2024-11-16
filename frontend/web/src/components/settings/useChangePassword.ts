import { useChangePasswordMutation } from "@navtrack/shared/hooks/queries/users/useChangePasswordMutation";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { FormikHelpers } from "formik";
import { useCallback } from "react";
import { object, ObjectSchema, ref, string } from "yup";
import { useNotification } from "../ui/notification/useNotification";
import { useCurrentUserQuery } from "@navtrack/shared/hooks/queries/user/useCurrentUserQuery";

export type ChangePasswordFormValues = {
  currentPassword?: string;
  password: string;
  confirmPassword: string;
};

export function useChangePassword() {
  const { showNotification } = useNotification();
  const changePasswordMutation = useChangePasswordMutation();
  const currentUser = useCurrentUserQuery();

  const handleSubmit = useCallback(
    (
      values: ChangePasswordFormValues,
      formikHelpers: FormikHelpers<ChangePasswordFormValues>
    ) => {
      changePasswordMutation.mutate(
        {
          data: {
            currentPassword: values.currentPassword,
            password: values.password,
            confirmPassword: values.confirmPassword
          }
        },
        {
          onSuccess: () => {
            showNotification({
              type: "success",
              description: "settings.password.success"
            });
            formikHelpers.resetForm();
          },
          onError: (error) => mapErrors(error, formikHelpers)
        }
      );
    },
    [changePasswordMutation, showNotification]
  );

  const validationSchema: ObjectSchema<ChangePasswordFormValues> = object({
    currentPassword: currentUser.data?.authentication?.password
      ? string().required("settings.password.current-password.required")
      : string().optional(),
    password: string()
      .notOneOf(
        [ref("currentPassword")],
        "settings.password.different-password"
      )
      .required("generic.password.required")
      .min(8, "generic.password.requirements.length"),
    confirmPassword: string()
      .oneOf([ref("password")], "generic.confirm-password.requirements.match")
      .required("generic.confirm-password.required")
      .min(8, "generic.password.requirements.length")
  }).defined();

  return {
    validationSchema,
    handleSubmit,
    isLoading: changePasswordMutation.isLoading
  };
}
