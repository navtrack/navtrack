import { IntlShape } from "react-intl";
import { object, SchemaOf, string } from "yup";
import { LoginModel } from "./LoginModel";

export const GetLoginModelValidation = (intl: IntlShape): SchemaOf<LoginModel> =>
  object({
    email: string()
      .email(intl.formatMessage({ id: "generic.email.valid" }))
      .required(intl.formatMessage({ id: "generic.email.required" }))
      .defined(),
    password: string()
      .required(intl.formatMessage({ id: "generic.password.required" }))
      .defined()
  }).defined();
