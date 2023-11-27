import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { FormikTextInput } from "../../../ui/form/text-input/FormikTextInput";
import { DeleteAssetFormValues } from "./types";
import { Modal } from "../../../ui/modal/Modal";
import { DeleteModalContainer } from "../../../ui/modal/DeleteModalContainer";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { nameOf } from "@navtrack/shared/utils/typescript";
import { useDeleteAssetValidationSchema } from "./useDeleteAssetValidationSchema";
import { useDeleteAsset } from "@navtrack/shared/hooks/assets/delete-asset/useDeleteAsset";
import { useNotification } from "../../../ui/notification/useNotification";
import { useNavigate } from "react-router-dom";
import { Paths } from "../../../../app/Paths";

type DeleteAssetModalProps = {
  show: boolean;
  close: () => void;
};

export function DeleteAssetModal(props: DeleteAssetModalProps) {
  const currentAsset = useCurrentAsset();
  const validationSchema = useDeleteAssetValidationSchema();
  const navigate = useNavigate();
  const { showNotification } = useNotification();

  const deleteAsset = useDeleteAsset({
    onSuccess: () => {
      navigate(Paths.Home);
      showNotification({
        type: "success",
        description: "assets.settings.general.delete-asset.success"
      });
    }
  });

  return (
    <Modal open={props.show} close={props.close} className="max-w-md">
      <Formik<DeleteAssetFormValues>
        initialValues={{ name: "" }}
        validationSchema={validationSchema}
        onSubmit={() => {
          if (currentAsset.data?.id) {
            deleteAsset.handleSubmit(currentAsset.data?.id);
          }
        }}>
        {({ values }) => (
          <Form>
            <DeleteModalContainer
              close={props.close}
              loading={deleteAsset.isLoading}>
              <h3 className="text-lg font-medium leading-6 text-gray-900">
                <FormattedMessage id="shared.delete-modal.title" />
              </h3>
              <div className="mt-2 text-sm">
                <p>
                  <FormattedMessage id="assets.settings.general.delete-asset.question" />
                </p>
                <p className="mt-2">
                  <FormattedMessage id="assets.settings.general.delete-asset.warning" />
                </p>
                <p className="mt-2">
                  <FormattedMessage
                    id="assets.settings.general.delete-asset.type-name"
                    values={{
                      name: (
                        <span className="font-semibold">
                          {currentAsset.data?.name}
                        </span>
                      )
                    }}
                  />
                </p>
              </div>
              <div className="mt-4">
                <FormikTextInput
                  name={nameOf<DeleteAssetFormValues>("name")}
                  value={values.name}
                />
              </div>
            </DeleteModalContainer>
          </Form>
        )}
      </Formik>
    </Modal>
  );
}
