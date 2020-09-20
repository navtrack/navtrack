import L, { DivIcon, LatLng } from "leaflet";
import { AppContextAccessor } from "./appContext/AppContextAccessor";

let map: L.Map;
// eslint-disable-next-line @typescript-eslint/no-unused-vars
let marker: L.Marker;

let polyline: L.Polyline;

export const MapService = {
  displayRoute: (points: LatLng[]): void => {
    if (!polyline) {
      polyline = L.polyline(points, { color: "blue" }).addTo(map);
    } else {
      polyline.setLatLngs(points);
    }

    map.fitBounds(polyline.getBounds(), {
      paddingTopLeft: [200, 310],
      paddingBottomRight: [20, 40]
    });
  },

  setMapVisibility: (visible: boolean): void => {
    let appAppContext = AppContextAccessor.getAppContext();

    if (appAppContext.mapIsVisible !== visible) {
      AppContextAccessor.setAppContext((x) => {
        return {
          ...x,
          mapIsVisible: visible
        };
      });
    }

    if (map) {
      if (visible) {
        map.dragging.enable();
        map.touchZoom.enable();
        map.doubleClickZoom.enable();
        map.scrollWheelZoom.enable();
        map.boxZoom.enable();
        map.keyboard.enable();

        if (marker) {
          marker.addTo(map);
        }
      } else {
        map.dragging.disable();
        map.touchZoom.disable();
        map.doubleClickZoom.disable();
        map.scrollWheelZoom.disable();
        map.boxZoom.disable();
        map.keyboard.disable();

        if (marker) {
          marker.removeFrom(map);
        }
      }
    }
  },

  initMap: (): void => {
    map = L.map("map", {
      zoomControl: false,
      attributionControl: false
    }).setView([51.505, -0.09], 15);

    L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {}).addTo(map);
  },

  showMarker: (position: [number, number]): void => {
    if (!marker) {
      marker = L.marker(position, {
        icon: new DivIcon({
          html: `<div class="text-blue-500"><i class="fa fa-map-marker-alt fa-3x" /></div>`,
          className: "border-0",
          iconSize: [28, 36],
          iconAnchor: [14, 36]
        })
      }).addTo(map);
    } else {
      marker.setLatLng(position);
    }

    map.panTo(position, { animate: false });
    MapService.centerMap();
  },

  centerMap: (): void => {
    const leftMargin = -90;

    const contentRect = document.getElementById("admin-content")?.getBoundingClientRect();

    if (contentRect) {
      const topMargin = -((contentRect.height - 39 - 26) / 2);

      map.panBy([leftMargin, topMargin], {
        animate: false
      });
    }
  }
};
