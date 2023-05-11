import { useCallback, useContext } from "react";
import { SignalRContext } from "../../../components/SignalRProvider";

export const useSignalR = () => {
  const signalr = useContext(SignalRContext);

  const invoke = useCallback(
    async <T>(
      hubUrl: string,
      methodName: string,
      ...args: any[]
    ): Promise<T | undefined> =>
      signalr
        ?.getConnection(hubUrl)
        .then((hubConnection) => hubConnection?.invoke<T>(methodName, ...args)),
    [signalr]
  );

  const on = useCallback(
    async (
      hubUrl: string,
      methodName: string,
      newMethod: (...args: any[]) => void
    ) =>
      signalr
        ?.getConnection(hubUrl)
        .then((hubConnection) => hubConnection?.on(methodName, newMethod)),
    [signalr]
  );

  return { invoke, on, connectionState: signalr?.connectionState };
};
