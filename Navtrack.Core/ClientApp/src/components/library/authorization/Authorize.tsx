import React, { ReactNode } from "react";
import { AuthorizationService } from "framework/authentication/AuthorizationService";
import { UserRole } from "apis/types/user/UserRole";

type Props = {
  children: ReactNode;
  userRole?: UserRole;
};

export default function Authorize(props: Props) {
  return <>{AuthorizationService.isAuthorized(props.userRole) && props.children}</>;
}
