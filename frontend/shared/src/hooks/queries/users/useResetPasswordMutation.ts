import { useAccountResetPassword } from "../../../api/index-generated";

export function useResetPasswordMutation() {
  const mutation = useAccountResetPassword();

  return mutation;
}
