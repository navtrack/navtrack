import { usePostUserPasswordReset } from "../../../api/index-generated";

export function useResetPasswordMutation() {
  const mutation = usePostUserPasswordReset();

  return mutation;
}
