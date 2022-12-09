import {
  HubConnection,
  HubConnectionBuilder,
  HttpTransportType,
  LogLevel
} from "@microsoft/signalr";
import { useCallback } from "react";
import { useRecoilValue } from "recoil";
import { appContextSelector } from "../../state/app.context";

type HubConnectionState = {
  hubConnection: HubConnection;
  isConnected: boolean;
};

// TODO remove this
// temporary fix https://github.com/dotnet/aspnetcore/issues/38286
if (!globalThis.document) {
  (globalThis.document as any) = undefined;
}

const hubConnections: Record<string, HubConnectionState> = {};

export const useSignalR = () => {
  const appContext = useRecoilValue(appContextSelector);

  const getAccessToken = useCallback(() => {
    return appContext.authentication.token?.accessToken as string;
  }, [appContext.authentication.token?.accessToken]);

  const initialise = useCallback(
    async (hubUrl: string): Promise<HubConnection> => {
      if (
        !hubConnections[hubUrl] &&
        appContext.authentication.isAuthenticated
      ) {
        hubConnections[hubUrl] = {
          hubConnection: new HubConnectionBuilder()
            .withUrl(hubUrl, {
              skipNegotiation: true,
              transport: HttpTransportType.WebSockets,
              accessTokenFactory: () => getAccessToken()
            })
            .configureLogging(LogLevel.Warning)
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
    [appContext.authentication.isAuthenticated, getAccessToken]
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
};
