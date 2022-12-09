import MapPin from "../ui/shared/map/MapPin";
import { useEffect, useState } from "react";
import { useMapEventHandler } from "@navtrack/ui-shared/hooks/map-events/useMapEventHandler";
import {
  LocationPinUpdateEvent,
  MapEvents
} from "@navtrack/ui-shared/hooks/map-events/events";

export const LocationPinUpdateEventHandler = () => {
  const [event, setEvent] = useState<LocationPinUpdateEvent>();

  useMapEventHandler<LocationPinUpdateEvent>({
    eventName: MapEvents.LocationPinUpdateEvent,
    handler: (e: LocationPinUpdateEvent) => {
      setEvent(e);
    }
  });

  useEffect(() => {
    if (typeof window !== "undefined" && "ReactNativeWebView" in window) {
      const reactNativeWebView = (window as any).ReactNativeWebView;

      reactNativeWebView.postMessage("loaded");
    }
  }, []);

  return (
    <MapPin
      latitude={event?.latitude}
      longitude={event?.longitude}
      zoom={event?.resetZoom ? 16 : undefined}
      follow={event?.follow}
    />
  );
};
