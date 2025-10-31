import { useState } from "react";
import { useAtomValue } from "jotai";
import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { Card } from "../../ui/card/Card";
import { Map } from "../../ui/map/Map";
import { MapTrip } from "../../ui/map/MapTrip";
import { CardMapWrapper } from "../../ui/map/CardMapWrapper";
import { TableV2 } from "../../ui/table/TableV2";
import { TripModel } from "@navtrack/shared/api/model";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { useTripsQuery } from "@navtrack/shared/hooks/queries/assets/useTripsQuery";
import { locationFiltersSelector } from "../shared/location-filter/locationFilterState";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { GeocodeReverse } from "@navtrack/shared/components/components/geo/GeocodeReverse";
import { AssetTripDetailsPanel } from "./AssetTripDetailsPanel";
import { Button } from "../../ui/button/Button";
import { faMagnifyingGlass } from "@fortawesome/free-solid-svg-icons";

export function AssetTripsPage() {
  const show = useShow();
  const [selectedTrip, setSelectedTrip] = useState<TripModel>();
  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("trips");
  const filters = useAtomValue(locationFiltersSelector(locationFilterKey));
  const query = useTripsQuery({ assetId: currentAsset.data?.id, ...filters });
  const hasTrips = (query.data?.items.length ?? 0) > 0;
  const [showTripPanel, setShowTripPanel] = useState(false);

  return (
    <>
      <LocationFilter filterPage="trips" duration avgAltitude avgSpeed />
      {selectedTrip && (
        <AssetTripDetailsPanel
          trip={selectedTrip}
          open={showTripPanel}
          close={() => setShowTripPanel(false)}
        />
      )}
      <div>
        <TableV2<TripModel>
          columns={[
            {
              labelId: "generic.start",
              sort: "desc",
              sortValue: (row) => row.startPosition.date,
              row: (row) => (
                <div>
                  <div>{show.dateTime(row.startPosition.date)}</div>
                  <div>
                    <GeocodeReverse
                      coordinates={row.startPosition.coordinates}
                    />
                  </div>
                </div>
              ),
              sortable: true
            },
            {
              labelId: "generic.end",
              sortValue: (row) => row.endPosition.date,
              row: (row) => (
                <div>
                  <div>{show.dateTime(row.endPosition.date)}</div>
                  <div>
                    <GeocodeReverse coordinates={row.endPosition.coordinates} />
                  </div>
                </div>
              ),
              sortable: true,
              footerColSpan: 2
            },
            {
              labelId: "generic.duration",
              row: (row) => show.duration(row.duration),
              sortValue: (row) => row.duration,
              footerClassName: "font-semibold",
              footer: () =>
                hasTrips && show.duration(query.data?.totalDuration),
              sortable: true
            },
            {
              labelId: "generic.distance",
              row: (row) => show.distance(row.distance),
              sortValue: (row) => row.distance,
              footerClassName: "font-semibold",
              footer: () =>
                hasTrips && show.distance(query.data?.totalDistance),
              sortable: true
            },
            {
              labelId: "generic.max-speed",
              row: (row) => show.speed(row.maxSpeed),
              sortValue: (row) => row.maxSpeed,
              footerClassName: "font-semibold",
              footer: () => hasTrips && show.speed(query.data?.maxSpeed),
              sortable: true
            },
            {
              labelId: "generic.positions",
              row: (row) => row.positions.length,
              sortValue: (row) => row.positions.length,
              footerClassName: "font-semibold",
              footer: () => hasTrips && query.data?.totalPositions,
              sortable: true
            },
            {
              row: (row) => (
                <>
                  <Button
                    icon={faMagnifyingGlass}
                    color="white"
                    size="sm"
                    onClick={(e) => {
                      e.stopPropagation();
                      setShowTripPanel(true);
                    }}
                  />
                </>
              ),
              footer: () => <></>
            }
          ]}
          rows={query.data?.items}
          setSelectedItem={(trip) => {
            setSelectedTrip(trip);
          }}
          className="flex h-80 grow"
        />
      </div>
      <Card className="flex grow">
        <CardMapWrapper style={{ flexGrow: 2, minHeight: 250 }}>
          <Map>
            <MapTrip
              trip={selectedTrip}
              options={{
                initialZoom: 15,
                padding: { top: 20, left: 20, bottom: 10, right: 20 }
              }}
            />
          </Map>
        </CardMapWrapper>
      </Card>
    </>
  );
}
