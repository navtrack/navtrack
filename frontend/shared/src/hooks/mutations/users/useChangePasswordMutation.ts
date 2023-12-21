import { useAccountChangePassword } from "../../../api/index-generated";

export function useChangePasswordMutation() {
  const mutation = useAccountChangePassword();

  return mutation;
}
