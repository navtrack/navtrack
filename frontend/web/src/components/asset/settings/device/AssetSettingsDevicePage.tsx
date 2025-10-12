import { FormattedMessage } from "react-intl";
import { useCallback, useEffect, useState } from "react";
import { Formik, Form, FormikHelpers } from "formik";
import { FormikAutocomplete } from "../../../ui/form/autocomplete/FormikAutocomplete";
import { FormikTextInput } from "../../../ui/form/text-input/FormikTextInput";
import { DeviceConfiguration } from "../../new/DeviceConfiguration";
import { AssetDevicesTable } from "./AssetDevicesTable";
import { useNotification } from "../../../ui/notification/useNotification";
import { object, ObjectSchema, string } from "yup";
import { DeviceTypeModel } from "@navtrack/shared/api/model";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { useAssetDevicesQuery } from "@navtrack/shared/hooks/queries/assets/useAssetDevicesQuery";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { useDeviceTypes } from "@navtrack/shared/hooks/devices/useDeviceTypes";
import { Card } from "../../../ui/card/Card";
import { CardBody } from "../../../ui/card/CardBody";
import { Heading } from "../../../ui/heading/Heading";
import { Button } from "../../../ui/button/Button";
import { useChangeDeviceMutation } from "@navtrack/shared/hooks/queries/assets/useChangeDeviceMutation";

type ChangeDeviceFormValues = {
  serialNumber: string;
  deviceTypeId: string;
};

export function AssetSettingsDevicePage() {
  const { showNotification } = useNotification();
  const currentAsset = useCurrentAsset();
  const changeDeviceMutation = useChangeDeviceMutation();

  const [selectedDeviceType, setSelectedDeviceType] =
    useState<DeviceTypeModel>();

  const devices = useAssetDevicesQuery(currentAsset?.data?.id);
  const { deviceTypes } = useDeviceTypes();

  const handleSubmit = useCallback(
    (
      values: ChangeDeviceFormValues,
      formikHelpers: FormikHelpers<ChangeDeviceFormValues>
    ) => {
      changeDeviceMutation.mutate(
        {
          assetId: `${currentAsset.data?.id}`,
          data: {
            serialNumber: values.serialNumber,
            deviceTypeId: values.deviceTypeId
          }
        },
        {
          onSuccess: () => {
            showNotification({
              type: "success",
              description: "assets.settings.device.save.success"
            });
          },
          onError: (error) => mapErrors(error, formikHelpers)
        }
      );
    },
    [currentAsset.data?.id, changeDeviceMutation, showNotification]
  );

  useEffect(() => {
    if (!selectedDeviceType) {
      setSelectedDeviceType(
        deviceTypes.find(
          (x) => x.id === currentAsset.data?.device?.deviceType.id
        )
      );
    }
  }, [
    currentAsset.data?.device?.deviceType?.id,
    deviceTypes,
    selectedDeviceType
  ]);

  const validationSchema: ObjectSchema<ChangeDeviceFormValues> = object({
    deviceTypeId: string().required("generic.device-type.required"),
    serialNumber: string().required("generic.serial-number.required")
  }).defined();

  return (
    <Card>
      <CardBody>
        <Heading type="h2">
          <FormattedMessage id="assets.settings.device.title" />
        </Heading>
        <div className="mt-4 grid grid-cols-6 space-x-6">
          <div className="col-span-3">
            <Formik<ChangeDeviceFormValues>
              initialValues={{
                serialNumber: currentAsset.data?.device?.serialNumber ?? "",
                deviceTypeId: currentAsset.data?.device?.deviceType.id ?? ""
              }}
              validationSchema={validationSchema}
              enableReinitialize
              onSubmit={(values, formikHelpers) =>
                handleSubmit(values, formikHelpers)
              }>
              {() => (
                <Form>
                  <div className="col-span-3 space-y-3">
                    <FormikAutocomplete
                      name="deviceTypeId"
                      label="generic.device-type"
                      placeholder="Select a device type"
                      loading={
                        deviceTypes === undefined ||
                        currentAsset.data === undefined
                      }
                      options={deviceTypes.map((x) => ({
                        value: x.id,
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
                      loading={currentAsset.data === undefined}
                    />
                    <div className="text-right">
                      <Button
                        color="secondary"
                        type="submit"
                        disabled={
                          deviceTypes === undefined ||
                          currentAsset.data === undefined
                        }
                        isLoading={changeDeviceMutation.isPending}>
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
        <div className="mt-6">
          <Heading type="h2">
            <FormattedMessage id="assets.settings.device.history" />
          </Heading>
          <div className="mt-4">
            <AssetDevicesTable
              rows={devices.data?.items}
              loading={devices.isLoading}
            />
          </div>
        </div>
      </CardBody>
    </Card>
  );
}
