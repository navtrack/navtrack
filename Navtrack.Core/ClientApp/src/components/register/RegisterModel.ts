export type RegisterModel = {
  email: string;
  password: string;
  confirmPassword: string;
};

export const InitialRegisterModel: RegisterModel = {
  email: "",
  password: "",
  confirmPassword: ""
};
