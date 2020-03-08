import React, { ReactNode } from "react";
import { UserRole } from "services/Api/Model/UserRole";
import { AuthorizationService } from "services/Authentication/AuthorizationService";

type Props = {
  children: ReactNode;
  userRole?: UserRole;
};

export default function Authorize(props: Props) {
  return <>{AuthorizationService.isAuthorized(props.userRole) && props.children}</>;
}
