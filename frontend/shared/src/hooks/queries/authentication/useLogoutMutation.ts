import { axiosInstance } from "../../../axios/axiosInstance";
import { useMutation, UseMutationOptions } from "@tanstack/react-query";

type UseLogoutMutationProps = {
  options?: Omit<UseMutationOptions<void, unknown, string>, "mutationFn">;
};

export function useLogoutMutation(props: UseLogoutMutationProps) {
  const mutation = useMutation<void, unknown, string>({
    mutationFn: async (accessToken) =>
      axiosInstance<void>({
        url: "/auth/logout",
        method: "post",
        headers: { Authorization: `Bearer ${accessToken}` }
      }),
    ...props.options
  });

  return mutation;
}
