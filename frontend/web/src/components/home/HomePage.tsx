import { Card } from "../ui/card/Card";
import { useAssetsQuery } from "@navtrack/shared/hooks/queries/useAssetsQuery";
import { CardMapWrapper } from "../ui/map/CardMapWrapper";
import { MapPin } from "../ui/map/MapPin";
import { Map } from "../ui/map/Map";
import { MapFitBounds } from "../ui/map/MapFitBounds";
import { useMemo } from "react";
import { LatLongModel } from "@navtrack/shared/api/model/generated";
import { StatCountCard } from "../ui/card/StatCountCard";
import { startOfDay } from "date-fns";
import { Skeleton } from "../ui/skeleton/Skeleton";

export function HomePage() {
  const assets = useAssetsQuery();

  const assetsWithPosition = useMemo(
    () => assets.data?.items.filter((x) => x.position !== undefined) ?? [],
    [assets.data?.items]
  );

  const coordinates = useMemo(
    () =>
      assetsWithPosition.map((x) => x.position?.coordinates) as LatLongModel[],
    [assetsWithPosition]
  );

  const onlineAssets = assets.data?.items.filter((x) => x.online).length ?? 0;
  const offlineAssets = (assets.data?.items.length ?? 0) - onlineAssets;
  const onlineTodayAssets = (
    assets.data?.items.filter(
      (x) =>
        x.position?.date !== undefined &&
        new Date(x.position.date) > startOfDay(new Date())
    ) ?? []
  ).length;

  return (
    <>
      <div>
        <div className="grid grid-cols-3 gap-5">
          <StatCountCard
            labelId="assets.online"
            count={onlineAssets}
            totalCount={assets.data?.items.length}
            loading={assets.isLoading}
          />
          <StatCountCard
            labelId="assets.online-today"
            count={onlineTodayAssets}
            totalCount={assets.data?.items.length}
            loading={assets.isLoading}
          />
          <StatCountCard
            labelId="assets.offline"
            count={offlineAssets}
            totalCount={assets.data?.items.length}
            loading={assets.isLoading}
          />
        </div>
      </div>
      <Card className="flex flex-grow">
        <CardMapWrapper>
          <Map center={{ latitude: 46.77689, longitude: 23.601674 }}>
            {assetsWithPosition.map((asset) => (
              <MapPin
                key={asset.id}
                coordinates={asset.position!.coordinates}
              />
            ))}
            <MapFitBounds coordinates={coordinates} />
          </Map>
        </CardMapWrapper>
      </Card>
    </>
  );
}
