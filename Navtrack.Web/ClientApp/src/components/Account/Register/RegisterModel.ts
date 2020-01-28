export type RegisterModel = {
  email: string,
  password: string,
  confirmPassword: string
};

export const DefaultRegisterModel: RegisterModel = {
  email: "",
  password: "",
  confirmPassword: ""
};