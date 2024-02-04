import { nameOf } from "@navtrack/shared/utils/typescript";
import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import { Card } from "../ui/card/Card";
import { FormikTextInput } from "../ui/form/text-input/FormikTextInput";
import {
  ChangePasswordFormValues,
  useChangePassword
} from "./useChangePassword";
import { NewButton } from "../ui/button/NewButton";
import { CardHeader } from "../ui/card/CardHeader";
import { Heading } from "../ui/heading/Heading";
import { CardBody } from "../ui/card/CardBody";
import { CardFooter } from "../ui/card/CardFooter";

export function SettingsPasswordPage() {
  const { validationSchema, handleSubmit } = useChangePassword();

  return (
    <>
      <Formik<ChangePasswordFormValues>
        initialValues={{
          currentPassword: "",
          password: "",
          confirmPassword: ""
        }}
        validationSchema={validationSchema}
        onSubmit={(values, formikHelpers) =>
          handleSubmit(values, formikHelpers)
        }>
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
                      type="password"
                      label="settings.password.current-password"
                      name={nameOf<ChangePasswordFormValues>("currentPassword")}
                    />
                  </div>
                  <div className="col-span-3 col-start-1">
                    <FormikTextInput
                      type="password"
                      label="generic.password"
                      name={nameOf<ChangePasswordFormValues>("password")}
                    />
                  </div>
                  <div className="col-span-3 col-start-1">
                    <FormikTextInput
                      type="password"
                      label="generic.confirm-password"
                      name={nameOf<ChangePasswordFormValues>("confirmPassword")}
                    />
                  </div>
                </div>
              </CardBody>
              <CardFooter className="text-right">
                <NewButton type="submit" size="lg">
                  <FormattedMessage id="generic.save" />
                </NewButton>
              </CardFooter>
            </Card>
          </Form>
        )}
      </Formik>
    </>
  );
}
