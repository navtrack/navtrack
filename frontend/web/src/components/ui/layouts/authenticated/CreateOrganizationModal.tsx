import { Form, Formik, FormikHelpers } from "formik";
import { useCallback } from "react";
import { FormattedMessage } from "react-intl";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { ObjectSchema, object, string } from "yup";
import { nameOf } from "@navtrack/shared/utils/typescript";
import { useCreateOrganizationMutation } from "@navtrack/shared/hooks/queries/organizations/useCreateOrganizationMutation";
import { Button } from "../../button/Button";
import { Modal } from "../../modal/Modal";
import { ModalContainer } from "../../modal/ModalContainer";
import { ModalContent } from "../../modal/ModalContent";
import { ModalIcon } from "../../modal/ModalIcon";
import { ModalBody } from "../../modal/ModalBody";
import { FormikTextInput } from "../../form/text-input/FormikTextInput";
import { ModalActions } from "../../modal/ModalActions";
import { useNotification } from "../../notification/useNotification";
import { faBuilding } from "@fortawesome/free-regular-svg-icons";
import { generatePath, useNavigate } from "react-router-dom";
import { Paths } from "../../../../app/Paths";

export type CreateOrganizationFormValues = {
  name: string;
};

type CreateOrganizationModalProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
};

export function CreateOrganizationModal(props: CreateOrganizationModalProps) {
  const createOrganizationMutation = useCreateOrganizationMutation();
  const { showNotification } = useNotification();
  const navigate = useNavigate();

  const validationSchema: ObjectSchema<CreateOrganizationFormValues> = object({
    name: string().required("generic.name.required")
  }).defined();

  const handleSubmit = useCallback(
    (
      values: CreateOrganizationFormValues,
      formikHelpers: FormikHelpers<CreateOrganizationFormValues>
    ) => {
      createOrganizationMutation.mutate(
        {
          data: {
            name: values.name
          }
        },
        {
          onSuccess: (result) => {
            props.setOpen(false);
            showNotification({
              type: "success",
              description: "organizations.create.success"
            });
            navigate(generatePath(Paths.OrganizationLive, { id: result.id }));
          },
          onError: (error) => mapErrors(error, formikHelpers)
        }
      );
    },
    [createOrganizationMutation, navigate, props, showNotification]
  );

  return (
    <>
      <Modal
        open={props.open}
        close={() => props.setOpen(false)}
        className="w-full max-w-md">
        <Formik<CreateOrganizationFormValues>
          initialValues={{ name: "" }}
          validationSchema={validationSchema}
          onSubmit={handleSubmit}>
          {() => (
            <Form>
              <ModalContainer>
                <ModalContent>
                  <ModalIcon icon={faBuilding} />
                  <ModalBody>
                    <h3 className="text-lg font-medium leading-6 text-gray-900">
                      <FormattedMessage id="organizations.create" />
                    </h3>
                    <div className="mt-2 space-y-4">
                      <FormikTextInput
                        name={nameOf<CreateOrganizationFormValues>("name")}
                        label="generic.name"
                      />
                    </div>
                  </ModalBody>
                </ModalContent>
                <ModalActions cancel={() => props.setOpen(false)}>
                  <Button
                    type="submit"
                    isLoading={createOrganizationMutation.isLoading}>
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
