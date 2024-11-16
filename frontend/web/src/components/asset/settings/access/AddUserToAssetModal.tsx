import { faUser } from "@fortawesome/free-solid-svg-icons";
import { Form, Formik } from "formik";
import { useMemo } from "react";
import { FormattedMessage } from "react-intl";
import { FormikTextInput } from "../../../ui/form/text-input/FormikTextInput";
import { Modal } from "../../../ui/modal/Modal";
import { ModalActions } from "../../../ui/modal/ModalActions";
import { ModalBody } from "../../../ui/modal/ModalBody";
import { ModalContainer } from "../../../ui/modal/ModalContainer";
import { ModalContent } from "../../../ui/modal/ModalContent";
import { ModalIcon } from "../../../ui/modal/ModalIcon";
import {
  AddUserToAssetFormValues,
  useAddUserToAsset
} from "./useAddUserToAsset";
import { AssetUserRole } from "@navtrack/shared/api/model/generated";
import { Button } from "../../../ui/button/Button";
import { FormikSelect } from "../../../ui/form/select/FormikSelect";

type AddUserToAssetModalProps = {
  show: boolean;
  close: () => void;
};

type Role = {
  label: string;
  value: AssetUserRole;
};

export function AddUserToAssetModal(props: AddUserToAssetModalProps) {
  const { validationSchema, handleSubmit, loading } = useAddUserToAsset({
    close: props.close
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
    <Modal open={props.show} close={props.close} className="w-full max-w-sm">
      <Formik<AddUserToAssetFormValues>
        initialValues={{ email: "", role: AssetUserRole.Viewer }}
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
              <ModalActions cancel={props.close}>
                <Button type="submit" isLoading={loading}>
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
