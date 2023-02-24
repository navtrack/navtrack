import { usePostUserPasswordForgot } from "../../../api/index-generated";

export const useForgotPasswordMutation = () => {
  const mutation = usePostUserPasswordForgot();

  return mutation;
};
