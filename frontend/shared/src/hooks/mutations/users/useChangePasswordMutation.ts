import { usePostUserPasswordChange } from "../../../api/index-generated";

export function useChangePasswordMutation() {
  const mutation = usePostUserPasswordChange();

  return mutation;
}
