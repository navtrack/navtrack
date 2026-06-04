import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { Map } from "../../ui/map/Map";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { useMessagesQuery } from "@navtrack/shared/hooks/queries/assets/useMessagesQuery";
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
import { useAtomValue } from "jotai";
import { DEFAULT_MAP_CENTER } from "@navtrack/shared/constants";
import { useTable } from "../../ui/table/useTable";

export function AssetLogPage() {
  const show = useShow();
  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("log");
  const filters = useAtomValue(locationFiltersSelector(locationFilterKey));
  const query = useMessagesQuery({
    assetId: currentAsset.data?.id,
    ...filters
  });

  const table = useTable<DeviceMessageModel>({
    selection: true,
    rows: query.data?.items,
    columns: [
      {
        labelId: "generic.date",
        footer: () => (
          <>
            {query.data?.items !== undefined &&
              query.data?.items.length > 0 &&
              (query.data?.totalCount! > query.data?.items.length! ? (
                <FormattedMessage
                  id="assets.log.table.positions-over"
                  values={{
                    count: (
                      <span className="font-semibold">
                        {query.data?.items.length}
                      </span>
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
                      <span className="font-semibold">
                        {query.data?.items.length}
                      </span>
                    )
                  }}
                />
              ))}
          </>
        ),
        sort: "desc",
        value: (row) => row.position.date,
        row: (row) => show.dateTime(row.position.date)
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
        value: (row) => row.position.altitude
      },
      {
        labelId: "generic.speed",
        row: (row) => show.speed(row.position.speed),
        value: (row) => row.position.speed
      },
      {
        labelId: "generic.heading",
        row: (row) => showHeading(row.position.heading),
        value: (row) => row.position.heading
      },
      {
        labelId: "generic.satellites",
        row: (row) => `${row.position.satellites}`,
        value: (row) => row.position.satellites
      }
    ]
  });

  return (
    <>
      <LocationFilter
        filterPage="log"
        center={table.selectedItem?.position.coordinates}
      />
      <TableV2<DeviceMessageModel> className="flex h-80" {...table.props} />
      <Card className="flex grow">
        <CardMapWrapper style={{ flexGrow: 2, minHeight: 250 }}>
          <Map
            center={
              table.selectedItem
                ? table.selectedItem.position.coordinates
                : DEFAULT_MAP_CENTER
            }>
            {table.selectedItem?.position.coordinates && (
              <MapPin
                pin={{
                  coordinates: table.selectedItem.position.coordinates,
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
