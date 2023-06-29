import { usePatchUser } from "../../../api/index-generated";

export function useUpdateUserMutation() {
  const mutation = usePatchUser({
    mutation: {}
  });

  return mutation;
}
