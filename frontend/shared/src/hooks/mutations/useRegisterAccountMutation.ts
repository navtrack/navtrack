import { usePostUser } from "../../api/index-generated";

export const useRegisterAccountMutation = () => {
  const mutation = usePostUser();

  return mutation;
};
