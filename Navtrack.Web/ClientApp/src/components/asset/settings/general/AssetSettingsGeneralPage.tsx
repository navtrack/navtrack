import AssetSettingsLayout from "../layout/AssetSettingsLayout";
import FormikTextInput from "../../../ui/shared/text-input/FormikTextInput";
import { Form, Formik } from "formik";
import useRenameAsset from "./useRenameAsset";
import Button from "../../../ui/shared/button/Button";
import { FormattedMessage } from "react-intl";
import DeleteAssetModal from "./DeleteAssetModal";
import { useState } from "react";
import useCurrentAsset from "../../../../hooks/assets/useCurrentAsset";
import Icon from "../../../ui/shared/icon/Icon";
import { faCheck } from "@fortawesome/free-solid-svg-icons";
import LoadingIndicator from "../../../ui/shared/loading-indicator/LoadingIndicator";

export default function AssetSettingsGeneralPage() {
  const renameAsset = useRenameAsset();
  const { currentAsset } = useCurrentAsset();
  const [showModal, setShowModal] = useState(false);

  return (
    <>
      {currentAsset && (
        <AssetSettingsLayout>
          <div className="divide-y divide-gray-200 col-span-9">
            <div className="py-6 px-4 sm:p-6 lg:pb-8">
              <div>
                <h2 className="text-lg leading-6 font-medium text-gray-900">
                  <FormattedMessage id="assets.settings.general" />
                </h2>
              </div>
              <div className="mt-6">
                <Formik<{ name: string }>
                  initialValues={{ name: `${currentAsset?.name}` }}
                  onSubmit={(values) => renameAsset.submit(values)}
                  validationSchema={renameAsset.validationSchema}
                  enableReinitialize>
                  {({ values }) => (
                    <Form className="grid grid-cols-12 gap-6">
                      <div className="col-span-7">
                        <FormikTextInput
                          name="name"
                          label="generic.name"
                          value={values.name}
                          rightAddon={
                            <div className="ml-2 flex items-center">
                              <Button color="white" type="submit" size="base">
                                <FormattedMessage id="assets.settings.general.rename" />
                              </Button>
                              <div className="w-4 ml-2">
                                {renameAsset.loading && <LoadingIndicator />}
                                {renameAsset.showSuccess && (
                                  <Icon
                                    icon={faCheck}
                                    className="text-green-600"
                                  />
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
            </div>
            <div className="p-6">
              <h2 className="text-lg leading-6 font-medium text-gray-900">
                <FormattedMessage id="assets.settings.general.delete-asset" />
              </h2>
              <p className="mt-2 text-sm text-gray-500">
                <FormattedMessage id="assets.settings.general.delete-asset.info" />
              </p>
              <div className="text-right mt-4">
                <Button
                  color="warn"
                  type="submit"
                  size="base"
                  onClick={() => setShowModal(true)}>
                  <FormattedMessage id="assets.settings.general.delete-asset" />
                </Button>
                <DeleteAssetModal
                  show={showModal}
                  close={() => setShowModal(false)}
                />
              </div>
            </div>
          </div>
        </AssetSettingsLayout>
      )}
    </>
  );
}
