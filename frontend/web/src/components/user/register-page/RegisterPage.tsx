import { Formik, Form } from "formik";
import { FormattedMessage } from "react-intl";
import { Button } from "../../ui/shared/button/Button";
import { Card } from "../../ui/shared/card/Card";
import { FormikTextInput } from "../../ui/shared/text-input/FormikTextInput";
import { Link } from "../../ui/shared/link/Link";
import Paths from "../../../app/Paths";
import { TextInputLeftAddon } from "../../ui/shared/text-input/TextInputLeftAddon";
import { Icon } from "../../ui/shared/icon/Icon";
import { faEnvelope } from "@fortawesome/free-regular-svg-icons";
import { faLock } from "@fortawesome/free-solid-svg-icons";
import { ExternalLogin } from "../login-page/external-login/ExternalLogin";
import { AUTHENTICATION } from "../../../constants";
import { useRegister } from "@navtrack/shared/hooks/user/register/useRegister";
import { useRegisterFormValidationSchema } from "@navtrack/shared/hooks/user/register/useRegisterFormValidationSchema";
import { useLogin } from "@navtrack/shared/hooks/app/authentication/useLogin";
import { InitialRegisterFormValues } from "@navtrack/shared/hooks/user/register/RegisterFormValues";

export function RegisterPage() {
  const validationSchema = useRegisterFormValidationSchema();
  const register = useRegister();
  const { externalLogin } = useLogin({
    clientId: AUTHENTICATION.CLIENT_ID
  });

  return (
    <>
      <h2 className="mx-auto mt-4 text-3xl font-extrabold text-gray-900">
        <FormattedMessage id="register.title" />
      </h2>
      <Card className="mx-auto mt-8 w-full max-w-md p-8">
        {register.success ? (
          <>
            <div className="text-center">
              <FormattedMessage id="register.success" />
            </div>
            <div className="mt-4 text-center">
              <Link to={Paths.Home} text="register.continue" />
            </div>
          </>
        ) : (
          <>
            <Formik
              initialValues={InitialRegisterFormValues}
              onSubmit={(values, formikHelpers) =>
                register.register(values, formikHelpers)
              }
              validationSchema={() => validationSchema}
              initialErrors={undefined}
              enableReinitialize>
              {({ values, status, errors }) => (
                <Form className="space-y-4">
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
                    label="generic.password"
                    className="pl-8"
                    type="password"
                    value={values.password}
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
                      disabled={register.loading}
                      fullWidth
                      size="lg"
                      loading={register.loading}>
                      <FormattedMessage id="register.button" />
                    </Button>
                  </div>
                </Form>
              )}
            </Formik>
            <div className="mt-4 text-center text-sm font-medium">
              <span className="text-gray-600">
                <FormattedMessage id="register.existing.question" />
              </span>
              <Link
                to={Paths.Home}
                text="register.existing.action"
                className="ml-1"
              />
            </div>
            <ExternalLogin login={externalLogin} />
          </>
        )}
      </Card>
    </>
  );
}
