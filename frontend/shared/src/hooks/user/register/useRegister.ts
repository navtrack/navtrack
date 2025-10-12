import { FormikHelpers } from "formik";
import { useCallback } from "react";
import { useRegisterAccountMutation } from "../../queries/users/useRegisterAccountMutation";
import { mapErrors } from "../../../utils/formik";
import { RegisterFormValues } from "./RegisterFormValues";

type UseRegisterProps = {
  onSuccess?: (values: RegisterFormValues) => void;
};

export const useRegister = (props?: UseRegisterProps) => {
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
            confirmPassword: values.confirmPassword,
            captcha: values.captcha
          }
        },
        {
          onError: (error) => mapErrors(error, formikHelpers),
          onSuccess: () => props?.onSuccess?.(values)
        }
      );
    },
    [props, registerAccountMutation]
  );

  return {
    register,
    loading: registerAccountMutation.isPending,
    success: registerAccountMutation.isSuccess
  };
};
