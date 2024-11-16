import { FormattedMessage } from "react-intl";
import { Heading } from "../../ui/heading/Heading";
import { useParams } from "react-router-dom";
import { TeamLayout } from "../TeamLayout";
import { Card } from "../../ui/card/Card";
import { CardBody } from "../../ui/card/CardBody";
import { Form, Formik } from "formik";
import { RenameAssetFormValues } from "../../organizations/settings/general/useRenameAsset";
import { FormikTextInput } from "../../ui/form/text-input/FormikTextInput";
import { Button } from "../../ui/button/Button";
import { useTeamQuery } from "@navtrack/shared/hooks/queries/teams/useTeamQuery";
import { useUpdateTeamMutation } from "@navtrack/shared/hooks/queries/teams/useUpdateTeamMutation";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { DeleteTeamModal } from "./DeleteTeamModal";
import { DeleteCard } from "../../ui/card/DeleteCard";

export function TeamSettingsPage() {
  const { id } = useParams();
  const team = useTeamQuery({ teamId: id });
  const updateTeam = useUpdateTeamMutation();

  return (
    <TeamLayout team={team.data} isLoading={team.isLoading}>
      <Card>
        <CardBody>
          <Heading type="h2">
            <FormattedMessage id="assets.settings.general" />
          </Heading>
          <div className="mt-4">
            <Formik<RenameAssetFormValues>
              initialValues={{ name: team.data?.name ?? "" }}
              onSubmit={(values, formikHelpers) => {
                if (team.data) {
                  updateTeam.mutate(
                    {
                      teamId: team.data?.id,
                      data: { name: values.name }
                    },
                    {
                      onSuccess: () => {
                        // setShowSuccess(true);
                        // setInterval(() => setShowSuccess(false), 5000);
                      },
                      onError: (error) => mapErrors(error, formikHelpers)
                    }
                  );
                }
              }}
              // validationSchema={renameAsset.validationSchema}
              enableReinitialize>
              {() => (
                <Form className="grid grid-cols-12 gap-6">
                  <div className="col-span-7">
                    <FormikTextInput
                      name="name"
                      label="generic.name"
                      loading={team.data === undefined}
                      rightAddon={
                        <div className="ml-2 flex items-center">
                          <Button
                            color="secondary"
                            type="submit"
                            size="md"
                            disabled={team.data === undefined}>
                            <FormattedMessage id="assets.settings.general.rename" />
                          </Button>
                          <div className="ml-2 w-4">
                            {/* {renameAsset.loading && <LoadingIndicator />}
                                  {renameAsset.showSuccess && (
                                    <Icon
                                      icon={faCheck}
                                      className="text-green-600"
                                    />
                                  )} */}
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
          <div className="flex justify-between">
            <div>
              <Heading type="h2">
                <FormattedMessage id="teams.settings.delete.team" />
              </Heading>
              <p className="mt-2 text-sm text-gray-500">
                <FormattedMessage id="teams.settings.delete.team.info" />
              </p>
            </div>
            <div className="flex items-end">
              <DeleteTeamModal team={team.data} />
            </div>
          </div>
        </CardBody>
      </DeleteCard>
    </TeamLayout>
  );
}
