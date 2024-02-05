import { FormattedMessage, useIntl } from "react-intl";
import { useCallback, useEffect, useState } from "react";
import { Formik, Form, FormikHelpers } from "formik";
import { FormikCustomSelect } from "../../../ui/form/select/FormikCustomSelect";
import { FormikTextInput } from "../../../ui/form/text-input/FormikTextInput";
import { DeviceConfiguration } from "../../add/DeviceConfiguration";
import { DevicesTable } from "./DevicesTable";
import { useNotification } from "../../../ui/notification/useNotification";
import { object, ObjectSchema, string } from "yup";
import { DeviceTypeModel } from "@navtrack/shared/api/model/generated";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { useChangeDeviceMutation } from "@navtrack/shared/hooks/mutations/assets/useChangeDeviceMutation";
import { useAssetDevicesQuery } from "@navtrack/shared/hooks/queries/useAssetDevicesQuery";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { useDeviceTypes } from "@navtrack/shared/hooks/devices/useDeviceTypes";
import { Card } from "../../../ui/card/Card";
import { CardBody } from "../../../ui/card/CardBody";
import { Heading } from "../../../ui/heading/Heading";
import { Button } from "../../../ui/button/Button";

type ChangeDeviceFormValues = {
  serialNumber: string;
  deviceTypeId: string;
};

export function AssetSettingsDevicePage() {
  const currentAsset = useCurrentAsset();

  const [selectedDeviceType, setSelectedDeviceType] =
    useState<DeviceTypeModel>();

  const mutation = useChangeDeviceMutation();
  const { showNotification } = useNotification();
  const intl = useIntl();

  const devices = useAssetDevicesQuery(currentAsset?.data?.id);
  const { deviceTypes } = useDeviceTypes();

  const handleSubmit = useCallback(
    (
      values: ChangeDeviceFormValues,
      formikHelpers: FormikHelpers<ChangeDeviceFormValues>
    ) => {
      mutation.mutate(
        {
          assetId: `${currentAsset.data?.id}`,
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
    [currentAsset.data?.id, devices, intl, mutation, showNotification]
  );

  useEffect(() => {
    if (!selectedDeviceType) {
      setSelectedDeviceType(
        deviceTypes.find(
          (x) => x.id === currentAsset.data?.device.deviceType.id
        )
      );
    }
  }, [
    currentAsset.data?.device.deviceType.id,
    deviceTypes,
    selectedDeviceType
  ]);

  const validationSchema: ObjectSchema<ChangeDeviceFormValues> = object({
    deviceTypeId: string().required(
      intl.formatMessage({ id: "generic.device-type.required" })
    ),
    serialNumber: string().required(
      intl.formatMessage({ id: "generic.serial-number.required" })
    )
  }).defined();

  return (
    <>
      {currentAsset.data && deviceTypes && (
        <Card>
          <CardBody>
            <Heading type="h2">
              <FormattedMessage id="assets.settings.device.title" />
            </Heading>
            <div className="mt-4 grid grid-cols-6 space-x-6">
              <div className="col-span-3">
                <Formik<ChangeDeviceFormValues>
                  initialValues={{
                    serialNumber: currentAsset.data.device.serialNumber,
                    deviceTypeId: currentAsset.data.device.deviceType.id
                  }}
                  validationSchema={validationSchema}
                  enableReinitialize
                  onSubmit={(values, formikHelpers) =>
                    handleSubmit(values, formikHelpers)
                  }>
                  {() => (
                    <Form>
                      <div className="col-span-3 space-y-3">
                        <FormikCustomSelect
                          name="deviceTypeId"
                          label="generic.device-type"
                          placeholder="Select a device type"
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
                        />
                        <div className="text-right">
                          <Button
                            color="secondary"
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
            <div className="mt-6">
              <Heading type="h2">
                <FormattedMessage id="assets.settings.device.history" />
              </Heading>
              <div className="mt-4">
                <DevicesTable
                  assetId={currentAsset.data.id}
                  rows={devices.data?.items}
                  loading={devices.isLoading}
                  refresh={devices.refetch}
                />
              </div>
            </div>
          </CardBody>
        </Card>
      )}
    </>
  );
}
