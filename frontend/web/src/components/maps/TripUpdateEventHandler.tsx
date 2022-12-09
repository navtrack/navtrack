import { useEffect, useState } from "react";
import { useMapEventHandler } from "@navtrack/ui-shared/hooks/map-events/useMapEventHandler";
import {
  MapEvents,
  TripUpdateEvent
} from "@navtrack/ui-shared/hooks/map-events/events";
import { MapTrip } from "../ui/shared/map/MapTrip";

export const TripUpdateEventHandler = () => {
  const [event, setEvent] = useState<TripUpdateEvent>();

  useMapEventHandler<TripUpdateEvent>({
    eventName: MapEvents.TripUpdateEvent,
    handler: (e: TripUpdateEvent) => {
      setEvent(e);
    }
  });

  useEffect(() => {
    if (typeof window !== "undefined" && "ReactNativeWebView" in window) {
      const reactNativeWebView = (window as any).ReactNativeWebView;

      reactNativeWebView.postMessage("loaded");
    }
  }, []);

  return <MapTrip trip={event?.trip} />;
};
