import { usePostUserPasswordChange } from "../../../api/index-generated";

export const useChangePasswordMutation = () => {
  const mutation = usePostUserPasswordChange();

  return mutation;
};
