import { Formik, Form } from "formik";
import { FormattedMessage } from "react-intl";
import { Link } from "react-router-dom";
import { Button } from "../../ui/shared/button/Button";
import { Card } from "../../ui/shared/card/Card";
import { FormikTextInput } from "../../ui/shared/text-input/FormikTextInput";
import { TextInputLeftAddon } from "../../ui/shared/text-input/TextInputLeftAddon";
import { Icon } from "../../ui/shared/icon/Icon";
import { faEnvelope } from "@fortawesome/free-regular-svg-icons";
import Paths from "../../../app/Paths";
import { InitialForgotPasswordFormValues } from "@navtrack/shared/hooks/user/forgot-password/ForgotPasswordFormValues";
import { useForgotPassword } from "@navtrack/shared/hooks/user/forgot-password/useForgotPassword";
import { useForgotPasswordFormValidationSchema } from "@navtrack/shared/hooks/user/forgot-password/useForgotPasswordFormValidationSchema";

export function ForgotPasswordPage() {
  const validationSchema = useForgotPasswordFormValidationSchema();
  const forgotPassword = useForgotPassword();

  return (
    <>
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
                      className="ml-1 text-sm font-medium text-blue-600">
                      <FormattedMessage id="forgot-password.go-home" />
                    </Link>
                  </div>
                </>
              ) : status?.code ? (
                <>
                  <div className="text-center">
                    <FormattedMessage id="forgot-password.error" />
                  </div>
                  <div className="mt-4 text-center text-sm font-medium">
                    <Link to={Paths.Home} className="ml-1 text-blue-600">
                      <FormattedMessage id="forgot-password.go-home" />
                    </Link>
                  </div>
                </>
              ) : (
                <>
                  <Form className="space-y-4">
                    <FormikTextInput
                      name="email"
                      label={"generic.email"}
                      value={values.email}
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
                        disabled={forgotPassword.loading}
                        fullWidth
                        loading={forgotPassword.loading}>
                        <FormattedMessage id="forgot-password.button" />
                      </Button>
                    </div>
                  </Form>
                  <div className="mt-4 text-center text-sm font-medium">
                    <span className="text-gray-600">
                      <FormattedMessage id="forgot-password.existing.question" />
                    </span>
                    <Link to="/login" className="ml-1 text-blue-600">
                      <FormattedMessage id="forgot-password.existing.action" />
                    </Link>
                  </div>
                </>
              )}
            </>
          )}
        </Formik>
      </Card>
    </>
  );
}
