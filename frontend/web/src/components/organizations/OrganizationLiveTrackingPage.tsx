import { Card } from "../ui/card/Card";
import { useAssetsQuery } from "@navtrack/shared/hooks/queries/assets/useAssetsQuery";
import { CardMapWrapper } from "../ui/map/CardMapWrapper";
import { Map } from "../ui/map/Map";
import { useMemo } from "react";
import { LatLong } from "@navtrack/shared/api/model/generated";
import { StatCountCard } from "../ui/card/StatCountCard";
import { startOfDay } from "date-fns";
import { MapPinLabel } from "../ui/map/MapPinLabel";
import { generatePath, useNavigate } from "react-router-dom";
import { Paths } from "../../app/Paths";
import { MapShowAllControl } from "../ui/map/MapShowAllControl";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";
import { DEFAULT_MAP_CENTER, DEFAULT_MAP_ZOOM } from "../../constants";

export function OrganizationLiveTrackingPage() {
  const navigate = useNavigate();
  const currentOrganization = useCurrentOrganization();
  const assets = useAssetsQuery({
    organizationId: currentOrganization.data?.id
  });

  const assetsWithPosition = useMemo(
    () =>
      assets.data?.items.filter(
        (x) => x.lastPositionMessage?.position !== undefined
      ) ?? [],
    [assets.data?.items]
  );

  const onlineAssets = assets.data?.items.filter((x) => x.online).length ?? 0;
  const offlineAssets = (assets.data?.items.length ?? 0) - onlineAssets;
  const onlineTodayAssets = (
    assets.data?.items.filter(
      (x) =>
        x.lastPositionMessage?.position?.date !== undefined &&
        new Date(x.lastPositionMessage?.position.date) > startOfDay(new Date())
    ) ?? []
  ).length;

  const coordinates = useMemo(
    () =>
      assetsWithPosition.map(
        (x) => x.lastPositionMessage?.position?.coordinates
      ) as LatLong[],
    [assetsWithPosition]
  );

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
          <Map center={DEFAULT_MAP_CENTER} initialZoom={DEFAULT_MAP_ZOOM}>
            {assetsWithPosition.map((asset) => (
              <MapPinLabel
                key={asset.id}
                pin={{
                  coordinates: asset.lastPositionMessage!.position.coordinates,
                  label: asset.name,
                  color: asset.online ? "green" : "primary"
                }}
                onClick={() =>
                  navigate(generatePath(Paths.AssetLive, { id: asset.id }))
                }
              />
            ))}
            <MapShowAllControl
              key={currentOrganization.id}
              coordinates={coordinates}
              options={{
                padding: { left: 60, top: 120, right: 10, bottom: 20 }
              }}
            />
          </Map>
        </CardMapWrapper>
      </Card>
    </>
  );
}
