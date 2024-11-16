import { faHome } from "@fortawesome/free-solid-svg-icons";
import { NavLink, generatePath } from "react-router-dom";
import { Paths } from "../../../../app/Paths";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";
import { useEffect, useState } from "react";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { Icon } from "../../icon/Icon";
import { useCurrentTeam } from "@navtrack/shared/hooks/current/useCurrentTeam";
import { c, classNames } from "@navtrack/shared/utils/tailwind";

type BreadcrumbItem = {
  name: string;
  href: string;
};

export function NavbarBreadcrumbs() {
  const currentAsset = useCurrentAsset();
  const currentOrganization = useCurrentOrganization();
  const currentTeam = useCurrentTeam();
  const [breadcrumbs, setBreadcrumbs] = useState<BreadcrumbItem[]>([]);

  useEffect(() => {
    const a = [];

    if (currentOrganization.data) {
      a.push({
        name: currentOrganization.data.name,
        href: generatePath(Paths.OrganizationLive, {
          id: currentOrganization.data.id
        })
      });
    }

    if (currentAsset.data) {
      a.push({
        name: currentAsset.data.name,
        href: generatePath(Paths.AssetLive, { id: currentAsset.data.id })
      });
    }

    if (currentTeam.data && currentOrganization.id) {
      a.push({
        name: "Teams",
        href: generatePath(Paths.OrganizationTeams, {
          id: currentOrganization.id
        })
      });
      a.push({
        name: currentTeam.data.name,
        href: generatePath(Paths.TeamUsers, { id: currentTeam.data.id })
      });
    }

    setBreadcrumbs(a);
  }, [
    currentAsset.data,
    currentOrganization.data,
    currentOrganization.id,
    currentTeam.data
  ]);

  return (
    <>
      {breadcrumbs.length > 0 && (
        <NavLink
          to={Paths.Organizations}
          className="rounded-md px-2 py-1 text-gray-900 hover:bg-gray-100">
          <Icon icon={faHome} />
        </NavLink>
      )}
      {breadcrumbs.map((page, i) => (
        <div className="flex items-center" key={page.name}>
          <span className="text-base">/</span>
          <NavLink
            to={page.href}
            className={classNames(
              "ml-2 rounded-md px-2 py-1 text-gray-900 hover:bg-gray-100",
              c(i === breadcrumbs.length - 1, "font-semibold")
            )}>
            {page.name}
          </NavLink>
        </div>
      ))}
    </>
  );
}
