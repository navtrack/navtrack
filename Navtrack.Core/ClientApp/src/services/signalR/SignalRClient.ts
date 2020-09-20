import {
  HubConnection,
  HubConnectionBuilder,
  HttpTransportType,
  LogLevel
} from "@microsoft/signalr";
import { AppContextAccessor } from "../appContext/AppContextAccessor";

type HubConnectionState = {
  hubConnection: HubConnection;
  isConnected: boolean;
};

let hubConnections: Record<string, HubConnectionState> = {};

export const SignalRClient = {
  invoke: async <T>(hubUrl: string, methodName: string, ...args: any[]): Promise<T> => {
    return initialise(hubUrl).then((hubConnection) => {
      // if (hubConnection) {
      return hubConnection.invoke<T>(methodName, ...args);
      // }
      // return new Promise<T>(() => []);
    });
  },

  on: async (hubUrl: string, methodName: string, newMethod: (...args: any[]) => void) => {
    return initialise(hubUrl).then((hubConnection) => hubConnection.on(methodName, newMethod));
  }
};

const initialise = async (hubUrl: string): Promise<HubConnection> => {
  if (!hubConnections[hubUrl]) {
    hubConnections[hubUrl] = {
      hubConnection: new HubConnectionBuilder()
        .withUrl(hubUrl, {
          skipNegotiation: true,
          transport: HttpTransportType.WebSockets,
          accessTokenFactory: () =>
            AppContextAccessor.getAppContext().authenticationInfo.access_token
        })
        .configureLogging(LogLevel.Trace)
        .withAutomaticReconnect()
        .build(),
      isConnected: false
    };
  }

  if (!hubConnections[hubUrl].isConnected) {
    await hubConnections[hubUrl].hubConnection
      .start()
      .then(() => (hubConnections[hubUrl].isConnected = true))
      .catch((err: any) => console.log("Error while establishing connection.", err));
  }

  return hubConnections[hubUrl].hubConnection;
};
