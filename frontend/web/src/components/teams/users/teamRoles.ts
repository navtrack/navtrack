import { TeamUserRole } from "@navtrack/shared/api/model/generated";
import { SelectOption } from "../../ui/form/select/Select";

export const teamRoles: SelectOption[] = [
  {
    label: "generic.member",
    value: TeamUserRole.Member
  },
  {
    label: "generic.owner",
    value: TeamUserRole.Owner
  }
];
