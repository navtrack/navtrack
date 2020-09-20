import React, { ReactNode } from "react";
import { UserRole } from "../../../apis/types/user/UserRole";
import { AuthorizationService } from "../../../services/authentication/AuthorizationService";

type Props = {
  children: ReactNode;
  userRole?: UserRole;
};

export default function Authorize(props: Props) {
  return <>{AuthorizationService.isAuthorized(props.userRole) && props.children}</>;
}
