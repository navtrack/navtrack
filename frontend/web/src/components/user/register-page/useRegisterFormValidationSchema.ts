import { ref } from "yup";
import { object, SchemaOf, string } from "yup";
import { RegisterFormValues } from "./RegisterFormValues";

export const useRegisterFormValidationSchema = () => {
  const validationSchema: SchemaOf<RegisterFormValues> = object({
    email: string()
      .email("generic.email.valid")
      .required("generic.email.required")
      .defined(),
    password: string()
      .required("generic.password.required")
      .min(8, "generic.password.requirements.length")
      .defined(),
    confirmPassword: string()
      .equals([ref("password")], "generic.confirm-password.requirements.match")
      .required("generic.confirm-password.required")
      .min(8, "generic.password.requirements.length")
      .defined()
      .defined(),
  });

  return validationSchema;
};
