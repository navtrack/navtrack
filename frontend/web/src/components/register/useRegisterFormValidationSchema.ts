import { useIntl } from "react-intl";
import { ref } from "yup";
import { object, SchemaOf, string } from "yup";
import { RegisterFormValues } from "./RegisterFormValues";

export const useRegisterFormValidationSchema = () => {
  const intl = useIntl();

  const validationSchema: SchemaOf<RegisterFormValues> = object({
    email: string()
      .email(intl.formatMessage({ id: "generic.email.valid" }))
      .required(intl.formatMessage({ id: "generic.email.required" }))
      .defined(),
    password: string()
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

  return validationSchema;
};
