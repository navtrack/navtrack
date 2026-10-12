import { ReactNode } from "react";
import {
  AssetUserRole,
  OrganizationUserRole,
  TeamUserRole
} from "../../api/model";
import { useAuthorize } from "../../hooks/current/useAuthorize";

type AuthorizeProps = {
  assetUserRole?: AssetUserRole;
  organizationUserRole?: OrganizationUserRole;
  teamUserRole?: TeamUserRole;
  children?: ReactNode;
};

export function Authorize(props: AuthorizeProps) {
  const authorize = useAuthorize();

  if (
    props.assetUserRole !== undefined &&
    authorize.asset(props.assetUserRole)
  ) {
    return props.children;
  }

  if (
    props.organizationUserRole !== undefined &&
    authorize.organization(props.organizationUserRole)
  ) {
    return props.children;
  }

  if (props.teamUserRole !== undefined && authorize.team(props.teamUserRole)) {
    return props.children;
  }

  return null;
}
