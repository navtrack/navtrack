import { AssetSettingsLayout } from "../layout/AssetSettingsLayout";
import { FormikTextInput } from "../../../ui/shared/text-input/FormikTextInput";
import { Form, Formik } from "formik";
import { useRenameAsset } from "./useRenameAsset";
import { Button } from "../../../ui/shared/button/Button";
import { FormattedMessage } from "react-intl";
import { DeleteAssetModal } from "./DeleteAssetModal";
import { useState } from "react";
import { Icon } from "../../../ui/shared/icon/Icon";
import { faCheck } from "@fortawesome/free-solid-svg-icons";
import { LoadingIndicator } from "../../../ui/shared/loading-indicator/LoadingIndicator";
import { RenameAssetFormValues } from "./types";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";

export function AssetSettingsGeneralPage() {
  const renameAsset = useRenameAsset();
  const currentAsset = useCurrentAsset();
  const [showModal, setShowModal] = useState(false);

  return (
    <>
      {currentAsset && (
        <AssetSettingsLayout>
          <div className="col-span-9 space-y-6 divide-y divide-gray-200">
            <div>
              <h2 className="text-lg font-medium leading-6 text-gray-900">
                <FormattedMessage id="assets.settings.general" />
              </h2>
              <div className="mt-6">
                <Formik<RenameAssetFormValues>
                  initialValues={{ name: `${currentAsset.data?.name}` }}
                  onSubmit={(values, formikHelpers) =>
                    renameAsset.submit(values, formikHelpers)
                  }
                  validationSchema={renameAsset.validationSchema}
                  enableReinitialize>
                  {() => (
                    <Form className="grid grid-cols-12 gap-6">
                      <div className="col-span-7">
                        <FormikTextInput
                          name="name"
                          label="generic.name"
                          rightAddon={
                            <div className="ml-2 flex items-center">
                              <Button color="white" type="submit" size="base">
                                <FormattedMessage id="assets.settings.general.rename" />
                              </Button>
                              <div className="ml-2 w-4">
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
            <div className="pt-6">
              <h2 className="text-lg font-medium leading-6 text-gray-900">
                <FormattedMessage id="assets.settings.general.delete-asset" />
              </h2>
              <p className="mt-2 text-sm text-gray-500">
                <FormattedMessage id="assets.settings.general.delete-asset.info" />
              </p>
              <div className="mt-4 text-right">
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
