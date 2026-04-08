import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { FormikTextInput } from "../ui/form/text-input/FormikTextInput";
import { nameOf } from "@navtrack/shared/utils/typescript";
import { DeleteModal } from "../ui/modal/DeleteModal";
import { Button } from "../ui/button/Button";
import { ObjectSchema, object, string } from "yup";
import { useDeleteAccountMutation } from "@navtrack/shared/hooks/queries/account/useDeleteAccountMutation";
import { useAuthentication } from "@navtrack/shared/hooks/app/authentication/useAuthentication";
import { useNotification } from "../ui/notification/useNotification";
import { mapErrors } from "@navtrack/shared/utils/formik";

export type DeleteAccountFormValues = {
  password: string;
};

export function DeleteAccountModal() {
  const { logout } = useAuthentication();
  const { showNotification } = useNotification();

  const deleteAccountMutation = useDeleteAccountMutation({
    onSuccess: () => {
      logout();
      showNotification({
        type: "success",
        description: "settings.account.delete.success"
      });
    }
  });

  const validationSchema: ObjectSchema<DeleteAccountFormValues> = object()
    .shape({
      password: string().required("generic.password.required")
    })
    .defined();

  return (
    <Formik<DeleteAccountFormValues>
      enableReinitialize
      initialValues={{ password: "" }}
      validationSchema={validationSchema}
      onSubmit={(values, formikHelpers) => {
        return deleteAccountMutation.mutateAsync(
          {
            data: { password: values.password }
          },
          {
            onError: (error) => mapErrors(error, formikHelpers)
          }
        );
      }}>
      {({ values, submitForm, resetForm, status }) => (
        <DeleteModal
          autoClose={false}
          maxWidth="lg"
          isLoading={deleteAccountMutation.isPending}
          onClose={() => resetForm()}
          onConfirm={() => submitForm()}
          renderButton={(open) => (
            <Button color="error" type="submit" size="base" onClick={open}>
              <FormattedMessage id="settings.account.delete.title" />
            </Button>
          )}>
          <Form>
            <div className="mt-2 text-sm">
              <p>
                <FormattedMessage id="settings.account.delete.question" />
              </p>
              <p className="mt-4">
                <FormattedMessage id="settings.account.delete.confirm" />
              </p>
            </div>
            <div className="mt-2">
              <FormikTextInput
                name={nameOf<DeleteAccountFormValues>("password")}
                type="password"
                value={values.password}
              />
            </div>
            {status?.message && (
              <div className="mt-2 text-sm text-red-600">
                {status.message}
              </div>
            )}
          </Form>
        </DeleteModal>
      )}
    </Formik>
  );
}
