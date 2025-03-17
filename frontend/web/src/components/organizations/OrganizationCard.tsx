import { faBuilding, faHdd, faUser } from "@fortawesome/free-solid-svg-icons";
import { useCurrentUserQuery } from "@navtrack/shared/hooks/queries/user/useCurrentUserQuery";
import { NavLink, generatePath } from "react-router-dom";
import { Paths } from "../../app/Paths";
import { Badge, BadgeColor } from "../ui/badge/Badge";
import { Card } from "../ui/card/Card";
import { Icon } from "../ui/icon/Icon";
import { Organization } from "@navtrack/shared/api/model";

type OrganizationCardProps = {
  organization: Organization;
};

export function OrganizationCard(props: OrganizationCardProps) {
  const currentUser = useCurrentUserQuery();

  const userOrganization = currentUser.data?.organizations?.find(
    (userOrganization) =>
      userOrganization.organizationId === props.organization.id
  );

  return (
    <NavLink
      to={generatePath(Paths.OrganizationLive, {
        id: props.organization.id
      })}>
      <Card className="flex flex-col cursor-pointer p-4 text-gray-900 hover:bg-gray-50 h-28">
        <div className="flex flex-1">
          <div className="h-7">
            <Icon icon={faBuilding} />
          </div>
          <div className="flex-1 text-lg font-semibold mx-2 h-14 line-clamp-2">
            {props.organization.name}
          </div>
          <div>
            <Badge color={BadgeColor.Blue}>{userOrganization?.userRole}</Badge>
          </div>
        </div>
        <div className="flex space-x-4 text-sm text-gray-500">
          <div>
            <Icon icon={faHdd} className="mr-1" />
            {props.organization.assetsCount}
          </div>
          <div>
            <Icon icon={faUser} className="mr-1" />
            {props.organization.usersCount}
          </div>
        </div>
      </Card>
    </NavLink>
  );
}
