import { createMutation } from "../queries/createMutation";

export type RegisterRequest = {
  email: string;
  password: string;
  confirmPassword: string;
};

export const useAccountRegisterMutation = createMutation<RegisterRequest>({
  url: "api/account/register"
});
