import { useAccountCreateAccount } from "../../../api/index-generated";

export function useRegisterAccountMutation() {
  const mutation = useAccountCreateAccount();

  return mutation;
}
