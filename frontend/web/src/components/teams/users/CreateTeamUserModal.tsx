import { faUserPlus } from "@fortawesome/free-solid-svg-icons";
import {
  OrganizationUserRole,
  TeamUserRole
} from "@navtrack/shared/api/model/generated";
import { Form, Formik, FormikHelpers } from "formik";
import { useCallback, useState } from "react";
import { FormattedMessage } from "react-intl";
import { FormikSelect } from "../../ui/form/select/FormikSelect";
import { ModalActions } from "../../ui/modal/ModalActions";
import { ModalContainer } from "../../ui/modal/ModalContainer";
import { ModalContent } from "../../ui/modal/ModalContent";
import { ModalIcon } from "../../ui/modal/ModalIcon";
import { Modal } from "../../ui/modal/Modal";
import { ModalBody } from "../../ui/modal/ModalBody";
import { Button } from "../../ui/button/Button";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { ObjectSchema, object, string } from "yup";
import { nameOf } from "@navtrack/shared/utils/typescript";
import { useNotification } from "../../ui/notification/useNotification";
import { useCreateTeamUserMutation } from "@navtrack/shared/hooks/queries/teams/useCreateTeamUserMutation";
import { useOrganizationUsersQuery } from "@navtrack/shared/hooks/queries/organizations/useOrganizationUsersQuery";
import { FormikAutocomplete } from "../../ui/form/autocomplete/FormikAutocomplete";
import { teamRoles } from "./teamRoles";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";

export type CreateUserFormValues = {
  email: string;
  role: string;
};

type CreateTeamUserModalProps = {
  teamId?: string;
};

export function CreateTeamUserModal(props: CreateTeamUserModalProps) {
  const createTeamUserMutation = useCreateTeamUserMutation();
  const currentOrganization = useCurrentOrganization();
  const { showNotification } = useNotification();
  const [open, setOpen] = useState(false);
  const organizationsUsersQuery = useOrganizationUsersQuery({
    organizationId: currentOrganization.data?.id
  });

  const validationSchema: ObjectSchema<CreateUserFormValues> = object({
    email: string()
      .email("generic.email.invalid")
      .required("generic.email.required"),
    role: string().required("generic.password.required")
  }).defined();

  const handleSubmit = useCallback(
    (
      values: CreateUserFormValues,
      formikHelpers: FormikHelpers<CreateUserFormValues>
    ) => {
      if (props.teamId) {
        createTeamUserMutation.mutate(
          {
            teamId: props.teamId,
            data: {
              email: values.email,
              userRole: values.role as TeamUserRole // TODO: Fix this
            }
          },
          {
            onSuccess: () => {
              setOpen(false);

              showNotification({
                type: "success",
                description: "organization.users.add.success"
              });
            },
            onError: (error) => mapErrors(error, formikHelpers)
          }
        );
      }
    },
    [createTeamUserMutation, props.teamId, showNotification]
  );

  return (
    <>
      <Button onClick={() => setOpen(true)} icon={faUserPlus}>
        <FormattedMessage id="generic.add-user" />
      </Button>
      <Modal
        open={open}
        close={() => setOpen(false)}
        className="w-full max-w-md">
        <Formik<CreateUserFormValues>
          initialValues={{ email: "", role: OrganizationUserRole.Member }}
          validationSchema={validationSchema}
          onSubmit={handleSubmit}>
          {() => (
            <Form>
              <ModalContainer>
                <ModalContent>
                  <ModalIcon icon={faUserPlus} />
                  <ModalBody>
                    <h3 className="text-lg font-medium leading-6 text-gray-900">
                      <FormattedMessage id="teams.users.add.title" />
                    </h3>
                    <div className="mt-2 space-y-4">
                      <FormikAutocomplete
                        name={nameOf<CreateUserFormValues>("email")}
                        label="generic.user"
                        placeholder="teams.users.add.search-placeholder"
                        options={organizationsUsersQuery.data?.items.map(
                          (user) => ({
                            label: user.email,
                            value: user.email
                          })
                        )}
                      />
                      <FormikSelect
                        name={nameOf<CreateUserFormValues>("role")}
                        label="generic.role"
                        placeholder="Select a role"
                        options={teamRoles}
                      />
                    </div>
                  </ModalBody>
                </ModalContent>
                <ModalActions cancel={() => setOpen(false)}>
                  <Button
                    type="submit"
                    isLoading={createTeamUserMutation.isLoading}>
                    <FormattedMessage id="generic.save" />
                  </Button>
                </ModalActions>
              </ModalContainer>
            </Form>
          )}
        </Formik>
      </Modal>
    </>
  );
}
