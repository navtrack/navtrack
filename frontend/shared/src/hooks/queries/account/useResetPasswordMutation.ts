import { useAccountResetPassword } from "../../../api";

export function useResetPasswordMutation() {
  const mutation = useAccountResetPassword();

  return mutation;
}
