import { usePostAccountPassword } from "../../api";

export const useChangePasswordMutation = () => {
  const mutation = usePostAccountPassword();

  return mutation;
};
