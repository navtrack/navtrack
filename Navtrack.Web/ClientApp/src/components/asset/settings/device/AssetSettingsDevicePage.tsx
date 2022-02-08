import AssetSettingsLayout from "../layout/AssetSettingsLayout";
import { FormattedMessage, useIntl } from "react-intl";
import Button from "../../../ui/shared/button/Button";
import { useCallback, useEffect, useState } from "react";
import { Formik, Form, FormikHelpers } from "formik";
import FormikSelectInput from "../../../ui/shared/select/FormikSelectInput";
import FormikTextInput from "../../../ui/shared/text-input/FormikTextInput";
import DeviceConfiguration from "../../add/DeviceConfiguration";
import DevicesTable from "./DevicesTable";
import useNotification from "../../../ui/shared/notification/useNotification";
import { object, SchemaOf, string } from "yup";
import { DeviceTypeModel } from "@navtrack/navtrack-app-shared/dist/api/model/generated";
import {
  mapErrors,
  useAddDeviceMutation,
  useAssetDevicesQuery,
  useCurrentAsset,
  useDevicesTypesQuery
} from "@navtrack/navtrack-app-shared";

type ChangeDeviceFormValues = {
  serialNumber: string;
  deviceTypeId: string;
};

export default function AssetSettingsDevicePage() {
  const currentAsset = useCurrentAsset();

  const [selectedDeviceType, setSelectedDeviceType] =
    useState<DeviceTypeModel>();

  const mutation = useAddDeviceMutation();
  const { showNotification } = useNotification();
  const intl = useIntl();

  const devices = useAssetDevicesQuery(currentAsset?.id);
  const { deviceTypes } = useDevicesTypesQuery();

  const handleSubmit = useCallback(
    (
      values: ChangeDeviceFormValues,
      formikHelpers: FormikHelpers<ChangeDeviceFormValues>
    ) => {
      mutation.mutate(
        {
          assetId: `${currentAsset?.id}`,
          data: {
            serialNumber: values.serialNumber,
            deviceTypeId: values.deviceTypeId
          }
        },
        {
          onSuccess: () => {
            devices.refetch();
            showNotification({
              type: "success",
              description: intl.formatMessage({
                id: "assets.settings.device.success"
              })
            });
          },
          onError: (error) => mapErrors(error, formikHelpers)
        }
      );
    },
    [currentAsset?.id, devices, intl, mutation, showNotification]
  );

  useEffect(() => {
    if (!selectedDeviceType) {
      setSelectedDeviceType(
        deviceTypes.find((x) => x.id === currentAsset?.device.deviceType.id)
      );
    }
  }, [currentAsset?.device.deviceType.id, deviceTypes, selectedDeviceType]);

  const validationSchema: SchemaOf<ChangeDeviceFormValues> = object({
    deviceTypeId: string()
      .required(intl.formatMessage({ id: "generic.device-type.required" }))
      .defined(),
    serialNumber: string()
      .required(intl.formatMessage({ id: "generic.serial-number.required" }))
      .defined()
  }).defined();

  return (
    <>
      {currentAsset && deviceTypes && (
        <AssetSettingsLayout>
          <h2 className="text-lg font-medium leading-6 text-gray-900">
            <FormattedMessage id="assets.settings.device.title" />
          </h2>
          <div className="mt-6 grid grid-cols-6 space-x-6">
            <div className="col-span-3">
              <Formik<ChangeDeviceFormValues>
                initialValues={{
                  serialNumber: currentAsset?.device.serialNumber,
                  deviceTypeId: currentAsset?.device.deviceType.id
                }}
                validationSchema={validationSchema}
                enableReinitialize
                onSubmit={(values, formikHelpers) =>
                  handleSubmit(values, formikHelpers)
                }>
                {() => (
                  <Form>
                    <div className="col-span-3 space-y-3">
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
                            deviceTypes.find((x) => x.id === value)
                          );
                        }}
                      />
                      <FormikTextInput
                        name="serialNumber"
                        label="generic.serial-number"
                        placeholder="assets.add.serial-number.placeholder"
                      />
                      <div className="text-right">
                        <Button
                          color="primary"
                          type="submit"
                          loading={mutation.isLoading}>
                          <FormattedMessage id="generic.save" />
                        </Button>
                      </div>
                    </div>
                  </Form>
                )}
              </Formik>
            </div>
            <div className="col-span-3">
              <DeviceConfiguration deviceType={selectedDeviceType} />
            </div>
          </div>
          <h2 className="mt-6 mb-4 text-lg font-medium leading-6 text-gray-900">
            <FormattedMessage id="assets.settings.device.history" />
          </h2>
          <DevicesTable
            assetId={currentAsset.id}
            rows={devices.data?.items}
            loading={devices.isLoading}
            refresh={devices.refetch}
          />
        </AssetSettingsLayout>
      )}
    </>
  );
}
