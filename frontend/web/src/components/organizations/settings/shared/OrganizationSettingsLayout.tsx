import { useContext, useMemo } from "react";
import { faCog } from "@fortawesome/free-solid-svg-icons";
import { generatePath, Outlet } from "react-router-dom";
import { Paths } from "../../../../app/Paths";
import { SettingsLayout } from "../../../ui/layouts/settings/SettingsLayout";
import { SettingsMenuItemProps } from "../../../ui/layouts/settings/SettingsMenuItem";
import { AuthenticatedLayoutTwoColumns } from "../../../ui/layouts/authenticated/AuthenticatedLayoutTwoColumns";
import { SlotContext } from "../../../../app/SlotContext";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";

export const defaultOrganizationSettingsMenuItems: SettingsMenuItemProps[] = [
  {
    label: "assets.settings.sidebar.settings",
    path: Paths.OrganizationSettings,
    icon: faCog,
    priority: 100
  }
];

export function OrganizationSettingsLayout() {
  const currentOrganization = useCurrentOrganization();
  const slots = useContext(SlotContext);

  const menuItems = useMemo(() => {
    return !!currentOrganization.id
      ? (
          slots?.organizationSettingsMenuItems ??
          defaultOrganizationSettingsMenuItems
        ).map((item) => ({
          ...item,
          path: generatePath(item.path, { id: currentOrganization.id })
        }))
      : [];
  }, [currentOrganization.id, slots?.organizationSettingsMenuItems]);

  return (
    <AuthenticatedLayoutTwoColumns>
      <div className="flex gap-x-6">
        <SettingsLayout menuItems={menuItems}>
          <Outlet />
        </SettingsLayout>
      </div>
    </AuthenticatedLayoutTwoColumns>
  );
}
