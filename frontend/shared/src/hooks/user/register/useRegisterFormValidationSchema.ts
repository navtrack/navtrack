import { ref } from "yup";
import { object, ObjectSchema, string } from "yup";
import { RegisterFormValues } from "./RegisterFormValues";
import { useRecoilValue } from "recoil";
import { appConfigAtom } from "../../../state/appConfig";

export const useRegisterFormValidationSchema = () => {
  const appConfig = useRecoilValue(appConfigAtom);

  const validationSchema: ObjectSchema<RegisterFormValues> = object({
    email: string()
      .email("generic.email.invalid")
      .required("generic.email.required"),
    password: string()
      .required("generic.password.required")
      .min(8, "generic.password.requirements.length"),
    confirmPassword: string()
      .required("generic.confirm-password.required")
      .min(8, "generic.password.requirements.length")
      .oneOf([ref("password")], "generic.confirm-password.requirements.match"),
    captcha: appConfig?.captcha?.siteKey
      ? string().required("register.captcha")
      : string().optional()
  }).defined();

  return validationSchema;
};
