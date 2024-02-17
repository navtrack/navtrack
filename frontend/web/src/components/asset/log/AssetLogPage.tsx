import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { Map } from "../../ui/map/Map";
import { MapPin } from "../../ui/map/MapPin";
import { DEFAULT_MAP_CENTER } from "../../../constants";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { usePositionsQuery } from "@navtrack/shared/hooks/queries/usePositionsQuery";
import { useState } from "react";
import { useRecoilValue } from "recoil";
import { locationFiltersSelector } from "../shared/location-filter/locationFilterState";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";
import { MapContainer } from "../../ui/map/MapContainer";
import { Card } from "../../ui/card/Card";
import { TableV2 } from "../../ui/table/TableV2";
import { PositionModel } from "@navtrack/shared/api/model/generated";
import { useDateTime } from "@navtrack/shared/hooks/util/useDateTime";
import { useDistance } from "@navtrack/shared/hooks/util/useDistance";
import { FormattedMessage } from "react-intl";
import {
  showCoordinate,
  showHeading
} from "@navtrack/shared/utils/coordinates";

export function AssetLogPage() {
  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("log");
  const filters = useRecoilValue(locationFiltersSelector(locationFilterKey));
  const query = usePositionsQuery({
    assetId: currentAsset.data?.id,
    ...filters
  });

  const { showDateTime } = useDateTime();
  const { showSpeed, showAltitude } = useDistance();

  const [position, setPosition] = useState<PositionModel | undefined>(
    undefined
  );

  return (
    <>
      <LocationFilter
        filterPage="log"
        center={
          position
            ? {
                latitude: position.latitude,
                longitude: position.longitude
              }
            : undefined
        }
      />
      <TableV2<PositionModel>
        columns={[
          {
            labelId: "generic.date",
            rowClassName: "py-0.5",
            footerColSpan: 7,
            footer: (rows) => (
              <div className="h-4">
                {rows !== undefined &&
                  rows.length > 0 &&
                  (query.data?.totalCount! > query.data?.items.length! ? (
                    <FormattedMessage
                      id="assets.log.table.positions-over"
                      values={{
                        count: (
                          <span className="font-semibold">{rows.length}</span>
                        ),
                        total: (
                          <span className="font-semibold">
                            {query.data?.totalCount}
                          </span>
                        )
                      }}
                    />
                  ) : (
                    <FormattedMessage
                      id="assets.log.table.positions"
                      values={{
                        count: (
                          <span className="font-semibold">{rows.length}</span>
                        )
                      }}
                    />
                  ))}
              </div>
            ),
            sort: "desc",
            value: (row) => row.dateTime,
            render: (row) => showDateTime(row.dateTime),
            sortable: true
          },
          {
            labelId: "generic.latitude",
            render: (row) => showCoordinate(row.latitude)
          },
          {
            labelId: "generic.longitude",
            render: (row) => showCoordinate(row.longitude)
          },
          {
            labelId: "generic.altitude",
            render: (row) => showAltitude(row.altitude),
            value: (row) => row.altitude,
            sortable: true
          },
          {
            labelId: "generic.speed",
            render: (row) => showSpeed(row.speed),
            value: (row) => row.speed,
            sortable: true
          },
          {
            labelId: "generic.heading",
            render: (row) => showHeading(row.heading),
            value: (row) => row.heading,
            sortable: true
          },
          {
            labelId: "generic.satellites",
            render: (row) => `${row.satellites}`,
            value: (row) => row.satellites,
            sortable: true
          }
        ]}
        rows={query.data?.items}
        setSelectedItem={setPosition}
        className="flex h-44 flex-grow"
      />
      <Card className="flex flex-grow">
        <MapContainer>
          <Map
            center={
              position
                ? {
                    latitude: position.latitude,
                    longitude: position.longitude
                  }
                : DEFAULT_MAP_CENTER
            }
            initialZoom={16}>
            <MapPin position={position} follow />
          </Map>
        </MapContainer>
      </Card>
    </>
  );
}
