import { useAccountForgotPassword } from "../../../api";

export function useForgotPasswordMutation() {
  const mutation = useAccountForgotPassword();

  return mutation;
}
