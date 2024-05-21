export type RegisterFormValues = {
  email: string;
  password: string;
  confirmPassword: string;
  captcha?: string;
};

export const InitialRegisterFormValues: RegisterFormValues = {
  email: "",
  password: "",
  confirmPassword: ""
};
