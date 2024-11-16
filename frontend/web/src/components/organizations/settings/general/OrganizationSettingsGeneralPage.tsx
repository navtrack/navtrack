import { FormikTextInput } from "../../../ui/form/text-input/FormikTextInput";
import { Form, Formik, FormikHelpers } from "formik";
import { FormattedMessage } from "react-intl";
import { DeleteOrganizationModal } from "./DeleteOrganizationModal";
import { Icon } from "../../../ui/icon/Icon";
import { faCheck } from "@fortawesome/free-solid-svg-icons";
import { LoadingIndicator } from "../../../ui/loading-indicator/LoadingIndicator";
import { Card } from "../../../ui/card/Card";
import { CardBody } from "../../../ui/card/CardBody";
import { Heading } from "../../../ui/heading/Heading";
import { Button } from "../../../ui/button/Button";
import { object, ObjectSchema, string } from "yup";
import { useRenameOrganizationMutation } from "@navtrack/shared/hooks/queries/organizations/useRenameOrganizationMutation";
import { useCallback, useState } from "react";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { DeleteCard } from "../../../ui/card/DeleteCard";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";

export type RenameOrganizationFormValues = {
  name: string;
};

export function OrganizationSettingsGeneralPage() {
  const currentOrganization = useCurrentOrganization();
  const renameOrganizationMutation = useRenameOrganizationMutation();
  const [showSuccess, setShowSuccess] = useState(false);

  const submit = useCallback(
    (
      values: RenameOrganizationFormValues,
      formikHelpers: FormikHelpers<RenameOrganizationFormValues>
    ) => {
      setShowSuccess(false);
      if (currentOrganization.data) {
        renameOrganizationMutation.mutate(
          {
            organizationId: currentOrganization.data.id,
            data: { name: values.name }
          },
          {
            onSuccess: () => {
              setShowSuccess(true);
              setInterval(() => setShowSuccess(false), 5000);
            },
            onError: (error) => mapErrors(error, formikHelpers)
          }
        );
      }
    },
    [currentOrganization.data, renameOrganizationMutation]
  );

  const validationSchema: ObjectSchema<RenameOrganizationFormValues> = object({
    name: string().required("generic.name.required")
  }).defined();

  return (
    <>
      <Card>
        <CardBody>
          <Heading type="h2">
            <FormattedMessage id="assets.settings.general" />
          </Heading>
          <div className="mt-4">
            <Formik<RenameOrganizationFormValues>
              initialValues={{ name: currentOrganization.data?.name ?? "" }}
              onSubmit={(values, formikHelpers) =>
                submit(values, formikHelpers)
              }
              validationSchema={validationSchema}
              enableReinitialize>
              {() => (
                <Form className="grid grid-cols-12 gap-6">
                  <div className="col-span-7">
                    <FormikTextInput
                      name="name"
                      label="generic.name"
                      loading={currentOrganization.data === undefined}
                      rightAddon={
                        <div className="ml-2 flex items-center">
                          <Button
                            color="secondary"
                            type="submit"
                            size="md"
                            disabled={currentOrganization.data === undefined}>
                            <FormattedMessage id="assets.settings.general.rename" />
                          </Button>
                          <div className="ml-2 w-4">
                            {renameOrganizationMutation.isLoading && (
                              <LoadingIndicator />
                            )}
                            {renameOrganizationMutation.isSuccess && (
                              <Icon icon={faCheck} className="text-green-600" />
                            )}
                          </div>
                        </div>
                      }
                    />
                  </div>
                </Form>
              )}
            </Formik>
          </div>
        </CardBody>
      </Card>
      <DeleteCard>
        <CardBody>
          <div className="">
            <Heading type="h2">
              <FormattedMessage id="organizations.settings.delete.organization" />
            </Heading>
            <p className="mt-2 text-sm text-gray-500">
              <FormattedMessage id="organizations.settings.delete.info" />
            </p>
            <div className="mt-4 text-right">
              <DeleteOrganizationModal />
            </div>
          </div>
        </CardBody>
      </DeleteCard>
    </>
  );
}
