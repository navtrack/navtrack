import { Formik, Form } from "formik";
import { FormattedMessage } from "react-intl";
import { Card } from "../../ui/card/Card";
import { InitialResetPasswordFormValues } from "./ResetPasswordFormValues";
import { useResetPassword } from "./useResetPassword";
import { useResetPasswordFormValidationSchema } from "./useResetPasswordFormValidationSchema";
import { FormikTextInput } from "../../ui/form/text-input/FormikTextInput";
import { TextInputLeftAddon } from "../../ui/form/text-input/TextInputLeftAddon";
import { Icon } from "../../ui/icon/Icon";
import { Paths } from "../../../app/Paths";
import { faLock } from "@fortawesome/free-solid-svg-icons";
import { Link } from "../../ui/link/Link";
import { ErrorMessage } from "@navtrack/shared/components/ui/ErrorMessage";
import { UnauthenticatedLayout } from "../../ui/layouts/unauthenticated/UnauthenticatedLayout";
import { Button } from "../../ui/button/Button";

export const ResetPasswordPage = () => {
  const validationSchema = useResetPasswordFormValidationSchema();
  const resetPassword = useResetPassword();

  return (
    <UnauthenticatedLayout>
      <h2 className="mx-auto mt-4 text-3xl font-extrabold text-gray-900">
        <FormattedMessage id="reset-password.title" />
      </h2>
      <Card className="mx-auto mt-8 w-full max-w-md p-8">
        <Formik
          initialValues={InitialResetPasswordFormValues}
          onSubmit={(values, formikHelpers) =>
            resetPassword.resetPassword(values, formikHelpers)
          }
          validationSchema={() => validationSchema}
          enableReinitialize>
          {({ values, status }) => (
            <>
              {resetPassword.success ? (
                <>
                  <div className="text-center">
                    <FormattedMessage id="reset-password.success" />
                  </div>
                  <div className="mt-4 text-center text-sm font-medium">
                    <Link to={Paths.Home} label="reset-password.go-home" />
                  </div>
                </>
              ) : status?.code ? (
                <>
                  <div className="text-center">
                    <ErrorMessage code={status?.code} />
                  </div>
                  <div className="mt-4 text-center text-sm font-medium">
                    <Link
                      to={Paths.ForgotPassword}
                      label="reset-password.forgot"
                    />
                    <span className="mx-2">•</span>
                    <Link to={Paths.Home} label="reset-password.go-home" />
                  </div>
                </>
              ) : (
                <>
                  <Form className="space-y-4">
                    <FormikTextInput
                      name="password"
                      label={"generic.password"}
                      type="password"
                      value={values.password}
                      autoComplete="new-password"
                      className="pl-8"
                      leftAddon={
                        <TextInputLeftAddon>
                          <Icon icon={faLock} />
                        </TextInputLeftAddon>
                      }
                    />
                    <FormikTextInput
                      name="confirmPassword"
                      label="generic.confirm-password"
                      className="pl-8"
                      type="password"
                      autoComplete="new-password"
                      value={values.confirmPassword}
                      leftAddon={
                        <TextInputLeftAddon>
                          <Icon icon={faLock} />
                        </TextInputLeftAddon>
                      }
                    />
                    <div className="pt-2">
                      <Button
                        type="submit"
                        size="lg"
                        full
                        isLoading={resetPassword.loading}>
                        <FormattedMessage id="reset-password.button" />
                      </Button>
                    </div>
                  </Form>
                  <div className="mt-4 text-center text-sm font-medium">
                    <Link to="/login" label="reset-password.existing.action" />
                  </div>
                </>
              )}
            </>
          )}
        </Formik>
      </Card>
    </UnauthenticatedLayout>
  );
};
