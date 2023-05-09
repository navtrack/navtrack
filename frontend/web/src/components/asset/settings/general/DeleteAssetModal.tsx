import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { FormikTextInput } from "../../../ui/shared/text-input/FormikTextInput";
import { DeleteAssetFormValues } from "./types";
import { useDeleteAsset } from "./useDeleteAsset";
import { Modal } from "../../../ui/shared/modal/Modal";
import { DeleteModalContainer } from "../../../ui/shared/modal/DeleteModalContainer";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { nameOf } from "@navtrack/shared/utils/typescript";

interface IDeleteAssetModal {
  show: boolean;
  close: () => void;
}

export function DeleteAssetModal(props: IDeleteAssetModal) {
  const currentAsset = useCurrentAsset();
  const { handleSubmit, validationSchema, loading } = useDeleteAsset();

  return (
    <Modal open={props.show} close={props.close} className="max-w-md">
      <Formik<DeleteAssetFormValues>
        initialValues={{ name: "" }}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}>
        {({ values }) => (
          <Form>
            <DeleteModalContainer close={props.close} loading={loading}>
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
