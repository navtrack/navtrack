import { usePostUser } from "../../../api/index-generated";

export function useRegisterAccountMutation() {
  const mutation = usePostUser();

  return mutation;
}
