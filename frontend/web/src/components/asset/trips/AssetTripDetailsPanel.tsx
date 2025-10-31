import { useContext, useMemo, useState } from "react";
import {
  Dialog,
  DialogBackdrop,
  DialogPanel,
  DialogTitle
} from "@headlessui/react";
import { XMarkIcon } from "@heroicons/react/24/outline";
import { ZINDEX_PANEL } from "../../../constants";
import { PositionDataModel, TripModel } from "@navtrack/shared/api/model";
import { faArrowRight, faInfoCircle } from "@fortawesome/free-solid-svg-icons";
import { FormattedMessage } from "react-intl";
import { Card } from "../../ui/card/Card";
import { MapCenter } from "../../ui/map/MapCenter";
import { MapPin } from "../../ui/map/MapPin";
import { MapTrip } from "../../ui/map/MapTrip";
import { Slider } from "../../ui/slider/Slider";
import { PositionCardItems } from "../shared/position-card/PositionCardItems";
import { Map } from "../../ui/map/Map";
import { Icon } from "../../ui/icon/Icon";
import { SlotContext } from "../../../app/SlotContext";
import { useOnChange } from "@navtrack/shared/hooks/util/useOnChange";
import { GeocodeReverse } from "@navtrack/shared/components/components/geo/GeocodeReverse";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { PositionCardItem } from "../shared/position-card/PositionCardItem";
import { CardMapWrapper } from "../../ui/map/CardMapWrapper";

type AssetTripDetailsPanelProps = {
  trip: TripModel;
  open: boolean;
  close: () => void;
};

export function AssetTripDetailsPanel(props: AssetTripDetailsPanelProps) {
  const show = useShow();
  const [selectedTripLocationIndex, setSelectedTripLocationIndex] =
    useState<number>(0);

  const selectedTripPosition = useMemo(
    () =>
      props.trip !== undefined && selectedTripLocationIndex !== undefined
        ? props.trip.positions[selectedTripLocationIndex]
        : undefined,
    [props.trip, selectedTripLocationIndex]
  );

  const [reverseGeocodePosition, setReverseGeocodePosition] = useState<
    PositionDataModel | undefined
  >(selectedTripPosition);

  const [showPin, setShowPin] = useState(false);
  const slots = useContext(SlotContext);

  useOnChange(props.trip, () => {
    setShowPin(false);
    setSelectedTripLocationIndex(0);

    console.log("Trip changed");
  });

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
                    <FormattedMessage id="generic.trip-details" />
                  </DialogTitle>
                  <button
                    type="button"
                    onClick={props.close}
                    className="relative rounded-md text-gray-400 hover:text-gray-500 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600">
                    <XMarkIcon aria-hidden="true" className="size-6" />
                  </button>
                </div>
                <div className="px-6 flex border-b border-gray-200 space-x-6 pb-4">
                  <PositionCardItem
                    label="generic.start"
                    value={
                      <div>
                        <div>
                          {show.dateTime(props.trip.startPosition.date)}
                        </div>
                        <div>
                          <GeocodeReverse
                            coordinates={props.trip.startPosition.coordinates}
                          />
                        </div>
                      </div>
                    }
                  />
                  <div className="flex items-center">
                    <Icon icon={faArrowRight} />
                  </div>
                  <PositionCardItem
                    label="generic.end"
                    value={
                      <div>
                        <div>{show.dateTime(props.trip.endPosition.date)}</div>
                        <div>
                          <GeocodeReverse
                            coordinates={props.trip.endPosition.coordinates}
                          />
                        </div>
                      </div>
                    }
                  />
                </div>
                <div className="px-6 pb-4 flex space-x-4 border-b border-gray-200">
                  <PositionCardItem
                    label="generic.distance"
                    value={show.distance(props.trip.distance)}
                    className="col-span-1"
                  />
                  <PositionCardItem
                    label="generic.duration"
                    value={show.duration(props.trip.duration)}
                    className="col-span-1"
                  />
                  <PositionCardItem
                    label="generic.average-speed"
                    value={show.speed(props.trip.averageSpeed)}
                    className="col-span-1"
                  />
                  <PositionCardItem
                    label="generic.max-speed"
                    value={show.speed(props.trip.maxSpeed)}
                    className="col-span-1"
                  />
                  <PositionCardItem
                    label="generic.average-fuel-consumption"
                    value={show.fuelConsumption(
                      props.trip.averageFuelConsumption
                    )}
                    className="col-span-1"
                  />
                  <PositionCardItem
                    label="generic.fuel-consumption"
                    value={show.volume(props.trip.fuelConsumption)}
                    className="col-span-1"
                  />
                </div>
                <div
                  className="grow px-6 border-b border-gray-200 flex pb-4"
                  style={{ minHeight: 300 }}>
                  <CardMapWrapper>
                    <Map>
                      <MapTrip
                        trip={props.trip}
                        options={{
                          initialZoom: 15,
                          padding: {
                            top: 10,
                            left: 10,
                            bottom: 10,
                            right: 10
                          }
                        }}
                      />
                      {selectedTripPosition && showPin && (
                        <>
                          <MapPin
                            pin={{
                              coordinates: selectedTripPosition.coordinates
                            }}
                            zIndexOffset={1}
                          />
                          <MapCenter
                            position={selectedTripPosition.coordinates}
                          />
                        </>
                      )}
                    </Map>
                  </CardMapWrapper>
                </div>
                <div className="px-6">
                  <Card>
                    <div className="flex rounded-t-lg border-b border-gray-900/5 bg-gray-50 p-2 text-xs tracking-wider text-gray-500">
                      <div className="font-medium uppercase">
                        <FormattedMessage id="generic.position" />
                      </div>
                      <div className="w-full text-right tracking-normal">
                        <Icon icon={faInfoCircle} />
                        <span className="ml-1">
                          <FormattedMessage id="assets.trips.position.tip" />
                        </span>
                      </div>
                    </div>
                    <div className="flex" style={{ flexBasis: 0 }}>
                      <div className="flex grow bg-white px-3">
                        <Slider
                          step={1}
                          min={0}
                          max={props.trip?.positions.length - 1}
                          value={selectedTripLocationIndex}
                          onChange={(value) =>
                            setSelectedTripLocationIndex(value)
                          }
                          onMouseUp={() => {
                            if (selectedTripPosition) {
                              setReverseGeocodePosition(selectedTripPosition);
                            }
                          }}
                          onMouseDown={() => {
                            if (!showPin) {
                              setShowPin(true);
                            }
                          }}
                        />
                      </div>
                    </div>
                    {selectedTripPosition && (
                      <div className="px-3 py-2">
                        <div className="mb-2 flex justify-between">
                          <PositionCardItems position={selectedTripPosition} />
                        </div>
                        {reverseGeocodePosition !== undefined &&
                          slots?.assetLiveTrackingPositionCardExtraItems?.(
                            reverseGeocodePosition
                          )}
                      </div>
                    )}
                  </Card>
                </div>
              </div>
            </DialogPanel>
          </div>
        </div>
      </div>
    </Dialog>
  );
}
