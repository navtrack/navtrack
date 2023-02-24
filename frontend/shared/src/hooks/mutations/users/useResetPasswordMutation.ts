import { usePostUserPasswordReset } from "../../../api/index-generated";

export const useResetPasswordMutation = () => {
  const mutation = usePostUserPasswordReset();

  return mutation;
};
