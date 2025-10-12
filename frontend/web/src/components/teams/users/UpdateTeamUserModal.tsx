import { TeamUserRole, TeamUser } from "@navtrack/shared/api/model";
import { FormikHelpers } from "formik";
import { useCallback } from "react";
import { FormikSelect } from "../../ui/form/select/FormikSelect";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { TextInput } from "../../ui/form/text-input/TextInput";
import { NewEditModal } from "../../ui/modal/NewEditModal";
import { faUserEdit } from "@fortawesome/free-solid-svg-icons";
import { useNotification } from "../../ui/notification/useNotification";
import { useUpdateTeamUserMutation } from "@navtrack/shared/hooks/queries/teams/useUpdateTeamUserMutation";
import { teamRoles } from "./teamRoles";

type UpdateTeamUserModalProps = {
  user: TeamUser;
  teamId?: string;
};

export type UpdateUserFormValues = {
  role: TeamUserRole;
};

export function UpdateTeamUserModal(props: UpdateTeamUserModalProps) {
  const mutation = useUpdateTeamUserMutation();
  const { showNotification } = useNotification();

  const handleSubmit = useCallback(
    async (
      values: UpdateUserFormValues,
      formikHelpers: FormikHelpers<UpdateUserFormValues>,
      close: () => void
    ) => {
      if (props.user && props.teamId) {
        await mutation.mutateAsync(
          {
            teamId: props.teamId,
            userId: props.user.userId,
            data: {
              userRole: values.role
            }
          },
          {
            onSuccess: () => {
              close();
              showNotification({
                type: "success",
                description: "organization.users.edit.success"
              });
            },
            onError: (error) => mapErrors(error, formikHelpers)
          }
        );
      }
    },
    [mutation, props.teamId, props.user, showNotification]
  );

  return (
    <NewEditModal<UpdateUserFormValues>
      initialValues={{
        role: props.user.userRole
      }}
      icon={faUserEdit}
      onSubmit={handleSubmit}
      isLoading={mutation.isPending}
      title="generic.edit-user">
      <TextInput
        name="email"
        label="generic.email"
        autoComplete="off"
        value={props.user.email}
        disabled
      />
      <FormikSelect
        name="role"
        label="generic.role"
        placeholder="Select a role"
        options={teamRoles}
      />
    </NewEditModal>
  );
}
