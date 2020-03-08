import { UserRole } from "./UserRole";

export type UserModel = {
  id: number;
  email: string;
  password: string;
  confirmPassword: string;
  role: UserRole;
};

export const DefaultUserModel: UserModel = {
  id: 0,
  email: "",
  password: "",
  confirmPassword: "",
  role: UserRole.User
};
