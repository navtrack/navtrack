import { useQuery } from "react-query";
import { useRecoilValue } from "recoil";
import { AssetsModel } from "@navtrack/navtrack-shared/dist/api/model/generated";
import { configSelector } from "@navtrack/navtrack-shared";
import useSignalR from "../app/useSignalR";

export default function useGetAssetsSignalRQuery() {
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
}
