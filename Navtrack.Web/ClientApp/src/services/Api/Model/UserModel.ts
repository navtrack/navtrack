export type UserModel = {
  id: number,
  email: string,
  password: string,
  confirmPassword: string
};

export const DefaultUserModel : UserModel = {
  id: 0,
  email: "",
  password: "",
  confirmPassword: ""
};