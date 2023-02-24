import { useForgotPasswordMutation } from "@navtrack/ui-shared/hooks/mutations/users/useForgotPasswordMutation";
import { mapErrors } from "@navtrack/ui-shared/utils/formik";
import { FormikHelpers } from "formik";
import { useCallback } from "react";
import { ForgotPasswordFormValues } from "./ForgotPasswordFormValues";

export const useForgotPassword = () => {
  const resetPasswordMutation = useForgotPasswordMutation();

  const resetPassword = useCallback(
    (
      values: ForgotPasswordFormValues,
      formikHelpers: FormikHelpers<ForgotPasswordFormValues>
    ) => {
      resetPasswordMutation.mutate(
        {
          data: {
            email: values.email,
          },
        },
        {
          onError: (error) => mapErrors(error, formikHelpers),
        }
      );
    },
    [resetPasswordMutation]
  );

  return {
    resetPassword,
    loading: resetPasswordMutation.isLoading,
    success: resetPasswordMutation.isSuccess,
  };
};
