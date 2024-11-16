import { nameOf } from "@navtrack/shared/utils/typescript";
import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { Card } from "../ui/card/Card";
import { FormikTextInput } from "../ui/form/text-input/FormikTextInput";
import {
  ChangePasswordFormValues,
  useChangePassword
} from "./useChangePassword";
import { Button } from "../ui/button/Button";
import { CardHeader } from "../ui/card/CardHeader";
import { Heading } from "../ui/heading/Heading";
import { CardBody } from "../ui/card/CardBody";
import { CardFooter } from "../ui/card/CardFooter";
import { useCurrentUserQuery } from "@navtrack/shared/hooks/queries/user/useCurrentUserQuery";

export function ChangePasswordCard() {
  const currentUser = useCurrentUserQuery();
  const changePassword = useChangePassword();

  return (
    <Formik<ChangePasswordFormValues>
      initialValues={{
        currentPassword: "",
        password: "",
        confirmPassword: ""
      }}
      validationSchema={changePassword.validationSchema}
      onSubmit={changePassword.handleSubmit}>
      {() => (
        <Form>
          <Card>
            <CardHeader>
              <Heading type="h2">
                <FormattedMessage id="settings.password.title" />
              </Heading>
            </CardHeader>
            <CardBody>
              <div className="grid grid-cols-6 gap-6">
                <div className="col-span-3">
                  <FormikTextInput
                    autoComplete="current-password"
                    type="password"
                    label="settings.password.current-password"
                    name={nameOf<ChangePasswordFormValues>("currentPassword")}
                    disabled={
                      currentUser.isSuccess &&
                      !currentUser.data?.authentication?.password
                    }
                    loading={currentUser.isLoading}
                  />
                </div>
                <div className="col-span-3 col-start-1">
                  <FormikTextInput
                    autoComplete="new-password"
                    type="password"
                    label="generic.password"
                    name={nameOf<ChangePasswordFormValues>("password")}
                  />
                </div>
                <div className="col-span-3 col-start-1">
                  <FormikTextInput
                    autoComplete="new-password"
                    type="password"
                    label="generic.confirm-password"
                    name={nameOf<ChangePasswordFormValues>("confirmPassword")}
                  />
                </div>
              </div>
            </CardBody>
            <CardFooter className="text-right">
              <Button
                type="submit"
                size="lg"
                isLoading={changePassword.isLoading}>
                <FormattedMessage id="generic.save" />
              </Button>
            </CardFooter>
          </Card>
        </Form>
      )}
    </Formik>
  );
}
