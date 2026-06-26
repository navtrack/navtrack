import { OrganizationUserRole } from "@navtrack/shared/api/model";
import { SelectOption } from "../ui/form/select/Select";

export const organizationUserRoles: SelectOption[] = [
  {
    label: "member",
    value: OrganizationUserRole.Member
  },
  {
    label: "owner",
    value: OrganizationUserRole.Owner
  }
];
