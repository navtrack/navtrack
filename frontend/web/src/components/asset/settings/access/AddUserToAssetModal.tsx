import { faUser } from "@fortawesome/free-solid-svg-icons";
import { Form, Formik } from "formik";
import { useMemo } from "react";
import { FormattedMessage } from "react-intl";
import { Button } from "../../../ui/button-old/Button";
import { FormikTextInput } from "../../../ui/form/text-input/FormikTextInput";
import { Modal } from "../../../ui/modal/Modal";
import { ModalActions } from "../../../ui/modal/ModalActions";
import { ModalBody } from "../../../ui/modal/ModalBody";
import { ModalContainer } from "../../../ui/modal/ModalContainer";
import { ModalContent } from "../../../ui/modal/ModalContent";
import { ModalIcon } from "../../../ui/modal/ModalIcon";
import { FormikCustomSelect } from "../../../ui/form/select/FormikCustomSelect";
import { AddUserToAssetFormValues } from "./types";
import { useAddUserToAsset } from "./useAddUserToAsset";
import { AssetRoleType } from "@navtrack/shared/api/model/custom/AssetRoleType";

interface IAddUserToAssetModal {
  show: boolean;
  close: () => void;
}

type Role = {
  label: string;
  value: AssetRoleType;
};

export function AddUserToAssetModal(props: IAddUserToAssetModal) {
  const { validationSchema, handleSubmit, loading } = useAddUserToAsset({
    close: props.close
  });

  const roles: Role[] = useMemo(
    () => [
      {
        label: "Owner",
        value: AssetRoleType.Owner
      },
      {
        label: "Viewer",
        value: AssetRoleType.Viewer
      }
    ],
    []
  );

  return (
    <Modal open={props.show} close={props.close} className="w-full max-w-sm">
      <Formik<AddUserToAssetFormValues>
        initialValues={{ email: "", role: "" }}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}>
        {() => (
          <Form>
            <ModalContainer>
              <ModalContent>
                <ModalIcon icon={faUser} />
                <ModalBody>
                  <h3 className="text-lg font-medium leading-6 text-gray-900">
                    <FormattedMessage id="assets.settings.access.add-user.title" />
                  </h3>
                  <div className="mt-2 space-y-2">
                    <FormikTextInput
                      name="email"
                      label="generic.email"
                      autoComplete="off"
                    />
                    <FormikCustomSelect
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
              <ModalActions close={props.close}>
                <Button type="submit" loading={loading}>
                  <FormattedMessage id="generic.save" />
                </Button>
              </ModalActions>
            </ModalContainer>
          </Form>
        )}
      </Formik>
    </Modal>
  );
}
