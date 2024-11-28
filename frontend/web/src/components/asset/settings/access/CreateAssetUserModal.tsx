import { faUserPlus } from "@fortawesome/free-solid-svg-icons";
import { Form, Formik } from "formik";
import { useMemo, useState } from "react";
import { FormattedMessage } from "react-intl";
import { FormikTextInput } from "../../../ui/form/text-input/FormikTextInput";
import { Modal } from "../../../ui/modal/Modal";
import { ModalActions } from "../../../ui/modal/ModalActions";
import { ModalBody } from "../../../ui/modal/ModalBody";
import { ModalContainer } from "../../../ui/modal/ModalContainer";
import { ModalContent } from "../../../ui/modal/ModalContent";
import { ModalIcon } from "../../../ui/modal/ModalIcon";
import {
  CreateAssetUserFormValues,
  useCreateAssetUser
} from "./useCreateAssetUser";
import { AssetUserRole } from "@navtrack/shared/api/model/generated";
import { Button } from "../../../ui/button/Button";
import { FormikSelect } from "../../../ui/form/select/FormikSelect";
import { useOrganizationUsersQuery } from "@navtrack/shared/hooks/queries/organizations/useOrganizationUsersQuery";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";
import { FormikAutocomplete } from "../../../ui/form/autocomplete/FormikAutocomplete";
import { nameOf } from "@navtrack/shared/utils/typescript";

type Role = {
  label: string;
  value: AssetUserRole;
};

export function CreateAssetUserModal() {
  const [open, setOpen] = useState(false);
  const { validationSchema, handleSubmit, loading } = useCreateAssetUser({
    close: () => setOpen(false)
  });
  const currentOrganization = useCurrentOrganization();
  const organizationsUsersQuery = useOrganizationUsersQuery({
    organizationId: currentOrganization.data?.id
  });

  const roles: Role[] = useMemo(
    () => [
      // {
      //   label: "Owner",
      //   value: AssetUserRole.Owner
      // },
      {
        label: "Viewer",
        value: AssetUserRole.Viewer
      }
    ],
    []
  );

  return (
    <>
      <Button onClick={() => setOpen(true)} icon={faUserPlus}>
        <FormattedMessage id="generic.add-user" />
      </Button>
      <Modal
        open={open}
        close={() => setOpen(false)}
        className="w-full max-w-sm">
        <Formik<CreateAssetUserFormValues>
          initialValues={{ email: "", role: AssetUserRole.Viewer }}
          validationSchema={validationSchema}
          onSubmit={handleSubmit}>
          {() => (
            <Form>
              <ModalContainer>
                <ModalContent>
                  <ModalIcon icon={faUserPlus} />
                  <ModalBody>
                    <h3 className="text-lg font-medium leading-6 text-gray-900">
                      <FormattedMessage id="assets.settings.access.add-user.title" />
                    </h3>
                    <div className="mt-2 space-y-2">
                      <FormikAutocomplete
                        name={nameOf<CreateAssetUserFormValues>("email")}
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
                        name="role"
                        label="generic.role"
                        placeholder="Select a role"
                        options={roles.map((x) => ({
                          value: x.value,
                          label: x.label
                        }))}
                      />
                    </div>
                  </ModalBody>
                </ModalContent>
                <ModalActions cancel={() => setOpen(false)}>
                  <Button type="submit" isLoading={loading}>
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
