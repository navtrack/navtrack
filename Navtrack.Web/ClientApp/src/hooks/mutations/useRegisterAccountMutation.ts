import { usePostAccount } from "../../api";

export const useRegisterAccountMutation = () => {
  const mutation = usePostAccount();

  return mutation;
};
