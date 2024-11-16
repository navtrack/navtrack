import { OrganizationUserRole } from "@navtrack/shared/api/model/generated";
import { SelectOption } from "../ui/form/select/Select";

export const organizationUserRoles: SelectOption[] = [
  {
    label: "generic.member",
    value: OrganizationUserRole.Member
  },
  {
    label: "generic.owner",
    value: OrganizationUserRole.Owner
  }
];
