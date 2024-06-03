import { Formik, Form } from "formik";
import { FormattedMessage } from "react-intl";
import { Card } from "../../ui/card/Card";
import { FormikTextInput } from "../../ui/form/text-input/FormikTextInput";
import { TextInputLeftAddon } from "../../ui/form/text-input/TextInputLeftAddon";
import { Icon } from "../../ui/icon/Icon";
import { faEnvelope } from "@fortawesome/free-regular-svg-icons";
import { Paths } from "../../../app/Paths";
import { InitialForgotPasswordFormValues } from "@navtrack/shared/hooks/user/forgot-password/ForgotPasswordFormValues";
import { useForgotPassword } from "@navtrack/shared/hooks/user/forgot-password/useForgotPassword";
import { useForgotPasswordFormValidationSchema } from "@navtrack/shared/hooks/user/forgot-password/useForgotPasswordFormValidationSchema";
import { Link } from "../../ui/link/Link";
import { UnauthenticatedLayout } from "../../ui/layouts/unauthenticated/UnauthenticatedLayout";
import { Button } from "../../ui/button/Button";

export function ForgotPasswordPage() {
  const validationSchema = useForgotPasswordFormValidationSchema();
  const forgotPassword = useForgotPassword();

  return (
    <UnauthenticatedLayout>
      <h2 className="mx-auto mt-4 text-3xl font-extrabold text-gray-900">
        <FormattedMessage id="forgot-password.title" />
      </h2>
      <Card className="mx-auto mt-8 w-full max-w-md p-8">
        <Formik
          initialValues={InitialForgotPasswordFormValues}
          onSubmit={(values, formikHelpers) =>
            forgotPassword.resetPassword(values, formikHelpers)
          }
          validationSchema={() => validationSchema}
          enableReinitialize>
          {({ values, status }) => (
            <>
              {forgotPassword.success ? (
                <>
                  <div className="text-center">
                    <FormattedMessage id="forgot-password.success" />
                  </div>
                  <div className="mt-4 text-center">
                    <Link
                      to={Paths.Home}
                      className="ml-1"
                      label="forgot-password.go-home"
                    />
                  </div>
                </>
              ) : status?.code ? (
                <>
                  <div className="text-center">
                    <FormattedMessage id="forgot-password.error" />
                  </div>
                  <div className="mt-4 text-center text-sm font-medium">
                    <Link
                      to={Paths.Home}
                      className="ml-1"
                      label="forgot-password.go-home"
                    />
                  </div>
                </>
              ) : (
                <>
                  <Form className="space-y-3">
                    <FormikTextInput
                      name="email"
                      label={"generic.email"}
                      value={values.email}
                      autoComplete="username"
                      className="pl-8"
                      leftAddon={
                        <TextInputLeftAddon>
                          <Icon icon={faEnvelope} />
                        </TextInputLeftAddon>
                      }
                    />
                    <div className="pt-2">
                      <Button
                        type="submit"
                        size="lg"
                        color="secondary"
                        full
                        isLoading={forgotPassword.loading}>
                        <FormattedMessage id="forgot-password.button" />
                      </Button>
                    </div>
                  </Form>
                  <div className="mt-4 text-center text-sm font-medium">
                    <span className="text-gray-600">
                      <FormattedMessage id="forgot-password.existing.question" />
                    </span>
                    <Link
                      to={Paths.Home}
                      className="ml-1"
                      label="forgot-password.existing.action"
                    />
                  </div>
                </>
              )}
            </>
          )}
        </Formik>
      </Card>
    </UnauthenticatedLayout>
  );
}
