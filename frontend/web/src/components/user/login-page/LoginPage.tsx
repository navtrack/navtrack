import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { Button } from "../../ui/shared/button/Button";
import { Icon } from "../../ui/shared/icon/Icon";
import { Card } from "../../ui/shared/card/Card";
import { Link } from "../../ui/shared/link/Link";
import { Copyright } from "../../shared/Copyright";
import { faLock } from "@fortawesome/free-solid-svg-icons";
import { FormikTextInput } from "../../ui/shared/text-input/FormikTextInput";
import { TextInputLeftAddon } from "../../ui/shared/text-input/TextInputLeftAddon";
import { faEnvelope } from "@fortawesome/free-regular-svg-icons";
import { LoadingIndicator } from "../../ui/shared/loading-indicator/LoadingIndicator";
import Paths from "../../../app/Paths";
import { ExternalLogin } from "./external-login/ExternalLogin";
import { Alert } from "../../ui/shared/alert/Alert";
import { useLoginFormValidationSchema } from "@navtrack/shared/hooks/user/login/useLoginFormValidationSchema";
import { useLogin } from "@navtrack/shared/hooks/app/authentication/useLogin";
import { InitialLoginFormValues } from "@navtrack/shared/hooks/user/login/LoginFormValues";
import { useMemo } from "react";
import { AuthenticationErrorType } from "@navtrack/shared/hooks/app/authentication/authentication";

export function LoginPage() {
  const login = useLogin();
  const validationSchema = useLoginFormValidationSchema();

  const errorMessage = useMemo(
    () =>
      login.error !== undefined
        ? login.error === AuthenticationErrorType.Internal
          ? "login.internal-login-error"
          : login.error === AuthenticationErrorType.Other
          ? "authentication.logged-out"
          : "generic.error-message"
        : undefined,
    [login.error]
  );

  return (
    <>
      <h2 className="mx-auto mt-4 text-3xl font-extrabold text-gray-900">
        <FormattedMessage id="login.title" />
      </h2>
      <Card className="mx-auto mt-8 w-full max-w-md p-8">
        {errorMessage !== undefined && (
          <Alert className="mb-3">
            <FormattedMessage id={errorMessage} />
          </Alert>
        )}
        <Formik
          initialValues={InitialLoginFormValues}
          onSubmit={(values) =>
            login.internalLogin({
              username: values.email,
              password: values.password
            })
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
                  disabled={login.loading}
                  fullWidth>
                  {login.loading ? (
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
        <ExternalLogin login={login.externalLogin} />
      </Card>
      <div className="mt-4 text-center text-xs">
        <Copyright />
      </div>
    </>
  );
}
