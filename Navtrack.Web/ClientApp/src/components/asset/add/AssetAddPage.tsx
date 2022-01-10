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
import { DeviceTypeModel } from "../../../api/model";

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
            <div className="shadow overflow-hidden rounded-lg">
              <div className="px-4 py-5 bg-white p-6">
                <div>
                  <h2 className="text-lg leading-6 font-medium text-gray-900">
                    <FormattedMessage id="assets.add.title" />
                  </h2>
                </div>
                <div className="grid grid-cols-6 gap-6 mt-6">
                  <div className="grid col-span-3 gap-6">
                    <FormikTextInput
                      name="name"
                      label="generic.name"
                      placeholder="assets.add.name.placeholder"
                    />
                    <FormikSelectInput
                      name="deviceTypeId"
                      label="generic.device-type"
                      placeholder="Select a device type"
                      items={deviceTypes}
                      idKey="id"
                      labelKey="displayName"
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
                  <div className="grid col-span-3 gap-6">
                    <DeviceConfiguration deviceType={selectedDeviceType} />
                  </div>
                </div>
              </div>
              <div className="px-6 py-3 bg-gray-50 text-right">
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
