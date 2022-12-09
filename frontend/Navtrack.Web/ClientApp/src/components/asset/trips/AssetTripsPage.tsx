import { faInfoCircle } from "@fortawesome/free-solid-svg-icons";
import { useState } from "react";
import { FormattedMessage } from "react-intl";
import { useRecoilState, useRecoilValue } from "recoil";
import LocationFilter from "../shared/location-filter/LocationFilter";
import Card from "../../ui/shared/card/Card";
import Icon from "../../ui/shared/icon/Icon";
import Map from "../../ui/shared/map/Map";
import MapPin from "../../ui/shared/map/MapPin";
import MapTrip from "../../ui/shared/map/MapTrip";
import Slider from "../../ui/shared/slider/Slider";
import LocationBar from "../shared/location-bar/LocationBar";
import {
  selectedTripLocationIndexAtom,
  selectedTripLocationSelector,
  selectedTripSelector
} from "./state";
import TripsTable from "./TripsTable";

export default function AssetTripsPage() {
  const selectedTrip = useRecoilValue(selectedTripSelector);
  const selectedTripLocation = useRecoilValue(selectedTripLocationSelector);
  const [selectedTripLocationIndex, setSelectedTripLocationIndex] =
    useRecoilState(selectedTripLocationIndexAtom);
  const [showPin, setShowPin] = useState(false);

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
              <div className="flex flex-grow bg-white  px-3">
                <Slider
                  step={1}
                  marks
                  min={1}
                  max={selectedTrip?.locations.length}
                  displayValueLabel="auto"
                  value={selectedTripLocationIndex}
                  onChange={(_, index) => {
                    setSelectedTripLocationIndex(index as number);
                  }}
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
          <div className="flex" style={{ flexGrow: 2, minHeight: "200px" }}>
            <div className="flex flex-grow rounded-lg bg-white shadow">
              <Map
                center={{
                  latitude: selectedTrip.startLocation.latitude,
                  longitude: selectedTrip.startLocation.longitude
                }}
                zoom={16}>
                <MapTrip trip={selectedTrip} />
                {selectedTripLocation && showPin && (
                  <MapPin
                    latitude={selectedTripLocation.latitude}
                    longitude={selectedTripLocation.longitude}
                    zIndexOffset={1}
                  />
                )}
              </Map>
            </div>
          </div>
        </>
      )}
    </>
  );
}
