import { useAccountRegister } from "../../../api/index-generated";

export function useRegisterAccountMutation() {
  const mutation = useAccountRegister();

  return mutation;
}
