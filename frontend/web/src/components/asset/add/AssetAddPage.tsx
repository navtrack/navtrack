import { Form, Formik } from "formik";
import { FormattedMessage, useIntl } from "react-intl";
import { FormikTextInput } from "../../ui/form/text-input/FormikTextInput";
import { FormikCustomSelect } from "../../ui/form/select/FormikCustomSelect";
import { generatePath, useNavigate } from "react-router-dom";
import { DeviceConfiguration } from "./DeviceConfiguration";
import { useContext, useState } from "react";
import { DeviceTypeModel } from "@navtrack/shared/api/model/generated";
import {
  AddAssetFormValues,
  DefaultAddAssetFormValues
} from "@navtrack/shared/hooks/assets/add-asset/AddAssetFormValues";
import { useAddAssetValidationSchema } from "@navtrack/shared/hooks/assets/add-asset/useAddAssetValidationSchema";
import { useAddAsset } from "@navtrack/shared/hooks/assets/add-asset/useAddAsset";
import { useScrollToAsset } from "../../ui/layouts/authenticated/useScrollToAsset";
import { useNotification } from "../../ui/notification/useNotification";
import { useDeviceTypes } from "@navtrack/shared/hooks/devices/useDeviceTypes";
import { useAssetsQuery } from "@navtrack/shared/hooks/queries/useAssetsQuery";
import { Card } from "../../ui/card/Card";
import { Paths } from "../../../app/Paths";
import { Heading } from "../../ui/heading/Heading";
import { CardHeader } from "../../ui/card/CardHeader";
import { CardBody } from "../../ui/card/CardBody";
import { CardFooter } from "../../ui/card/CardFooter";
import { SlotContext } from "../../../app/SlotContext";
import { Button } from "../../ui/button/Button";

export function AssetAddPage() {
  const slot = useContext(SlotContext);
  const { deviceTypes } = useDeviceTypes();
  const intl = useIntl();
  const navigate = useNavigate();
  const { showNotification } = useNotification();
  const assetsQuery = useAssetsQuery();
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
      navigate(generatePath(Paths.AssetsLive, { id: asset.id }));
    }
  });
  const validationSchema = useAddAssetValidationSchema();
  const [selectedDeviceType, setSelectedDeviceType] =
    useState<DeviceTypeModel>();

  return (
    <>
      <Card>
        <Formik<AddAssetFormValues>
          initialValues={DefaultAddAssetFormValues}
          validationSchema={validationSchema}
          enableReinitialize
          onSubmit={(values, formikHelpers) =>
            addAsset.handleSubmit(values, formikHelpers)
          }>
          {() => (
            <Form>
              <CardHeader>
                <Heading type="h2">
                  <FormattedMessage id="assets.add.title" />
                </Heading>
              </CardHeader>
              <CardBody>
                <div className="grid grid-cols-6 gap-6">
                  <div className="col-span-3 grid gap-6">
                    <FormikTextInput
                      name="name"
                      label="generic.name"
                      placeholder="assets.add.name.placeholder"
                    />
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
              </CardBody>
              {slot?.assetAddFooterBlock}
              <CardFooter className="text-right">
                <Button type="submit" loading={addAsset.isLoading} size="lg">
                  <FormattedMessage id="generic.save" />
                </Button>
              </CardFooter>
            </Form>
          )}
        </Formik>
      </Card>
    </>
  );
}
