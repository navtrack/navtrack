import { useResetPasswordMutation } from "@navtrack/shared/hooks/queries/users/useResetPasswordMutation";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { FormikHelpers } from "formik";
import { useCallback } from "react";
import { useParams } from "react-router-dom";
import { object, ObjectSchema, ref, string } from "yup";

export type ResetPasswordFormValues = {
  password: string;
  confirmPassword: string;
};

export const InitialResetPasswordFormValues: ResetPasswordFormValues = {
  password: "",
  confirmPassword: ""
};

export const useResetPassword = () => {
  const changePasswordMutation = useResetPasswordMutation();
  const { id } = useParams<{ id: string }>();

  const resetPassword = useCallback(
    (
      values: ResetPasswordFormValues,
      formikHelpers: FormikHelpers<ResetPasswordFormValues>
    ) => {
      changePasswordMutation.mutate(
        {
          data: {
            id: id!,
            password: values.password,
            confirmPassword: values.confirmPassword
          }
        },
        {
          onError: (error) => mapErrors(error, formikHelpers)
        }
      );
    },
    [changePasswordMutation, id]
  );

  const validationSchema: ObjectSchema<ResetPasswordFormValues> = object({
    password: string()
      .required("generic.password.required")
      .min(8, "generic.password.requirements.length"),
    confirmPassword: string()
      .required("generic.confirm-password.required")
      .min(8, "generic.password.requirements.length")
      .oneOf([ref("password")], "generic.confirm-password.requirements.match")
  }).defined();

  return {
    resetPassword,
    validationSchema,
    loading: changePasswordMutation.isPending,
    success: changePasswordMutation.isSuccess
  };
};
