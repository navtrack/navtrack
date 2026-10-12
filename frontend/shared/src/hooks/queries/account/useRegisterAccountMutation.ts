import { useAccountCreateAccount } from "../../../api";

export function useRegisterAccountMutation() {
  const mutation = useAccountCreateAccount();

  return mutation;
}
