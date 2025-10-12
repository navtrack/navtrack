import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { Map } from "../../ui/map/Map";
import { DEFAULT_MAP_CENTER } from "../../../constants";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { useMessagesQuery } from "@navtrack/shared/hooks/queries/assets/useMessagesQuery";
import { useState } from "react";
import { locationFiltersSelector } from "../shared/location-filter/locationFilterState";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";
import { Card } from "../../ui/card/Card";
import { TableV2 } from "../../ui/table/TableV2";
import { DeviceMessageModel } from "@navtrack/shared/api/model";
import { FormattedMessage } from "react-intl";
import {
  showCoordinate,
  showHeading
} from "@navtrack/shared/utils/coordinates";
import { CardMapWrapper } from "../../ui/map/CardMapWrapper";
import { MapPin } from "../../ui/map/MapPin";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { useAtom } from "jotai";

export function AssetLogPage() {
  const show = useShow();
  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("log");
  const filters = useAtom(locationFiltersSelector(locationFilterKey));
  const query = useMessagesQuery({
    assetId: currentAsset.data?.id,
    ...filters
  });

  const [message, setMessage] = useState<DeviceMessageModel | undefined>(
    undefined
  );

  return (
    <>
      <LocationFilter filterPage="log" center={message?.position.coordinates} />
      <TableV2<DeviceMessageModel>
        columns={[
          {
            labelId: "generic.date",
            rowClassName: "py-0.5",
            footerColSpan: 7,
            footer: (rows) => (
              <>
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
              </>
            ),
            sort: "desc",
            sortValue: (row) => row.position.date,
            row: (row) => show.dateTime(row.position.date),
            sortable: true
          },
          {
            labelId: "generic.latitude",
            row: (row) => showCoordinate(row.position.coordinates.latitude)
          },
          {
            labelId: "generic.longitude",
            row: (row) => showCoordinate(row.position.coordinates.longitude)
          },
          {
            labelId: "generic.altitude",
            row: (row) => show.altitude(row.position.altitude),
            sortValue: (row) => row.position.altitude,
            sortable: true
          },
          {
            labelId: "generic.speed",
            row: (row) => show.speed(row.position.speed),
            sortValue: (row) => row.position.speed,
            sortable: true
          },
          {
            labelId: "generic.heading",
            row: (row) => showHeading(row.position.heading),
            sortValue: (row) => row.position.heading,
            sortable: true
          },
          {
            labelId: "generic.satellites",
            row: (row) => `${row.position.satellites}`,
            sortValue: (row) => row.position.satellites,
            sortable: true
          }
        ]}
        rows={query.data?.items}
        setSelectedItem={setMessage}
        className="flex h-44 flex-grow"
      />
      <Card className="flex flex-grow">
        <CardMapWrapper>
          <Map
            center={message ? message.position.coordinates : DEFAULT_MAP_CENTER}
            initialZoom={14}>
            {message?.position.coordinates && (
              <MapPin
                pin={{
                  coordinates: message?.position.coordinates,
                  follow: true
                }}
              />
            )}
          </Map>
        </CardMapWrapper>
      </Card>
    </>
  );
}
