import { usePostAccountPassword } from "@navtrack/navtrack-shared";

export const useChangePasswordMutation = () => {
  const mutation = usePostAccountPassword();

  return mutation;
};
