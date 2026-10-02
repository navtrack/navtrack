import { authAxiosInstance } from "../../../axios/authAxiosInstance";
import { useMutation, UseMutationOptions } from "@tanstack/react-query";

type UseLogoutMutationProps = {
  options?: Omit<UseMutationOptions<void, unknown, void>, "mutationFn">;
};

export function useLogoutMutation(props: UseLogoutMutationProps) {
  const mutation = useMutation<void, unknown, void>({
    mutationFn: async () =>
      authAxiosInstance<void>({
        url: "/auth/logout",
        method: "post"
      }),
    ...props.options
  });

  return mutation;
}
