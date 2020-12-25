import { IntlShape } from "react-intl";
import { ref } from "yup";
import { object, SchemaOf, string } from "yup";
import { RegisterModel } from "./RegisterModel";

export const GetRegisterModelValidation = (intl: IntlShape): SchemaOf<RegisterModel> =>
  object({
    email: string()
      .email(intl.formatMessage({ id: "generic.email.valid" }))
      .required(intl.formatMessage({ id: "generic.email.required" }))
      .defined(),
    password: string()
      .required(intl.formatMessage({ id: "generic.password.required" }))
      .min(8, intl.formatMessage({ id: "generic.password.requirements.length" }))
      .defined(),
    confirmPassword: string()
      .equals(
        [ref("password")],
        intl.formatMessage({ id: "generic.confirmPassword.requirements.match" })
      )
      .required(intl.formatMessage({ id: "generic.password.required" }))
      .min(8, intl.formatMessage({ id: "generic.password.requirements.length" }))
      .defined()
  }).defined();
