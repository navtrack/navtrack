import { useCallback, useEffect } from "react";
import { MapEvents } from "./events";

interface IUseWebViewEventHandler<T> {
  eventName: MapEvents;
  handler: (payload: T) => void;
}

export function useMapEventHandler<T>(props: IUseWebViewEventHandler<T>) {
  const handleEvent = useCallback(
    (event: Event) => {
      const custom = event as CustomEvent<T>;
      props.handler(custom.detail);
    },
    [props]
  );

  useEffect(() => {
    window.addEventListener(props.eventName as unknown as string, handleEvent);

    return () => {
      window.removeEventListener(
        props.eventName as unknown as string,
        handleEvent
      );
    };
  });
}
