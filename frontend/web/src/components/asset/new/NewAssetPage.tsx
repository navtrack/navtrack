import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { FormikTextInput } from "../../ui/form/text-input/FormikTextInput";
import { FormikAutocomplete } from "../../ui/form/autocomplete/FormikAutocomplete";
import { generatePath, useNavigate } from "react-router-dom";
import { DeviceConfiguration } from "./DeviceConfiguration";
import { useContext, useState } from "react";
import { DeviceType } from "@navtrack/shared/api/model/generated";
import {
  CreateAssetFormValues,
  DefaultCreateAssetFormValues,
  useCreateAsset
} from "@navtrack/shared/hooks/assets/useCreateAsset";
import { useScrollToAsset } from "../../ui/layouts/authenticated/useScrollToAsset";
import { useNotification } from "../../ui/notification/useNotification";
import { useDeviceTypes } from "@navtrack/shared/hooks/devices/useDeviceTypes";
import { useAssetsQuery } from "@navtrack/shared/hooks/queries/assets/useAssetsQuery";
import { Card } from "../../ui/card/Card";
import { Paths } from "../../../app/Paths";
import { Heading } from "../../ui/heading/Heading";
import { CardHeader } from "../../ui/card/CardHeader";
import { CardBody } from "../../ui/card/CardBody";
import { CardFooter } from "../../ui/card/CardFooter";
import { SlotContext } from "../../../app/SlotContext";
import { Button } from "../../ui/button/Button";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";

export function NewAssetPage() {
  const slot = useContext(SlotContext);
  const { deviceTypes } = useDeviceTypes();
  const navigate = useNavigate();
  const { showNotification } = useNotification();
  const currentOrganization = useCurrentOrganization();
  const assetsQuery = useAssetsQuery({
    organizationId: currentOrganization.data?.id
  });
  const { setScrollToAsset } = useScrollToAsset();
  const createAsset = useCreateAsset({
    onSuccess: (asset) => {
      showNotification({
        type: "success",
        description: "assets.add.success"
      });
      assetsQuery.refetch().then(() => {
        setScrollToAsset(asset.id);
      });
      navigate(generatePath(Paths.AssetLive, { id: asset.id }));
    }
  });
  const [selectedDeviceType, setSelectedDeviceType] = useState<DeviceType>();

  return (
    <>
      <Card>
        <Formik<CreateAssetFormValues>
          initialValues={DefaultCreateAssetFormValues}
          validationSchema={createAsset.validationSchema}
          enableReinitialize
          onSubmit={(values, formikHelpers) =>
            createAsset.handleSubmit(values, formikHelpers)
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
                    <FormikAutocomplete
                      name="deviceTypeId"
                      label="generic.device-type"
                      placeholder="devices.search-placeholder"
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
                <Button
                  type="submit"
                  isLoading={createAsset.isLoading}
                  size="lg">
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
