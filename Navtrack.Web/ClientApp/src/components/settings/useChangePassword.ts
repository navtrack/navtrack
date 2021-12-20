import { FormikHelpers } from "formik";
import { useCallback } from "react";
import { useIntl } from "react-intl";
import { object, ref, SchemaOf, string } from "yup";
import { useAddAssetMutation } from "../../hooks/mutations/useAddAssetMutation";
import { useChangePasswordMutation } from "../../hooks/mutations/useChangePasswordMutation";
import useDevicesTypesQuery from "../../hooks/queries/useDevicesTypesQuery";
import { mapErrors } from "../../utils/formik";
import useNotification from "../ui/shared/notification/useNotification";
import { ChangePasswordFormValues } from "./types";

export default function useChangePassword() {
  const deviceTypes = useDevicesTypesQuery();
  const intl = useIntl();
  const addAssetMutation = useAddAssetMutation();
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

  const validationSchema: SchemaOf<ChangePasswordFormValues> = object({
    currentPassword: string()
      .required(
        intl.formatMessage({
          id: "settings.password.current-password.required"
        })
      )
      .defined(),
    password: string()
      .not(
        [ref("currentPassword")],
        intl.formatMessage({
          id: "settings.password.different-password"
        })
      )
      .required(intl.formatMessage({ id: "generic.password.required" }))
      .min(
        8,
        intl.formatMessage({ id: "generic.password.requirements.length" })
      )
      .defined(),
    confirmPassword: string()
      .equals(
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
      .defined()
  }).defined();

  return {
    deviceTypes: deviceTypes.data?.items ?? [],
    validationSchema,
    handleSubmit,
    loading: addAssetMutation.isLoading
  };
}
