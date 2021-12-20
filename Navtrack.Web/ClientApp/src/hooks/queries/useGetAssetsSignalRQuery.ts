import { useQuery } from "react-query";
import { AssetList } from "../../api/model";
import useSignalR from "../app/useSignalR";
import { useConfig } from "../config/useConfig";

export default function useGetAssetsSignalRQuery() {
  const signalR = useSignalR();
  const config = useConfig();

  const query = useQuery(
    "GetAssetsSignalR",
    () => signalR.invoke<AssetList>(`${config?.apiUrl}/hubs/assets`, "GetAll"),
    {
      refetchIntervalInBackground: true,
      refetchInterval: 5000
    }
  );

  return query;
}
