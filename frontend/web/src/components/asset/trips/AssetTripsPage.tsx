import { faInfoCircle } from "@fortawesome/free-solid-svg-icons";
import { Fragment, useState } from "react";
import { FormattedMessage } from "react-intl";
import { useRecoilState, useRecoilValue } from "recoil";
import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { Card } from "../../ui/card/Card";
import { Icon } from "../../ui/icon/Icon";
import { Map } from "../../ui/map/Map";
import { MapPin } from "../../ui/map/MapPin";
import { MapTrip } from "../../ui/map/MapTrip";
import { Slider } from "../../ui/slider/Slider";
import { LocationBar } from "../shared/location-bar/LocationBar";
import {
  selectedTripPositionIndexAtom,
  selectedTripPositionSelector,
  selectedTripSelector
} from "./tripsState";
import { TripsTable } from "./TripsTable";
import { useOnChange } from "@navtrack/shared/hooks/util/useOnChange";
import { MapContainer } from "../../ui/map/MapContainer";

export function AssetTripsPage() {
  const selectedTrip = useRecoilValue(selectedTripSelector);
  const selectedTripLocation = useRecoilValue(selectedTripPositionSelector);
  const [selectedTripLocationIndex, setSelectedTripLocationIndex] =
    useRecoilState(selectedTripPositionIndexAtom);
  const [showPin, setShowPin] = useState(false);

  useOnChange(selectedTrip, () => {
    setShowPin(false);
  });

  return (
    <>
      <LocationFilter filterPage="trips" duration avgAltitude avgSpeed />
      <TripsTable />
      {selectedTrip !== undefined && (
        <>
          <Card>
            <div className="flex rounded-t-lg border-b bg-gray-50 p-2 text-xs tracking-wider text-gray-500">
              <div className="font-medium uppercase">
                <FormattedMessage id="generic.location" />
              </div>
              <div className="w-full text-right tracking-normal">
                <Icon icon={faInfoCircle} />
                <span className="ml-1">
                  <FormattedMessage id="assets.trips.location.tip" />
                </span>
              </div>
            </div>
            <div className="flex" style={{ flexBasis: 0 }}>
              <div className="flex flex-grow bg-white px-3">
                <Slider
                  step={1}
                  min={1}
                  max={selectedTrip?.positions.length}
                  value={selectedTripLocationIndex}
                  onChange={(value) => setSelectedTripLocationIndex(value)}
                  onMouseDown={() => {
                    if (!showPin) {
                      setShowPin(true);
                    }
                  }}
                />
              </div>
            </div>
            <div className="p-2">
              <LocationBar location={selectedTripLocation} />
            </div>
          </Card>
          <Card className="flex flex-grow">
            <MapContainer style={{ flexGrow: 2, minHeight: "200px" }}>
              <Map
                center={{
                  latitude: selectedTrip.startPosition.latitude,
                  longitude: selectedTrip.startPosition.longitude
                }}
                initialZoom={16}>
                <MapTrip trip={selectedTrip} />
                {selectedTripLocation && showPin && (
                  <MapPin
                    position={{ ...selectedTripLocation }}
                    zIndexOffset={1}
                  />
                )}
              </Map>
            </MapContainer>
          </Card>
        </>
      )}
    </>
  );
}
