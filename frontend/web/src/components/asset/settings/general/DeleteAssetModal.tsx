import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { FormikTextInput } from "../../../ui/form/text-input/FormikTextInput";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { nameOf } from "@navtrack/shared/utils/typescript";
import { useDeleteAsset } from "@navtrack/shared/hooks/assets/useDeleteAsset";
import { useNotification } from "../../../ui/notification/useNotification";
import { generatePath, useNavigate } from "react-router-dom";
import { Paths } from "../../../../app/Paths";
import { DeleteModal } from "../../../ui/modal/DeleteModal";
import { Button } from "../../../ui/button/Button";
import { ObjectSchema, object, string } from "yup";
import { useContext } from "react";
import { SlotContext } from "../../../../app/SlotContext";

export type DeleteAssetFormValues = {
  name?: string; // TODO make required
};

export function DeleteAssetModal() {
  const slot = useContext(SlotContext);
  const currentAsset = useCurrentAsset();
  const navigate = useNavigate();
  const { showNotification } = useNotification();

  const deleteAssetMutation = useDeleteAsset({
    onSuccess: () => {
      navigate(
        generatePath(Paths.OrganizationLive, {
          id: currentAsset.data?.organizationId
        })
      );
      showNotification({
        type: "success",
        description: "assets.settings.general.delete-asset.success"
      });
    }
  });

  const validationSchema: ObjectSchema<DeleteAssetFormValues> = object()
    .shape({
      name: string()
        .required("generic.name.required")
        .equals(
          [`${currentAsset.data?.name}`],
          "assets.settings.general.delete-asset.name-match"
        )
    })
    .defined();

  return (
    <Formik<DeleteAssetFormValues>
      enableReinitialize
      initialValues={{ name: "" }}
      validationSchema={validationSchema}
      onSubmit={() => {
        if (currentAsset.data?.id) {
          return deleteAssetMutation.handleSubmit(currentAsset.data.id);
        }
      }}>
      {({ values, submitForm, resetForm }) => (
        <DeleteModal
          autoClose={false}
          maxWidth="lg"
          onClose={() => resetForm()}
          onConfirm={() => submitForm()}
          isLoading={deleteAssetMutation.isLoading}
          renderButton={(open) => (
            <Button color="error" type="submit" size="base" onClick={open}>
              <FormattedMessage id="assets.settings.general.delete-asset" />
            </Button>
          )}>
          <Form>
            <div className="mt-2 text-sm">
              <p>
                <FormattedMessage id="assets.settings.general.delete-asset.question" />
              </p>
              <p className="mt-2">
                <FormattedMessage id="assets.settings.general.delete-asset.warning" />
              </p>
              <p className="mt-2">
                <FormattedMessage
                  id="assets.settings.general.delete-asset.type-name"
                  values={{
                    name: (
                      <span className="font-semibold">
                        {currentAsset.data?.name}
                      </span>
                    )
                  }}
                />
              </p>
            </div>
            <div className="mt-4">
              <FormikTextInput
                name={nameOf<DeleteAssetFormValues>("name")}
                value={values.name}
              />
            </div>
          </Form>
          {slot?.assetDeleteModalBlock}
        </DeleteModal>
      )}
    </Formik>
  );
}
