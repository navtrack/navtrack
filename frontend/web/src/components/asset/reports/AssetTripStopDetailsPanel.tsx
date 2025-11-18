import {
  Dialog,
  DialogBackdrop,
  DialogPanel,
  DialogTitle
} from "@headlessui/react";
import { XMarkIcon } from "@heroicons/react/24/outline";
import { ZINDEX_PANEL } from "../../../constants";
import {
  faFlagCheckered,
  faStopwatch
} from "@fortawesome/free-solid-svg-icons";
import { FormattedMessage } from "react-intl";
import { MapPin } from "../../ui/map/MapPin";
import { Map } from "../../ui/map/Map";
import { GeocodeReverse } from "@navtrack/shared/components/components/geo/GeocodeReverse";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { CardMapWrapper } from "../../ui/map/CardMapWrapper";
import { TripStopModel } from "@navtrack/shared/hooks/queries/assets/useAssetTripsStopsQueries";
import { DEFAULT_MAP_ZOOM_FOR_PIN } from "@navtrack/shared/constants";
import { MapCenter } from "../../ui/map/MapCenter";
import { TimelineItem } from "../../ui/timeline/TimelineItem";
import { faFlag } from "@fortawesome/free-regular-svg-icons";
import { Button } from "../../ui/button/Button";

type AssetTripDetailsPanelProps = {
  stop: TripStopModel;
  open: boolean;
  close: () => void;
  previous: () => void;
  next: () => void;
  nextDisabled: boolean;
  previousDisabled: boolean;
};

export function AssetTripStopDetailsPanel(props: AssetTripDetailsPanelProps) {
  const show = useShow();

  return (
    <Dialog
      open={props.open}
      onClose={props.close}
      className="relative"
      style={{ zIndex: ZINDEX_PANEL }}>
      <DialogBackdrop className="fixed inset-0 bg-black/30" />
      <div className="fixed inset-0" />
      <div className="fixed inset-0 overflow-hidden">
        <div className="absolute inset-0 overflow-hidden">
          <div className="pointer-events-none fixed inset-y-0 right-0 flex max-w-full pl-10 sm:pl-16">
            <DialogPanel
              transition
              className="pointer-events-auto w-screen max-w-4xl transform transition duration-500 ease-in-out data-closed:translate-x-full sm:duration-700">
              <div className="relative flex h-full flex-col overflow-y-auto bg-white py-6 shadow-xl space-y-4">
                <div className="px-4 border-b border-gray-200 flex justify-between pb-4 items-center">
                  <DialogTitle className="text-base font-semibold text-gray-900">
                    <FormattedMessage id="generic.stop-details" />
                  </DialogTitle>
                  <div className="flex space-x-2">
                    <Button
                      onClick={props.previous}
                      size="sm"
                      color="white"
                      disabled={props.previousDisabled}>
                      <FormattedMessage id="generic.previous" />
                    </Button>
                    <Button
                      onClick={props.next}
                      size="sm"
                      color="white"
                      disabled={props.nextDisabled}>
                      <FormattedMessage id="generic.next" />
                    </Button>
                    <button
                      type="button"
                      onClick={props.close}
                      className="cursor-pointer relative rounded-md text-gray-400 hover:text-gray-500 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600">
                      <XMarkIcon aria-hidden="true" className="size-6" />
                    </button>
                  </div>
                </div>
                <div className="px-6 border-b border-gray-200 pb-4">
                  <TimelineItem
                    icon={faFlagCheckered}
                    title={
                      <FormattedMessage
                        id="generic.arrived-at"
                        values={{ date: show.dateTime(props.stop.arrivalDate) }}
                      />
                    }
                    subTitle={
                      <GeocodeReverse
                        coordinates={props.stop.arrivalCoordinates}
                      />
                    }
                  />
                  <TimelineItem
                    icon={faStopwatch}
                    title={
                      <FormattedMessage
                        id="generic.stopped-for"
                        values={{
                          duration: show.duration(props.stop.duration)
                        }}
                      />
                    }
                  />
                  {props.stop.departureCoordinates && (
                    <TimelineItem
                      icon={faFlag}
                      last
                      title={
                        <FormattedMessage
                          id="generic.departed-at"
                          values={{
                            date: show.dateTime(props.stop.departureDate)
                          }}
                        />
                      }
                      subTitle={
                        <GeocodeReverse
                          coordinates={props.stop.departureCoordinates}
                        />
                      }
                    />
                  )}
                </div>
                <div className="grow px-6 flex" style={{ minHeight: 300 }}>
                  <CardMapWrapper>
                    <Map initialZoom={DEFAULT_MAP_ZOOM_FOR_PIN}>
                      <MapPin
                        pin={{ coordinates: props.stop.arrivalCoordinates }}
                      />
                      <MapCenter
                        position={props.stop.arrivalCoordinates}
                        zoom={DEFAULT_MAP_ZOOM_FOR_PIN}
                      />
                    </Map>
                  </CardMapWrapper>
                </div>
              </div>
            </DialogPanel>
          </div>
        </div>
      </div>
    </Dialog>
  );
}
