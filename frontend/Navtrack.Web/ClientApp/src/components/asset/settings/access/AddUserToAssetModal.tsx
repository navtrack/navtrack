import { faUser } from "@fortawesome/free-solid-svg-icons";
import { Form, Formik } from "formik";
import { useMemo } from "react";
import { FormattedMessage } from "react-intl";
import Button from "../../../ui/shared/button/Button";
import FormikTextInput from "../../../ui/shared/text-input/FormikTextInput";
import Modal from "../../../ui/shared/modal/Modal";
import ModalActions from "../../../ui/shared/modal/ModalActions";
import ModalBody from "../../../ui/shared/modal/ModalBody";
import ModalContainer from "../../../ui/shared/modal/ModalContainer";
import ModalContent from "../../../ui/shared/modal/ModalContent";
import ModalIcon from "../../../ui/shared/modal/ModalIcon";
import FormikSelectInput from "../../../ui/shared/select/FormikSelectInput";
import { AddUserToAssetFormValues } from "./types";
import useAddUserToAsset from "./useAddUserToAsset";
import { AssetRoleType } from "@navtrack/navtrack-app-shared";

interface IAddUserToAssetModal {
  show: boolean;
  close: () => void;
}

type Role = {
  label: string;
  value: AssetRoleType;
};

export default function AddUserToAssetModal(props: IAddUserToAssetModal) {
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
                      autoCompleteOff
                    />
                    <FormikSelectInput
                      name="role"
                      label="generic.role"
                      placeholder="Select a role"
                      items={roles.map((x) => ({
                        id: x.value,
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
