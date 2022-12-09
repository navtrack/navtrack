import { useQuery } from "react-query";
import { useRecoilValue } from "recoil";
import { AssetsModel } from "../../api/model/generated";
import { configSelector } from "../../state/app.config";
import { useSignalR } from "../signalr/useSignalR";

export const useGetAssetsSignalRQuery = () => {
  const signalR = useSignalR();
  const config = useRecoilValue(configSelector);

  const query = useQuery(
    "GetAssetsSignalR",
    () =>
      signalR.invoke<AssetsModel>(`${config?.apiUrl}/hubs/assets`, "GetAll"),
    {
      refetchIntervalInBackground: true,
      refetchInterval: 5000
    }
  );

  return query;
};
