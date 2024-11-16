import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { FormikTextInput } from "../../../ui/form/text-input/FormikTextInput";
import { nameOf } from "@navtrack/shared/utils/typescript";
import { useNotification } from "../../../ui/notification/useNotification";
import { useNavigate } from "react-router-dom";
import { Paths } from "../../../../app/Paths";
import { DeleteModal } from "../../../ui/modal/DeleteModal";
import { Button } from "../../../ui/button/Button";
import { ObjectSchema, object, string } from "yup";
import { useDeleteOrganizationMutation } from "@navtrack/shared/hooks/queries/organizations/useDeleteOrganizationMutation";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";

export type DeleteOrganizationFormValues = {
  name: string;
};

export function DeleteOrganizationModal() {
  const currentOrganization = useCurrentOrganization();
  const navigate = useNavigate();
  const { showNotification } = useNotification();

  const deleteOrganizationMutation = useDeleteOrganizationMutation({
    onSuccess: () => {
      navigate(Paths.Home);
      showNotification({
        type: "success",
        description: "organizations.settings.delete.success"
      });
    }
  });

  const validationSchema: ObjectSchema<DeleteOrganizationFormValues> = object()
    .shape({
      name: string()
        .oneOf([`${currentOrganization.data?.name}`], "generic.name.not-match")
        .required("generic.name.not-match")
    })
    .defined();

  return (
    <Formik<DeleteOrganizationFormValues>
      enableReinitialize
      initialValues={{ name: "" }}
      validationSchema={validationSchema}
      onSubmit={() => {
        if (currentOrganization.data !== undefined) {
          return deleteOrganizationMutation.mutateAsync({
            organizationId: currentOrganization.data.id
          });
        }
      }}>
      {({ values, submitForm, resetForm }) => (
        <DeleteModal
          autoClose={false}
          maxWidth="lg"
          isLoading={deleteOrganizationMutation.isLoading}
          onClose={() => resetForm()}
          onConfirm={() => submitForm()}
          renderButton={(open) => (
            <Button color="error" type="submit" size="base" onClick={open}>
              <FormattedMessage id="organizations.settings.delete.organization" />
            </Button>
          )}>
          <Form>
            <div className="mt-2 text-sm">
              <p>
                <FormattedMessage
                  id="organizations.settings.delete.question"
                  values={{
                    organization: (
                      <span className="font-semibold">
                        {currentOrganization.data?.name}
                      </span>
                    )
                  }}
                />
              </p>
              <p className="mt-4">
                <FormattedMessage id="organizations.settings.delete.confirm" />
              </p>
            </div>
            <div className="mt-2">
              <FormikTextInput
                name={nameOf<DeleteOrganizationFormValues>("name")}
                value={values.name}
              />
            </div>
          </Form>
        </DeleteModal>
      )}
    </Formik>
  );
}
