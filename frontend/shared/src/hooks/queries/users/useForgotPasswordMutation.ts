import { useAccountForgotPassword } from "../../../api/index-generated";

export function useForgotPasswordMutation() {
  const mutation = useAccountForgotPassword();

  return mutation;
}
