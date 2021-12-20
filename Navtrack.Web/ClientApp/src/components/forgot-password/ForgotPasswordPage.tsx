import { Formik, Form } from "formik";
import { FormattedMessage } from "react-intl";
import { Link } from "react-router-dom";
import Button from "../ui/shared/button/Button";
import Card from "../ui/shared/card/Card";
import { InitialForgotPasswordFormValues } from "./ForgotPasswordFormValues";
import { useForgotPassword } from "./useForgotPassword";
import { useForgotPasswordFormValidationSchema } from "./useForgotPasswordFormValidationSchema";
import FormikTextInput from "../ui/shared/text-input/FormikTextInput";
import TextInputLeftAddon from "../ui/shared/text-input/TextInputLeftAddon";
import Icon from "../ui/shared/icon/Icon";
import { faEnvelope } from "@fortawesome/free-regular-svg-icons";

export default function ForgotPasswordPage() {
  const validationSchema = useForgotPasswordFormValidationSchema();
  const forgotPassword = useForgotPassword();

  return (
    <>
      <h2 className="mt-4 mx-auto text-3xl font-extrabold text-gray-900">
        <FormattedMessage id="forgot-password.title" />
      </h2>
      <Card className="max-w-md mx-auto p-8 mt-8 w-full">
        {forgotPassword.success ? (
          <>
            <div className="text-center">
              <FormattedMessage id="register.success" />
            </div>
            <div className="text-center mt-4">
              <Link to="/login" className="ml-1 text-blue-600">
                <FormattedMessage id="register.continue" />
              </Link>
            </div>
          </>
        ) : (
          <>
            <Formik
              initialValues={InitialForgotPasswordFormValues}
              onSubmit={(values) => forgotPassword.register(values)}
              validationSchema={() => validationSchema}
              enableReinitialize>
              {({ values }) => (
                <Form className="space-y-4">
                  <FormikTextInput
                    name="email"
                    label={"generic.email"}
                    value={values.email}
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
              )}
            </Formik>
            <div className="text-center text-sm font-medium ium mt-4">
              <span className="text-gray-600">
                <FormattedMessage id="forgot-password.existing.question" />
              </span>
              <Link to="/login" className="ml-1 text-blue-600">
                <FormattedMessage id="forgot-password.existing.action" />
              </Link>
            </div>
          </>
        )}
      </Card>
    </>
  );
}
