import { usePatchUser } from "@navtrack/navtrack-shared";

export const useUpdateUserMutation = () => {
  const mutation = usePatchUser();

  return mutation;
};
