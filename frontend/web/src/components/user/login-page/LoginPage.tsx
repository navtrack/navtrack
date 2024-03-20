import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { Icon } from "../../ui/icon/Icon";
import { Card } from "../../ui/card/Card";
import { Link } from "../../ui/link/Link";
import { Copyright } from "../../shared/Copyright";
import { faLock } from "@fortawesome/free-solid-svg-icons";
import { FormikTextInput } from "../../ui/form/text-input/FormikTextInput";
import { TextInputLeftAddon } from "../../ui/form/text-input/TextInputLeftAddon";
import { faEnvelope } from "@fortawesome/free-regular-svg-icons";
import { Paths } from "../../../app/Paths";
import { Alert } from "../../ui/alert/Alert";
import { useLoginFormValidationSchema } from "@navtrack/shared/hooks/user/login/useLoginFormValidationSchema";
import { useLogin } from "@navtrack/shared/hooks/app/authentication/useLogin";
import { InitialLoginFormValues } from "@navtrack/shared/hooks/user/login/LoginFormValues";
import { useContext, useMemo } from "react";
import { AuthenticationErrorType } from "@navtrack/shared/hooks/app/authentication/authentication";
import { UnauthenticatedLayout } from "../../ui/layouts/unauthenticated/UnauthenticatedLayout";
import { Button } from "../../ui/button/Button";
import { SlotContext } from "../../../app/SlotContext";
import { isLoggingInAtom } from "@navtrack/shared/state/authentication";
import { useRecoilValue } from "recoil";

export function LoginPage() {
  const login = useLogin();
  const validationSchema = useLoginFormValidationSchema();
  const slots = useContext(SlotContext);
  const isLoading = useRecoilValue(isLoggingInAtom);

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
    <UnauthenticatedLayout>
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
                  color="secondary"
                  size="lg"
                  isLoading={isLoading}
                  full>
                  <FormattedMessage id="login.button" />
                </Button>
              </div>
            </Form>
          )}
        </Formik>
        <div className="mt-4 text-center text-sm font-medium">
          <Link to={Paths.Register} label="login.new.action" />
          <span className="mx-2">•</span>
          <Link to={Paths.ForgotPassword} label="login.forgot" />
        </div>
        {slots?.externalLogin}
      </Card>
      <div className="mt-4 text-center text-xs">
        <Copyright />
      </div>
    </UnauthenticatedLayout>
  );
}
