import { FormikHelpers } from "formik";
import { useCallback } from "react";
import { useForgotPasswordMutation } from "../../mutations/users/useForgotPasswordMutation";
import { mapErrors } from "../../../utils/formik";
import { ForgotPasswordFormValues } from "./ForgotPasswordFormValues";

type UseForgotPasswordProps = {
  onSuccess?: () => void;
};

export function useForgotPassword(props?: UseForgotPasswordProps) {
  const resetPasswordMutation = useForgotPasswordMutation();

  const resetPassword = useCallback(
    (
      values: ForgotPasswordFormValues,
      formikHelpers: FormikHelpers<ForgotPasswordFormValues>
    ) => {
      resetPasswordMutation.mutate(
        {
          data: {
            email: values.email
          }
        },
        {
          onError: (error) => mapErrors(error, formikHelpers),
          onSuccess: props?.onSuccess
        }
      );
    },
    [props?.onSuccess, resetPasswordMutation]
  );

  return {
    resetPassword,
    loading: resetPasswordMutation.isLoading,
    success: resetPasswordMutation.isSuccess
  };
}
