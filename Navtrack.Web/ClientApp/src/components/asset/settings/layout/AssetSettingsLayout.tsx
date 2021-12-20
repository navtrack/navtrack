import { ReactNode, useMemo } from "react";
import { faCog, faUsers } from "@fortawesome/free-solid-svg-icons";
import useCurrentAsset from "../../../../hooks/assets/useCurrentAsset";
import SidebarItem, { ISidebarItem } from "./SidebarItem";
import { useIntl } from "react-intl";
import { generatePath } from "react-router";
import Paths from "../../../../app/Paths";

interface IAssetSettingsLayout {
  children?: ReactNode;
}

export default function AssetSettingsLayout(props: IAssetSettingsLayout) {
  const { currentAsset } = useCurrentAsset();
  const intl = useIntl();

  const routes: ISidebarItem[] = useMemo(
    () => [
      {
        label: intl.formatMessage({ id: "assets.settings.sidebar.settings" }),
        href: generatePath(Paths.AssetSettings, { id: currentAsset?.shortId }),
        icon: faCog
      },
      {
        label: intl.formatMessage({ id: "assets.settings.sidebar.access" }),
        href: generatePath(Paths.AssetSettingsAccess, {
          id: currentAsset?.shortId
        }),
        icon: faUsers
      }
    ],
    [currentAsset?.shortId, intl]
  );

  return (
    <div className="relative">
      <div className="bg-white rounded-lg shadow overflow-hidden">
        <div className="divide-y divide-gray-200 lg:grid lg:grid-cols-12 lg:divide-y-0 lg:divide-x">
          <aside className="py-6 lg:col-span-3">
            <nav className="space-y-1">
              {routes.map((item) => (
                <SidebarItem key={item.label} {...item} />
              ))}
            </nav>
          </aside>
          <div className="col-span-9">{props.children}</div>
        </div>
      </div>
    </div>
  );
}
