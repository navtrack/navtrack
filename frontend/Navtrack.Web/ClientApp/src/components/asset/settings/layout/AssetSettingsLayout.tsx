import { ReactNode, useMemo } from "react";
import { faCog, faHdd, faUsers } from "@fortawesome/free-solid-svg-icons";
import SidebarItem, { ISidebarItem } from "./SidebarItem";
import { generatePath } from "react-router";
import Paths from "../../../../app/Paths";
import { useCurrentAsset } from "@navtrack/navtrack-app-shared";

interface IAssetSettingsLayout {
  children?: ReactNode;
}

export default function AssetSettingsLayout(props: IAssetSettingsLayout) {
  const currentAsset = useCurrentAsset();

  const routes: ISidebarItem[] = useMemo(
    () => [
      {
        label: "assets.settings.sidebar.settings",
        href: generatePath(Paths.AssetsSettings, { id: currentAsset?.shortId }),
        icon: faCog
      },
      {
        label: "assets.settings.sidebar.device",
        href: generatePath(Paths.AssetsSettingsDevice, {
          id: currentAsset?.shortId
        }),
        icon: faHdd
      },
      {
        label: "assets.settings.sidebar.access",
        href: generatePath(Paths.AssetsSettingsAccess, {
          id: currentAsset?.shortId
        }),
        icon: faUsers
      }
    ],
    [currentAsset?.shortId]
  );

  return (
    <div className="relative">
      <div className="overflow-hidden rounded-lg bg-white shadow">
        <div className="divide-y divide-gray-200 lg:grid lg:grid-cols-12 lg:divide-y-0 lg:divide-x">
          <aside className="py-6 lg:col-span-3">
            <nav className="space-y-1">
              {routes.map((item) => (
                <SidebarItem key={item.label} {...item} />
              ))}
            </nav>
          </aside>
          <div className="col-span-9 p-6">{props.children}</div>
        </div>
      </div>
    </div>
  );
}
