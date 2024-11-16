import { faUserPlus } from "@fortawesome/free-solid-svg-icons";
import {
  OrganizationUserRole,
  TeamUserRole
} from "@navtrack/shared/api/model/generated";
import { Form, Formik, FormikHelpers } from "formik";
import { useCallback, useState } from "react";
import { FormattedMessage } from "react-intl";
import { FormikSelect } from "../ui/form/select/FormikSelect";
import { FormikTextInput } from "../ui/form/text-input/FormikTextInput";
import { ModalActions } from "../ui/modal/ModalActions";
import { ModalContainer } from "../ui/modal/ModalContainer";
import { ModalContent } from "../ui/modal/ModalContent";
import { ModalIcon } from "../ui/modal/ModalIcon";
import { Modal } from "../ui/modal/Modal";
import { ModalBody } from "../ui/modal/ModalBody";
import { Button } from "../ui/button/Button";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { ObjectSchema, mixed, object, string } from "yup";
import { useCurrentUserQuery } from "@navtrack/shared/hooks/queries/user/useCurrentUserQuery";
import { useCreateOrganizationUserMutation } from "@navtrack/shared/hooks/queries/organizations/useCreateOrganizationUserMutation";
import { organizationUserRoles } from "./organizationUserRoles";
import { nameOf } from "@navtrack/shared/utils/typescript";
import { useNotification } from "../ui/notification/useNotification";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";

export type CreateUserFormValues = {
  email: string;
  role: TeamUserRole;
};

export function CreateUserModal() {
  const createOrganizationUserMutation = useCreateOrganizationUserMutation();
  const currentUser = useCurrentUserQuery();
  const currentOrganization = useCurrentOrganization();
  const { showNotification } = useNotification();
  const [open, setOpen] = useState(false);

  const validationSchema: ObjectSchema<CreateUserFormValues> = object({
    email: string()
      .email("generic.email.invalid")
      .required("generic.email.required"),
    role: mixed<TeamUserRole>().required("generic.password.required")
  }).defined();

  const handleSubmit = useCallback(
    (
      values: CreateUserFormValues,
      formikHelpers: FormikHelpers<CreateUserFormValues>
    ) => {
      if (currentUser.data && currentOrganization.data) {
        createOrganizationUserMutation.mutate(
          {
            organizationId: currentOrganization.data.id,
            data: {
              email: values.email,
              userRole: values.role
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
    [
      currentUser.data,
      currentOrganization.data,
      createOrganizationUserMutation,
      showNotification
    ]
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
                      <FormattedMessage id="organization.users.add.title" />
                    </h3>
                    <div className="mt-2 space-y-4">
                      <FormikTextInput
                        name={nameOf<CreateUserFormValues>("email")}
                        label="generic.email"
                        autoComplete="off"
                      />
                      <FormikSelect
                        name={nameOf<CreateUserFormValues>("role")}
                        label="generic.role"
                        placeholder="Select a role"
                        options={organizationUserRoles}
                      />
                    </div>
                  </ModalBody>
                </ModalContent>
                <ModalActions cancel={() => setOpen(false)}>
                  <Button
                    type="submit"
                    isLoading={createOrganizationUserMutation.isLoading}>
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
