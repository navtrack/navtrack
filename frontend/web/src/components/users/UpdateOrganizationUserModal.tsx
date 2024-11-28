import {
  OrganizationUserRole,
  OrganizationUser
} from "@navtrack/shared/api/model/generated";
import { FormikHelpers } from "formik";
import { useCallback } from "react";
import { FormikSelect } from "../ui/form/select/FormikSelect";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { useCurrentUserQuery } from "@navtrack/shared/hooks/queries/user/useCurrentUserQuery";
import { TextInput } from "../ui/form/text-input/TextInput";
import { useUpdateOrganizationUserMutation } from "@navtrack/shared/hooks/queries/organizations/useUpdateOrganizationUserMutation";
import { NewEditModal } from "../ui/modal/NewEditModal";
import { faUserEdit } from "@fortawesome/free-solid-svg-icons";
import { organizationUserRoles } from "./organizationUserRoles";
import { useNotification } from "../ui/notification/useNotification";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";

type UpdateUserModalProps = {
  user: OrganizationUser;
};

export type UpdateUserFormValues = {
  role: string;
};

export function UpdateOrganizationUserModal(props: UpdateUserModalProps) {
  const currentUser = useCurrentUserQuery();
  const currentOrganization = useCurrentOrganization();
  const mutation = useUpdateOrganizationUserMutation();
  const { showNotification } = useNotification();

  const handleSubmit = useCallback(
    async (
      values: UpdateUserFormValues,
      formikHelpers: FormikHelpers<UpdateUserFormValues>,
      close: () => void
    ) => {
      if (currentUser.data && currentOrganization.data) {
        await mutation.mutateAsync(
          {
            organizationId: currentOrganization.data.id,
            userId: props.user.userId,
            data: {
              userRole: values.role as OrganizationUserRole // TODO
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
    [
      currentOrganization.data,
      currentUser.data,
      mutation,
      props.user.userId,
      showNotification
    ]
  );

  return (
    <NewEditModal<UpdateUserFormValues>
      initialValues={{
        role: props.user.userRole
      }}
      icon={faUserEdit}
      onSubmit={handleSubmit}
      isLoading={mutation.isLoading}
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
        options={organizationUserRoles}
      />
    </NewEditModal>
  );
}
