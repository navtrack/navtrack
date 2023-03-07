import { object, SchemaOf, string } from "yup";
import { LoginFormValues } from "./LoginFormValues";

export const useLoginFormValidationSchema = () => {
  const validationSchema: SchemaOf<LoginFormValues> = object({
    email: string()
      .email("generic.email.invalid")
      .required("generic.email.required")
      .defined(),
    password: string().required("generic.password.required").defined(),
  }).defined();

  return validationSchema;
};
