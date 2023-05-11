import { object, ObjectSchema, string } from "yup";
import { LoginFormValues } from "./LoginFormValues";

export const useLoginFormValidationSchema = () => {
  const validationSchema: ObjectSchema<LoginFormValues> = object({
    email: string()
      .email("generic.email.invalid")
      .required("generic.email.required"),
    password: string().required("generic.password.required")
  }).defined();

  return validationSchema;
};
