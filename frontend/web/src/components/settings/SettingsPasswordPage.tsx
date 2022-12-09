import { nameOf } from "@navtrack/ui-shared/utils/typescript";
import { Form, Formik } from "formik";
import { FormattedMessage } from "react-intl";
import Button from "../ui/shared/button/Button";
import Card from "../ui/shared/card/Card";
import FormikTextInput from "../ui/shared/text-input/FormikTextInput";
import Text from "../ui/shared/text/Text";
import SettingsLayout from "./SettingsLayout";
import { ChangePasswordFormValues } from "./types";
import useChangePassword from "./useChangePassword";

export default function SettingsPasswordPage() {
  const { validationSchema, handleSubmit } = useChangePassword();

  return (
    <SettingsLayout>
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
              <div className="p-6">
                <Text type="h3">
                  <FormattedMessage id="settings.password.title" />
                </Text>
                <div className="mt-6 grid grid-cols-6 gap-6">
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
              </div>
              <div className="bg-gray-50 px-6 py-3 text-right">
                <Button type="submit" size="lg">
                  <FormattedMessage id="generic.save" />
                </Button>
              </div>
            </Card>
          </Form>
        )}
      </Formik>
    </SettingsLayout>
  );
}
