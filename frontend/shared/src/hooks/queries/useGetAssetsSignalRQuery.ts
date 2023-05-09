import { useQuery } from "react-query";
import { AssetsModel } from "../../api/model/generated";
import { useSignalR } from "../app/signalR/useSignalR";

export const useGetAssetsSignalRQuery = () => {
  const signalR = useSignalR();

  const query = useQuery(
    "GetAssetsSignalR",
    () => signalR.invoke<AssetsModel>(`assets`, "GetAll"),
    {
      refetchIntervalInBackground: true,
      refetchInterval: 5000
    }
  );

  return query;
};
