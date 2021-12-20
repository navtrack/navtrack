import { useIntl } from "react-intl";
import { object, SchemaOf, string } from "yup";
import { LoginFormValues } from "./LoginFormValues";

export const useLoginFormValidationSchema = () => {
  const intl = useIntl();

  const validationSchema: SchemaOf<LoginFormValues> = object({
    email: string()
      .email(intl.formatMessage({ id: "generic.email.valid" }))
      .required(intl.formatMessage({ id: "generic.email.required" }))
      .defined(),
    password: string()
      .required(intl.formatMessage({ id: "generic.password.required" }))
      .defined()
  }).defined();

  return validationSchema;
};
