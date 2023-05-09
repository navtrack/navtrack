import { useChangePasswordMutation } from "@navtrack/shared/hooks/mutations/users/useChangePasswordMutation";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { FormikHelpers } from "formik";
import { useCallback } from "react";
import { useIntl } from "react-intl";
import { object, ObjectSchema, ref, string } from "yup";
import { useNotification } from "../ui/shared/notification/useNotification";
import { ChangePasswordFormValues } from "./types";

export function useChangePassword() {
  const intl = useIntl();
  const { showNotification } = useNotification();
  const changePasswordMutation = useChangePasswordMutation();

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
              description: intl.formatMessage({
                id: "settings.password.success"
              })
            });
            formikHelpers.resetForm();
          },
          onError: (error) => mapErrors(error, formikHelpers)
        }
      );
    },
    [changePasswordMutation, intl, showNotification]
  );

  const validationSchema: ObjectSchema<ChangePasswordFormValues> = object({
    currentPassword: string().required(
      intl.formatMessage({
        id: "settings.password.current-password.required"
      })
    ),
    password: string()
      .notOneOf(
        [ref("currentPassword")],
        intl.formatMessage({
          id: "settings.password.different-password"
        })
      )
      .required(intl.formatMessage({ id: "generic.password.required" }))
      .min(
        8,
        intl.formatMessage({ id: "generic.password.requirements.length" })
      ),
    confirmPassword: string()
      .oneOf(
        [ref("password")],
        intl.formatMessage({
          id: "generic.confirm-password.requirements.match"
        })
      )
      .required(intl.formatMessage({ id: "generic.confirm-password.required" }))
      .min(
        8,
        intl.formatMessage({ id: "generic.password.requirements.length" })
      )
  }).defined();

  return {
    validationSchema,
    handleSubmit
  };
}
