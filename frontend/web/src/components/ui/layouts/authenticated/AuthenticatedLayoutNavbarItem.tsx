import { FormattedMessage } from "react-intl";
import { IconWithText } from "../../icon/IconWithText";
import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { generatePath, NavLink, useLocation } from "react-router-dom";
import { BadgeFlatPill } from "../../badge/BadgeFlatPill";
import { Menu, MenuButton, MenuItem, MenuItems } from "@headlessui/react";
import { ZINDEX_MENU } from "../../../../constants";
import { NavbarMenuItem } from "./AuthenticatedLayoutNavbar";
import { useCallback, useMemo } from "react";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";

export function AuthenticatedLayoutNavbarItem(props: NavbarMenuItem) {
  const currentAsset = useCurrentAsset();
  const currentOrganization = useCurrentOrganization();
  const location = useLocation();

  const getPath = useCallback(
    (path: string) => {
      const isAsset = path.includes("/assets/:id");
      const isOrganization = path.includes("/orgs/:id");

      return generatePath(path, {
        id: `${
          isAsset
            ? currentAsset.id
            : isOrganization
              ? currentOrganization.id
              : undefined
        }`
      });
    },
    [currentAsset.id, currentOrganization.id]
  );

  const subMenuIsActive = useMemo(
    () =>
      props.subMenuItems?.some((item) => {
        let path = getPath(item.path);

        return location.pathname.includes(path);
      }) ?? false,
    [getPath, location.pathname, props.subMenuItems]
  );

  const NavMenuItem = (props: NavbarMenuItem & { isActive: boolean }) => (
    <div
      className={classNames(
        c(
          props.isActive,
          "border-gray-900 text-gray-900 hover:border-gray-900",
          "border-transparent hover:border-gray-300"
        ),
        "inline-flex items-center whitespace-nowrap border-b-2 px-1 pt-1 text-sm font-medium text-gray-900"
      )}>
      <IconWithText icon={props.icon}>
        <FormattedMessage id={props.label} />
      </IconWithText>
      {props.count !== undefined && (
        <BadgeFlatPill className="ml-2">{props.count}</BadgeFlatPill>
      )}
    </div>
  );

  if (props.subMenuItems) {
    return (
      <Menu as="div" className="relative h-14 text-left">
        <MenuButton className="h-14 inline-flex">
          <NavMenuItem {...props} isActive={subMenuIsActive} />
        </MenuButton>
        <MenuItems
          transition
          className="absolute left-0 mt-2 w-56 origin-top-right rounded-md bg-white shadow-lg ring-1 ring-black/5 transition focus:outline-none data-closed:scale-95 data-closed:transform data-closed:opacity-0 data-enter:duration-100 data-leave:duration-75 data-enter:ease-out data-leave:ease-in"
          style={{ zIndex: ZINDEX_MENU }}>
          <div className="py-1">
            {props.subMenuItems.map((item) => (
              <MenuItem key={item.path}>
                <NavLink
                  to={getPath(item.path)}
                  className="block px-4 py-2 text-sm text-gray-700 data-focus:bg-gray-100 data-focus:text-gray-900 data-focus:outline-none">
                  <IconWithText icon={item.icon}>
                    <FormattedMessage id={item.label} />
                  </IconWithText>
                </NavLink>
              </MenuItem>
            ))}
          </div>
        </MenuItems>
      </Menu>
    );
  }

  return (
    <NavLink to={getPath(props.path)} className="flex">
      {(navLinkRenderProps) => (
        <NavMenuItem {...props} isActive={navLinkRenderProps.isActive} />
      )}
    </NavLink>
  );
}
