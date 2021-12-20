import { useIntl } from "react-intl";
import { object, SchemaOf, string } from "yup";
import { ForgotPasswordFormValues } from "./ForgotPasswordFormValues";

export const useForgotPasswordFormValidationSchema = () => {
  const intl = useIntl();

  const validationSchema: SchemaOf<ForgotPasswordFormValues> = object({
    email: string()
      .email(intl.formatMessage({ id: "generic.email.valid" }))
      .required(intl.formatMessage({ id: "generic.email.required" }))
      .defined()
  }).defined();

  return validationSchema;
};
