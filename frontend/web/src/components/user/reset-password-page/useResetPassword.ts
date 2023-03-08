import { useResetPasswordMutation } from "@navtrack/shared/hooks/mutations/users/useResetPasswordMutation";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { FormikHelpers } from "formik";
import { useCallback } from "react";
import { useParams } from "react-router-dom";
import { ResetPasswordFormValues } from "./ResetPasswordFormValues";

export const useResetPassword = () => {
  const changePasswordMutation = useResetPasswordMutation();
  const { hash } = useParams<{ hash: string }>();

  const resetPassword = useCallback(
    (
      values: ResetPasswordFormValues,
      formikHelpers: FormikHelpers<ResetPasswordFormValues>
    ) => {
      changePasswordMutation.mutate(
        {
          data: {
            hash: hash,
            password: values.password,
            confirmPassword: values.confirmPassword
          }
        },
        {
          onError: (error) => mapErrors(error, formikHelpers)
        }
      );
    },
    [changePasswordMutation, hash]
  );

  return {
    resetPassword,
    loading: changePasswordMutation.isLoading,
    success: changePasswordMutation.isSuccess
  };
};
