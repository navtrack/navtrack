import { FormikHelpers } from "formik";
import { useCallback } from "react";
import { useForgotPasswordMutation } from "../../queries/users/useForgotPasswordMutation";
import { mapErrors } from "../../../utils/formik";
import { object, ObjectSchema, string } from "yup";

export type ForgotPasswordFormValues = {
  email: string;
};

export const InitialForgotPasswordFormValues: ForgotPasswordFormValues = {
  email: ""
};

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

  const validationSchema: ObjectSchema<ForgotPasswordFormValues> = object({
    email: string()
      .email("generic.email.invalid")
      .required("generic.email.required")
  }).defined();

  return {
    resetPassword,
    validationSchema,
    loading: resetPasswordMutation.isLoading,
    success: resetPasswordMutation.isSuccess
  };
}
