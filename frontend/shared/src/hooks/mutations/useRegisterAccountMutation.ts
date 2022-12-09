import { usePostAccount } from "../../api/index-generated";

export const useRegisterAccountMutation = () => {
  const mutation = usePostAccount();

  return mutation;
};
