import Map from "../ui/shared/map/Map";
import MapPin from "../ui/shared/map/MapPin";
import { useEffect, useState } from "react";
import { DEFAULT_MAP_CENTER } from "../../constants";
import {
  LocationPinUpdateEvent,
  useWebViewEventHandler
} from "@navtrack/navtrack-app-shared";

export function Maps() {
  const [event, setEvent] = useState<LocationPinUpdateEvent>();

  useWebViewEventHandler<LocationPinUpdateEvent>({
    eventName: "LocationPinUpdateEvent",
    handler: (e: LocationPinUpdateEvent) => {
      setEvent(e);
    }
  });

  useEffect(() => {
    if (typeof window !== undefined && "ReactNativeWebView" in window) {
      const reactNativeWebView = (window as any).ReactNativeWebView;

      reactNativeWebView.postMessage("loaded");
    }
  }, []);

  return (
    <div className="flex min-h-screen">
      <Map
        center={{
          latitude: DEFAULT_MAP_CENTER.latitude,
          longitude: DEFAULT_MAP_CENTER.longitude
        }}
        zoom={event ? 16 : 2}
        hideZoomControl
        hideAttribution>
        <MapPin
          latitude={event?.latitude}
          longitude={event?.longitude}
          zoom={event?.resetZoom ? 16 : undefined}
          follow={event?.follow}
        />
      </Map>
    </div>
  );
}
