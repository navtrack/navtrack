import React, { ReactNode } from "react";
import { AuthorizationService } from "services/authentication/AuthorizationService";
import { UserRole } from "services/api/types/user/UserRole";

type Props = {
  children: ReactNode;
  userRole?: UserRole;
};

export default function Authorize(props: Props) {
  return <>{AuthorizationService.isAuthorized(props.userRole) && props.children}</>;
}
