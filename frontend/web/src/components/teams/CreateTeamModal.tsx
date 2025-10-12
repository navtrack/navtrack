import { faPlus, faUsers } from "@fortawesome/free-solid-svg-icons";
import { Form, Formik, FormikHelpers } from "formik";
import { useCallback, useState } from "react";
import { FormattedMessage } from "react-intl";
import { FormikTextInput } from "../ui/form/text-input/FormikTextInput";
import { ModalActions } from "../ui/modal/ModalActions";
import { ModalContainer } from "../ui/modal/ModalContainer";
import { ModalContent } from "../ui/modal/ModalContent";
import { ModalIcon } from "../ui/modal/ModalIcon";
import { Modal } from "../ui/modal/Modal";
import { ModalBody } from "../ui/modal/ModalBody";
import { Button } from "../ui/button/Button";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { ObjectSchema, object, string } from "yup";
import { nameOf } from "@navtrack/shared/utils/typescript";
import { useNotification } from "../ui/notification/useNotification";
import { useCreateTeamMutation } from "@navtrack/shared/hooks/queries/teams/useCreateTeamMutation";
import { generatePath, useNavigate } from "react-router-dom";
import { Paths } from "../../app/Paths";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";
import { Authorize } from "@navtrack/shared/components/authorize/Authorize";
import { OrganizationUserRole } from "@navtrack/shared/api/model";

export type CreateTeamFormValues = {
  name: string;
};

export function CreateTeamModal() {
  const createTeamMutation = useCreateTeamMutation();
  const currentOrganization = useCurrentOrganization();
  const { showNotification } = useNotification();
  const [open, setOpen] = useState(false);
  const navigate = useNavigate();

  const validationSchema: ObjectSchema<CreateTeamFormValues> = object({
    name: string().required("generic.name.required")
  }).defined();

  const handleSubmit = useCallback(
    (
      values: CreateTeamFormValues,
      formikHelpers: FormikHelpers<CreateTeamFormValues>
    ) => {
      if (currentOrganization.data) {
        createTeamMutation.mutate(
          {
            organizationId: currentOrganization.data.id,
            data: {
              name: values.name
            }
          },
          {
            onSuccess: (data) => {
              setOpen(false);

              showNotification({
                type: "success",
                description: "teams.create.success"
              });

              navigate(
                generatePath(Paths.TeamUsers, {
                  id: data.id
                })
              );
            },
            onError: (error) => mapErrors(error, formikHelpers)
          }
        );
      }
    },
    [currentOrganization.data, createTeamMutation, showNotification, navigate]
  );

  return (
    <Authorize organizationUserRole={OrganizationUserRole.Owner}>
      <Button onClick={() => setOpen(true)} icon={faPlus}>
        <FormattedMessage id="teams.new" />
      </Button>
      <Modal
        open={open}
        close={() => setOpen(false)}
        className="w-full max-w-md">
        <Formik<CreateTeamFormValues>
          initialValues={{ name: "" }}
          validationSchema={validationSchema}
          onSubmit={handleSubmit}>
          {() => (
            <Form>
              <ModalContainer>
                <ModalContent>
                  <ModalIcon icon={faUsers} />
                  <ModalBody>
                    <h3 className="text-lg font-medium leading-6 text-gray-900">
                      <FormattedMessage id="teams.create" />
                    </h3>
                    <div className="mt-2 space-y-4">
                      <FormikTextInput
                        name={nameOf<CreateTeamFormValues>("name")}
                        label="generic.name"
                      />
                    </div>
                  </ModalBody>
                </ModalContent>
                <ModalActions cancel={() => setOpen(false)}>
                  <Button
                    type="submit"
                    isLoading={createTeamMutation.isPending}>
                    <FormattedMessage id="generic.save" />
                  </Button>
                </ModalActions>
              </ModalContainer>
            </Form>
          )}
        </Formik>
      </Modal>
    </Authorize>
  );
}
