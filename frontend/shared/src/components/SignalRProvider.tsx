import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel
} from "@microsoft/signalr";
import { ReactNode, createContext, useCallback, useState } from "react";
import { useRecoilValue } from "recoil";
import { appContextSelector } from "../state/appContext";
import { appConfigAtom } from "../state/appConfig";

type SignalRContextType =
  | {
      getConnection: (hubUrl: string) => Promise<HubConnection | undefined>;
      connectionState: HubConnectionState;
    }
  | undefined;

// TODO remove this
// temporary fix https://github.com/dotnet/aspnetcore/issues/38286
if (!globalThis.document) {
  (globalThis.document as any) = undefined;
}

export const SignalRContext = createContext<SignalRContextType>(undefined);

type SignalRProviderProps = {
  children: ReactNode;
};

export function SignalRProvider(props: SignalRProviderProps) {
  const appContext = useRecoilValue(appContextSelector);
  const appConfig = useRecoilValue(appConfigAtom);
  const [hubConnections, setHubConnection] = useState<
    Record<string, HubConnection>
  >({});

  const getAccessToken = useCallback(() => {
    return appContext.authentication.accessToken as string;
  }, [appContext.authentication.accessToken]);

  const getConnectionState = useCallback(() => {
    const connections = Object.keys(hubConnections).map(
      (key) => hubConnections[key]
    );

    if (connections.length > 0) {
      if (
        connections.find((x) => x.state === HubConnectionState.Disconnected)
      ) {
        return HubConnectionState.Disconnected;
      }

      if (
        connections.find((x) => x.state === HubConnectionState.Disconnecting)
      ) {
        return HubConnectionState.Disconnecting;
      }

      if (
        connections.find((x) => x.state === HubConnectionState.Reconnecting)
      ) {
        return HubConnectionState.Reconnecting;
      }

      if (connections.find((x) => x.state === HubConnectionState.Connecting)) {
        return HubConnectionState.Connecting;
      }

      return HubConnectionState.Connected;
    }

    return HubConnectionState.Disconnected;
  }, [hubConnections]);

  const getConnection = useCallback(
    async (hubName: string): Promise<HubConnection | undefined> => {
      if (appContext.authentication.isAuthenticated) {
        const hubUrl = `${appConfig?.apiUrl}/hubs/${hubName}`;

        const connection = hubConnections[hubUrl];

        if (connection && connection.state === HubConnectionState.Connected) {
          return connection;
        }

        const hubConnection = new HubConnectionBuilder()
          .withUrl(hubUrl, {
            skipNegotiation: true,
            transport: HttpTransportType.WebSockets,
            accessTokenFactory: () => getAccessToken()
          })
          .configureLogging(LogLevel.Warning)
          .withAutomaticReconnect()
          .build();

        await hubConnection
          .start()
          .then(() =>
            setHubConnection((x) => {
              x[hubUrl] = hubConnection;

              return x;
            })
          )
          .catch((error: any) =>
            console.log("Error while establishing connection.", error)
          );

        return hubConnection;
      }

      return undefined;
    },
    [
      appConfig?.apiUrl,
      appContext.authentication.isAuthenticated,
      getAccessToken,
      hubConnections
    ]
  );

  return (
    <SignalRContext.Provider
      value={{ getConnection, connectionState: getConnectionState() }}>
      {props.children}
    </SignalRContext.Provider>
  );
}
