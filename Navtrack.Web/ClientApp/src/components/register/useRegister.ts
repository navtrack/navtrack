import {
  mapErrors,
  useRegisterAccountMutation
} from "@navtrack/navtrack-app-shared";
import { FormikHelpers } from "formik";
import { useCallback } from "react";
import { RegisterFormValues } from "./RegisterFormValues";

export const useRegister = () => {
  const registerAccountMutation = useRegisterAccountMutation();

  const register = useCallback(
    (
      values: RegisterFormValues,
      formikHelpers: FormikHelpers<RegisterFormValues>
    ) => {
      registerAccountMutation.mutate(
        {
          data: {
            email: values.email,
            password: values.password,
            confirmPassword: values.confirmPassword
          }
        },
        {
          onError: (error) => mapErrors(error, formikHelpers)
        }
      );
    },
    [registerAccountMutation]
  );

  return {
    register,
    loading: registerAccountMutation.isLoading,
    success: registerAccountMutation.isSuccess
  };
};
