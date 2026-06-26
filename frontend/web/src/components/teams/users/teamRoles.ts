import { TeamUserRole } from "@navtrack/shared/api/model";
import { SelectOption } from "../../ui/form/select/Select";

export const teamRoles: SelectOption[] = [
  {
    label: "member",
    value: TeamUserRole.Member
  },
  {
    label: "owner",
    value: TeamUserRole.Owner
  }
];
