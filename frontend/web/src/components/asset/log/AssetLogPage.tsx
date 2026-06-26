import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { Map } from "../../ui/map/Map";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { useMessagesQuery } from "@navtrack/shared/hooks/queries/assets/useMessagesQuery";
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
import { DEFAULT_MAP_CENTER } from "@navtrack/shared/constants";
import { useTable } from "../../ui/table/useTable";
import { useLocationFilter } from "../shared/location-filter/useLocationFilter";
import { LocationFilterType } from "../shared/location-filter/locationFilterTypes";
import { GeocodeReverse } from "@navtrack/shared/components/components/geo/GeocodeReverse";

export function AssetLogPage() {
  const show = useShow();
  const currentAsset = useCurrentAsset();
  const filter = useLocationFilter({
    page: "asset-log",
    filters: [
      LocationFilterType.Altitude,
      LocationFilterType.Geofence,
      LocationFilterType.AvgSpeed,
      LocationFilterType.Speed
    ]
  });
  const query = useMessagesQuery({
    assetId: currentAsset.data?.id,
    ...filter.filters
  });

  const table = useTable<DeviceMessageModel>({
    selection: true,
    rows: query.data?.items,
    columns: [
      {
        labelId: "date",
        footerColSpan: 2,
        rowClassName: "text-nowrap",
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
        labelId: "location",
        footer: null,
        row: (item) => (
          <div className="text-ellipsis">
            <div>
              <GeocodeReverse coordinates={item.position.coordinates} />
            </div>
          </div>
        )
      },
      {
        labelId: "latitude",
        row: (row) => showCoordinate(row.position.coordinates.latitude)
      },
      {
        labelId: "longitude",
        row: (row) => showCoordinate(row.position.coordinates.longitude)
      },
      {
        labelId: "altitude",
        row: (row) => show.altitude(row.position.altitude),
        value: (row) => row.position.altitude
      },
      {
        labelId: "speed",
        row: (row) => show.speed(row.position.speed),
        value: (row) => row.position.speed
      },
      {
        labelId: "heading",
        row: (row) => showHeading(row.position.heading),
        value: (row) => row.position.heading
      },
      {
        labelId: "satellites",
        row: (row) => `${row.position.satellites}`,
        value: (row) => row.position.satellites
      }
    ]
  });

  return (
    <>
      <LocationFilter configuration={filter.configuration} />
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
