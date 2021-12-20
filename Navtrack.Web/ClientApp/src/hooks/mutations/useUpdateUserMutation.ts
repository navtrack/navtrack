import { usePatchUser } from "../../api";

export const useUpdateUserMutation = () => {
  const mutation = usePatchUser();

  return mutation;
};
