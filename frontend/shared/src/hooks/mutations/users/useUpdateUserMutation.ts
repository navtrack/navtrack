import { usePatchUser } from "../../../api/index-generated";

export const useUpdateUserMutation = () => {
  const mutation = usePatchUser();

  return mutation;
};
