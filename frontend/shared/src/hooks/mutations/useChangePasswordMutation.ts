import { usePostAccountPassword } from "../../api/index-generated";

export const useChangePasswordMutation = () => {
  const mutation = usePostAccountPassword();

  return mutation;
};
