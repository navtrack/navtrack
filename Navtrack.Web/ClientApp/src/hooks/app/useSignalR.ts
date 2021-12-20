import {
  HubConnection,
  HubConnectionBuilder,
  HttpTransportType,
  LogLevel
} from "@microsoft/signalr";
import { useCallback } from "react";
import useAppContext from "./useAppContext";

type HubConnectionState = {
  hubConnection: HubConnection;
  isConnected: boolean;
};

let hubConnections: Record<string, HubConnectionState> = {};

export default function useSignalR() {
  const { appContext } = useAppContext();

  const getAccessToken = useCallback(() => {
    return appContext.token?.accessToken as string;
  }, [appContext.token?.accessToken]);

  const initialise = useCallback(
    async (hubUrl: string): Promise<HubConnection> => {
      if (!hubConnections[hubUrl] && appContext.isAuthenticated) {
        hubConnections[hubUrl] = {
          hubConnection: new HubConnectionBuilder()
            .withUrl(hubUrl, {
              skipNegotiation: true,
              transport: HttpTransportType.WebSockets,
              accessTokenFactory: () => getAccessToken()
            })
            .configureLogging(LogLevel.Critical)
            .withAutomaticReconnect()
            .build(),
          isConnected: false
        };
      }

      if (!hubConnections[hubUrl].isConnected) {
        await hubConnections[hubUrl].hubConnection
          .start()
          .then(() => (hubConnections[hubUrl].isConnected = true))
          .catch((err: any) =>
            console.log("Error while establishing connection.", err)
          );
      }

      return hubConnections[hubUrl].hubConnection;
    },
    [appContext.isAuthenticated, getAccessToken]
  );

  const invoke = useCallback(
    async <T>(
      hubUrl: string,
      methodName: string,
      ...args: any[]
    ): Promise<T> => {
      return initialise(hubUrl).then((hubConnection) => {
        if (hubConnection) {
          return hubConnection.invoke<T>(methodName, ...args);
        }
        return new Promise<T>(() => {});
      });
    },
    [initialise]
  );

  const on = useCallback(
    async (
      hubUrl: string,
      methodName: string,
      newMethod: (...args: any[]) => void
    ) => {
      return initialise(hubUrl).then((hubConnection) =>
        hubConnection.on(methodName, newMethod)
      );
    },
    [initialise]
  );

  return { invoke, on };
}