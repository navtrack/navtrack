import { Formik, Form } from "formik";
import { FormattedMessage } from "react-intl";
import { Card } from "../../ui/card/Card";
import { FormikTextInput } from "../../ui/form/text-input/FormikTextInput";
import { Link } from "../../ui/link/Link";
import { Paths } from "../../../app/Paths";
import { TextInputLeftAddon } from "../../ui/form/text-input/TextInputLeftAddon";
import { Icon } from "../../ui/icon/Icon";
import { faEnvelope } from "@fortawesome/free-regular-svg-icons";
import { faLock } from "@fortawesome/free-solid-svg-icons";
import { useRegister } from "@navtrack/shared/hooks/user/register/useRegister";
import { useRegisterFormValidationSchema } from "@navtrack/shared/hooks/user/register/useRegisterFormValidationSchema";
import { InitialRegisterFormValues } from "@navtrack/shared/hooks/user/register/RegisterFormValues";
import { UnauthenticatedLayout } from "../../ui/layouts/unauthenticated/UnauthenticatedLayout";
import { Button } from "../../ui/button/Button";
import { SlotContext } from "../../../app/SlotContext";
import { useContext } from "react";

export function RegisterPage() {
  const validationSchema = useRegisterFormValidationSchema();
  const register = useRegister();
  const slots = useContext(SlotContext);

  return (
    <UnauthenticatedLayout>
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
              <Link to={Paths.Home} label="register.continue" />
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
              {({ values }) => (
                <Form className="space-y-4">
                  <FormikTextInput
                    name="email"
                    label="generic.email"
                    value={values.email}
                    autoComplete="username"
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
                    autoComplete="new-password"
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
                    autoComplete="new-password"
                    value={values.confirmPassword}
                    leftAddon={
                      <TextInputLeftAddon>
                        <Icon icon={faLock} />
                      </TextInputLeftAddon>
                    }
                  />
                  {slots?.captcha}
                  <div className="pt-2">
                    <Button
                      type="submit"
                      color="secondary"
                      full
                      size="lg"
                      isLoading={register.loading}>
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
                label="register.existing.action"
                className="ml-1"
              />
            </div>
            {slots?.externalLogin}
          </>
        )}
      </Card>
    </UnauthenticatedLayout>
  );
}
