import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import Button from "../ui/shared/button/Button";
import Icon from "../ui/shared/icon/Icon";
import { InitialLoginFormValues } from "./LoginFormValues";
import { useLoginFormValidationSchema } from "./useLoginFormValidationSchema";
import Card from "../ui/shared/card/Card";
import Link from "../ui/shared/link/Link";
import Copyright from "../shared/Copyright";
import { faLock } from "@fortawesome/free-solid-svg-icons";
import FormikTextInput from "../ui/shared/text-input/FormikTextInput";
import TextInputLeftAddon from "../ui/shared/text-input/TextInputLeftAddon";
import { faEnvelope } from "@fortawesome/free-regular-svg-icons";
import LoadingIndicator from "../ui/shared/loading-indicator/LoadingIndicator";
import Paths from "../../app/Paths";
import ExternalLogin from "./external-login/ExternalLogin";
import Alert from "../ui/shared/alert/Alert";
import { useLogin } from "@navtrack/navtrack-app-shared";
import { AUTHENTICATION } from "../../constants";

export default function LoginPage() {
  const validationSchema = useLoginFormValidationSchema();
  const {
    internalLogin,
    externalLogin,
    loading,
    internalLoginError,
    externalLoginError
  } = useLogin({
    clientId: AUTHENTICATION.CLIENT_ID
  });

  return (
    <>
      <h2 className="mx-auto mt-4 text-3xl font-extrabold text-gray-900">
        <FormattedMessage id="login.title" />
      </h2>
      <Card className="mx-auto mt-8 w-full max-w-md p-8">
        {internalLoginError && (
          <Alert className="mb-3">
            <FormattedMessage id="login.internal-login-error" />
          </Alert>
        )}
        {externalLoginError && (
          <Alert className="mb-3">
            <FormattedMessage id="generic.error-message" />
          </Alert>
        )}
        <Formik
          initialValues={InitialLoginFormValues}
          onSubmit={(values) =>
            internalLogin({ username: values.email, password: values.password })
          }
          validationSchema={validationSchema}>
          {({ values }) => (
            <Form className="space-y-2">
              <FormikTextInput
                name="email"
                label="generic.email"
                value={values.email}
                className="pl-8"
                leftAddon={
                  <TextInputLeftAddon>
                    <Icon icon={faEnvelope} />
                  </TextInputLeftAddon>
                }
              />
              <FormikTextInput
                name="password"
                label={"generic.password"}
                type="password"
                value={values.password}
                className="pl-8"
                leftAddon={
                  <TextInputLeftAddon>
                    <Icon icon={faLock} />
                  </TextInputLeftAddon>
                }
              />
              <div className="pt-2">
                <Button
                  type="submit"
                  color="primary"
                  size="lg"
                  disabled={loading}
                  fullWidth>
                  {loading ? (
                    <LoadingIndicator />
                  ) : (
                    <FormattedMessage id="login.button" />
                  )}
                </Button>
              </div>
            </Form>
          )}
        </Formik>
        <div className="mt-4 text-center text-sm font-medium">
          <Link to={Paths.Register} text="login.new.action" />
          <span className="mx-2">•</span>
          <Link to={Paths.ForgotPassword} text="login.forgot" />
        </div>
        <ExternalLogin login={externalLogin} />
      </Card>
      <div className="mt-4 text-center text-xs">
        <Copyright />
      </div>
    </>
  );
}
