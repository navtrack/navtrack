import { faPlus, faUserPlus } from "@fortawesome/free-solid-svg-icons";
import { Form, Formik, FormikHelpers } from "formik";
import { useCallback, useState } from "react";
import { FormattedMessage } from "react-intl";
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
import { FormikAutocomplete } from "../../ui/form/autocomplete/FormikAutocomplete";
import { useAssetsQuery } from "@navtrack/shared/hooks/queries/assets/useAssetsQuery";
import { useCreateTeamAssetMutation } from "@navtrack/shared/hooks/queries/teams/useCreateTeamAssetMutation";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";

export type CreateAssetFormValues = {
  assetId: string;
};

type CreateTeamUserModalProps = {
  teamId?: string;
};

export function CreateTeamAssetModal(props: CreateTeamUserModalProps) {
  const createTeamAsset = useCreateTeamAssetMutation();
  const currentOrganization = useCurrentOrganization();
  const { showNotification } = useNotification();
  const [open, setOpen] = useState(false);
  const assets = useAssetsQuery({
    organizationId: currentOrganization.data?.id
  });

  const validationSchema: ObjectSchema<CreateAssetFormValues> = object({
    assetId: string().required("generic.asset.required")
  }).defined();

  const handleSubmit = useCallback(
    (
      values: CreateAssetFormValues,
      formikHelpers: FormikHelpers<CreateAssetFormValues>
    ) => {
      if (props.teamId) {
        createTeamAsset.mutate(
          {
            teamId: props.teamId,
            data: {
              assetId: values.assetId
            }
          },
          {
            onSuccess: () => {
              setOpen(false);

              showNotification({
                type: "success",
                description: "teams.assets.add.success"
              });
            },
            onError: (error) => mapErrors(error, formikHelpers)
          }
        );
      }
    },
    [createTeamAsset, props.teamId, showNotification]
  );

  return (
    <>
      <Button onClick={() => setOpen(true)} icon={faPlus}>
        <FormattedMessage id="generic.add-asset" />
      </Button>
      <Modal
        open={open}
        close={() => setOpen(false)}
        className="w-full max-w-md">
        <Formik<CreateAssetFormValues>
          initialValues={{ assetId: "" }}
          validationSchema={validationSchema}
          onSubmit={handleSubmit}>
          {() => (
            <Form>
              <ModalContainer>
                <ModalContent>
                  <ModalIcon icon={faUserPlus} />
                  <ModalBody>
                    <h3 className="text-lg font-medium leading-6 text-gray-900">
                      <FormattedMessage id="teams.assets.add.title" />
                    </h3>
                    <div className="mt-2 space-y-4">
                      <FormikAutocomplete
                        name={nameOf<CreateAssetFormValues>("assetId")}
                        label="generic.asset"
                        placeholder="teams.assets.add.search-placeholder"
                        options={assets.data?.items.map((x) => ({
                          label: x.name,
                          value: x.id
                        }))}
                      />
                    </div>
                  </ModalBody>
                </ModalContent>
                <ModalActions cancel={() => setOpen(false)}>
                  <Button type="submit" isLoading={createTeamAsset.isPending}>
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
