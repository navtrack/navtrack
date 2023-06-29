import { usePostUserPasswordForgot } from "../../../api/index-generated";

export function useForgotPasswordMutation() {
  const mutation = usePostUserPasswordForgot();

  return mutation;
}
