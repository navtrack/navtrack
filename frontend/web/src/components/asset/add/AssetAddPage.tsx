import { Form, Formik } from "formik";
import { FormattedMessage, useIntl } from "react-intl";
import { Button } from "../../ui/shared/button/Button";
import { FormikTextInput } from "../../ui/shared/text-input/FormikTextInput";
import { FormikSelectInput } from "../../ui/shared/select/FormikSelectInput";
import { useHistory } from "react-router-dom";
import { DeviceConfiguration } from "./DeviceConfiguration";
import { useState } from "react";
import { DeviceTypeModel } from "@navtrack/shared/api/model/generated";
import {
  AddAssetFormValues,
  DefaultAddAssetFormValues
} from "@navtrack/shared/hooks/assets/add-asset/AddAssetFormValues";
import { useAddAssetValidationSchema } from "@navtrack/shared/hooks/assets/add-asset/useAddAssetValidationSchema";
import { useAddAsset } from "@navtrack/shared/hooks/assets/add-asset/useAddAsset";
import { useScrollToAsset } from "../../ui/layouts/admin/useScrollToAsset";
import { useNotification } from "../../ui/shared/notification/useNotification";
import { useDeviceTypes } from "@navtrack/shared/hooks/devices/useDeviceTypes";
import { useGetAssetsQuery } from "@navtrack/shared/hooks/queries/useGetAssetsQuery";

export function AssetAddPage() {
  const { deviceTypes } = useDeviceTypes();
  const intl = useIntl();
  const history = useHistory();
  const { showNotification } = useNotification();
  const assetsQuery = useGetAssetsQuery();
  const { setScrollToAsset } = useScrollToAsset();
  const addAsset = useAddAsset({
    onSuccess: (asset) => {
      showNotification({
        type: "success",
        description: intl.formatMessage({ id: "assets.add.success" })
      });
      assetsQuery.refetch().then(() => {
        setScrollToAsset(asset.id);
      });
      history.push(`/assets/${asset.id}/live`);
    }
  });
  const validationSchema = useAddAssetValidationSchema();
  const [selectedDeviceType, setSelectedDeviceType] =
    useState<DeviceTypeModel>();

  return (
    <div>
      <Formik<AddAssetFormValues>
        initialValues={DefaultAddAssetFormValues}
        validationSchema={validationSchema}
        enableReinitialize
        onSubmit={(values, formikHelpers) =>
          addAsset.handleSubmit(values, formikHelpers)
        }>
        {() => (
          <Form>
            <div className="overflow-hidden rounded-lg shadow">
              <div className="bg-white p-6 px-4 py-5">
                <div>
                  <h2 className="text-lg font-medium leading-6 text-gray-900">
                    <FormattedMessage id="assets.add.title" />
                  </h2>
                </div>
                <div className="mt-6 grid grid-cols-6 gap-6">
                  <div className="col-span-3 grid gap-6">
                    <FormikTextInput
                      name="name"
                      label="generic.name"
                      placeholder="assets.add.name.placeholder"
                    />
                    <FormikSelectInput
                      name="deviceTypeId"
                      label="generic.device-type"
                      placeholder="Select a device type"
                      items={deviceTypes.map((x) => ({
                        id: x.id,
                        label: x.displayName
                      }))}
                      onChange={(value) => {
                        setSelectedDeviceType(
                          deviceTypes.find((d) => d.id === value)
                        );
                      }}
                    />
                    <FormikTextInput
                      name="serialNumber"
                      label="generic.serial-number"
                      placeholder="assets.add.serial-number.placeholder"
                    />
                  </div>
                  <div className="col-span-3 grid gap-6">
                    <DeviceConfiguration deviceType={selectedDeviceType} />
                  </div>
                </div>
              </div>
              <div className="bg-gray-50 px-6 py-3 text-right">
                <Button
                  color="primary"
                  type="submit"
                  loading={addAsset.isLoading}
                  size="lg">
                  <FormattedMessage id="generic.save" />
                </Button>
              </div>
            </div>
          </Form>
        )}
      </Formik>
    </div>
  );
}
