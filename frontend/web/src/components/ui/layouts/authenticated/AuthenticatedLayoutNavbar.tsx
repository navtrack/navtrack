import { NavLink } from "react-router-dom";
import { Paths } from "../../../../app/Paths";
import { AuthenticatedLayoutNavbarProfile } from "./AuthenticatedLayoutNavbarProfile";
import { NavtrackLogoDark } from "../../logo/NavtrackLogoDark";
import { AuthenticatedLayoutNavbarItem } from "./AuthenticatedLayoutNavbarItem";
import { FormattedMessage } from "react-intl";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { NavbarOrganization } from "./NavbarOrganization";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";
import { useContext, useMemo } from "react";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { NavbarBreadcrumbs } from "./NavbarBreadcrumbs";
import {
  AssetUserRole,
  OrganizationUserRole
} from "@navtrack/shared/api/model";
import { SlotContext } from "../../../../app/SlotContext";
import { useAuthorize } from "@navtrack/shared/hooks/current/useAuthorize";
import { useNavbarAssetMenuItems } from "./useNavbarAssetMenuItems";
import { useNavbarOrganizationMenuItems } from "./useNavbarOrganizationMenuItems";

type AuthenticatedLayoutNavbarProps = {
  hideLogo?: boolean;
};

export type NavbarMenuItem = {
  label: string;
  path: string;
  icon: IconProp;
  order: number;
  count?: number;
  organizationRole?: OrganizationUserRole;
  assetRole?: AssetUserRole;
  subMenuItems?: NavbarMenuItem[];
};

export function AuthenticatedLayoutNavbar(
  props: AuthenticatedLayoutNavbarProps
) {
  const currentAsset = useCurrentAsset();
  const currentOrganization = useCurrentOrganization();
  const slots = useContext(SlotContext);
  const authorize = useAuthorize();
  const assetMenuItems = useNavbarAssetMenuItems();
  const organizationMenuItems = useNavbarOrganizationMenuItems();

  const menuItems = useMemo(
    () =>
      currentAsset.id !== undefined
        ? assetMenuItems.filter(
            (item) =>
              item.assetRole === undefined || authorize.asset(item.assetRole)
          )
        : currentOrganization.id !== undefined
          ? organizationMenuItems.filter(
              (item) =>
                item.organizationRole === undefined ||
                authorize.organization(item.organizationRole)
            )
          : [],
    [
      assetMenuItems,
      authorize,
      currentAsset.id,
      currentOrganization.id,
      organizationMenuItems
    ]
  );

  return (
    <div className="relative bg-white px-6 shadow">
      <div
        className={classNames(
          "flex h-14 flex-1 justify-between",
          c(menuItems.length > 0, "border-b border-gray-200")
        )}>
        <div className="flex items-center space-x-2 text-sm">
          {!props.hideLogo && (
            <div className="-ml-2 flex h-14 w-64 items-center">
              <NavLink to={Paths.Home} className="flex items-center">
                <NavtrackLogoDark className="h-10 w-10 p-2" />
                <span className="ml-2 text-2xl font-semibold tracking-wide text-gray-900">
                  <FormattedMessage id="navtrack" />
                </span>
              </NavLink>
            </div>
          )}
          <NavbarBreadcrumbs />
        </div>
        <div className="flex items-center space-x-4">
          {slots?.navbarAdditional}
          <NavbarOrganization />
          {/*<button
            type="button"
            className="relative h-8 w-8 rounded-full bg-white p-1 text-gray-900 hover:text-gray-500 focus:outline-none focus:ring-2 focus:ring-blue-700 focus:ring-offset-2">
            <Icon icon={faBell} size="lg" />
          </button> */}
          <AuthenticatedLayoutNavbarProfile />
        </div>
      </div>
      {menuItems.length > 0 && (
        <div className="flex h-14 space-x-8">
          {menuItems.map((item) => {
            return <AuthenticatedLayoutNavbarItem {...item} key={item.path} />;
          })}
        </div>
      )}
    </div>
  );
}
