import { usePostAccount } from "@navtrack/navtrack-shared";

export const useRegisterAccountMutation = () => {
  const mutation = usePostAccount();

  return mutation;
};
