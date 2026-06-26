import { ref } from "yup";
import { object, ObjectSchema, string } from "yup";
import { RegisterFormValues } from "./RegisterFormValues";
import { appConfigStore } from "../../../state/appConfig";

export const useRegisterFormValidationSchema = () => {
  const validationSchema: ObjectSchema<RegisterFormValues> = object({
    email: string()
      .email("email.invalid")
      .required("email.required"),
    password: string()
      .required("password.required")
      .min(8, "password.requirements.length"),
    confirmPassword: string()
      .required("confirm-password.required")
      .min(8, "password.requirements.length")
      .oneOf([ref("password")], "confirm-password.requirements.match"),
    captcha: appConfigStore.config?.captcha?.siteKey
      ? string().required("register.captcha")
      : string().optional()
  }).defined();

  return validationSchema;
};
