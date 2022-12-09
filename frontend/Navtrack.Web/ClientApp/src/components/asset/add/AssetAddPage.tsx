import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import Button from "../../ui/shared/button/Button";
import FormikTextInput from "../../ui/shared/text-input/FormikTextInput";
import FormikSelectInput from "../../ui/shared/select/FormikSelectInput";
import {
  AddAssetFormValues,
  DefaultAddAssetFormValues
} from "./AddAssetFormValues";
import useAddAsset from "./useAddAsset";
import DeviceConfiguration from "./DeviceConfiguration";
import { useState } from "react";
import { DeviceTypeModel } from "@navtrack/navtrack-app-shared/dist/api/model/generated";

export default function AssetAddPage() {
  const { deviceTypes, validationSchema, handleSubmit, loading } =
    useAddAsset();
  const [selectedDeviceType, setSelectedDeviceType] =
    useState<DeviceTypeModel>();

  return (
    <div>
      <Formik<AddAssetFormValues>
        initialValues={DefaultAddAssetFormValues}
        validationSchema={validationSchema}
        enableReinitialize
        onSubmit={(values, formikHelpers) =>
          handleSubmit(values, formikHelpers)
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
                  loading={loading}
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
