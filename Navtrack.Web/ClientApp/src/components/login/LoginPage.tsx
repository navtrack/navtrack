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
import { useLogin } from "../../hooks/authentication/useLogin";
import FormikTextInput from "../ui/shared/text-input/FormikTextInput";
import TextInputLeftAddon from "../ui/shared/text-input/TextInputLeftAddon";
import { faEnvelope } from "@fortawesome/free-regular-svg-icons";
import LoadingIndicator from "../ui/shared/loading-indicator/LoadingIndicator";
import Paths from "../../app/Paths";

export default function LoginPage() {
  const validationSchema = useLoginFormValidationSchema();
  const { login, loading } = useLogin();

  return (
    <>
      <h2 className="mt-4 mx-auto text-3xl font-extrabold text-gray-900">
        <FormattedMessage id="login.title" />
      </h2>
      <Card className="max-w-md mx-auto p-8 mt-8 w-full">
        <Formik
          initialValues={InitialLoginFormValues}
          onSubmit={(values) => login(values)}
          validationSchema={validationSchema}>
          {({ values }) => (
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
        {/* <div className="text-center text-sm mt-4 font-medium">
          <Link to={Paths.ForgotPassword} text="login.forgot" />
        </div> */}
        <div className="text-center text-sm font-medium mt-4 border-t pt-4">
          <span className="text-gray-600">
            <FormattedMessage id="login.new.question" />
          </span>
          <Link to={Paths.Register} className="ml-1" text="login.new.action" />
        </div>
      </Card>
      <div className="text-center text-xs mt-4">
        <Copyright />
      </div>
    </>
  );
}
