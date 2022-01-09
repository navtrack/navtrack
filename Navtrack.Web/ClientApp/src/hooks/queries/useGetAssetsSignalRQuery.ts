import { useQuery } from "react-query";
import { useRecoilValue } from "recoil";
import { AssetListModel } from "../../api/model";
import { configSelector } from "../../state/app.config";
import useSignalR from "../app/useSignalR";

export default function useGetAssetsSignalRQuery() {
  const signalR = useSignalR();
  const config = useRecoilValue(configSelector);

  const query = useQuery(
    "GetAssetsSignalR",
    () =>
      signalR.invoke<AssetListModel>(`${config?.apiUrl}/hubs/assets`, "GetAll"),
    {
      refetchIntervalInBackground: true,
      refetchInterval: 5000
    }
  );

  return query;
}
