import { useGetEnvironment } from "../../api/index-generated";

export const useGetEnvironmentQuery = () => {
  const query = useGetEnvironment({
    query: {
      refetchOnWindowFocus: false,
      refetchOnMount: false
    }
  });

  return query;
};
