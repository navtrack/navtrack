import { Form, Formik, FormikHelpers } from "formik";
import { useCallback } from "react";
import { FormikTextInput } from "../ui/form/text-input/FormikTextInput";
import { FormattedMessage } from "react-intl";
import { FormikSelect } from "../ui/form/select/FormikSelect";
import { useNotification } from "../ui/notification/useNotification";
import { useUpdateUserMutation } from "@navtrack/shared/hooks/queries/users/useUpdateUserMutation";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { nameOf } from "@navtrack/shared/utils/typescript";
import { SelectOption } from "../ui/form/select/Select";
import { UnitsType } from "@navtrack/shared/api/model";
import { Button } from "../ui/button/Button";
import { Card } from "../ui/card/Card";
import { CardHeader } from "../ui/card/CardHeader";
import { Heading } from "../ui/heading/Heading";
import { CardBody } from "../ui/card/CardBody";
import { CardFooter } from "../ui/card/CardFooter";
import { useCurrentUserQuery } from "@navtrack/shared/hooks/queries/user/useCurrentUserQuery";

type AccountSettingsFormValues = {
  email?: string;
  units?: UnitsType;
};

const units: SelectOption[] = [
  {
    label: "generic.units.metric",
    value: `${UnitsType.Metric}`
  },
  {
    label: "generic.units.imperial",
    value: `${UnitsType.Imperial}`
  }
];

export function SettingsAccountPage() {
  const user = useCurrentUserQuery();
  const updateUserMutation = useUpdateUserMutation();
  const { showNotification } = useNotification();

  const handleSubmit = useCallback(
    (
      values: AccountSettingsFormValues,
      formikHelpers: FormikHelpers<AccountSettingsFormValues>
    ) => {
      updateUserMutation.mutate(
        {
          data: {
            email: values.email,
            unitsType: values.units
          }
        },
        {
          onSuccess: () => {
            showNotification({
              type: "success",
              description: "settings.account.success"
            });
          },
          onError: (error) => mapErrors(error, formikHelpers)
        }
      );
    },
    [showNotification, updateUserMutation]
  );

  return (
    <>
      <Card>
        <CardHeader>
          <Heading type="h2">
            <FormattedMessage id="settings.account.title" />
          </Heading>
        </CardHeader>
        <Formik<AccountSettingsFormValues>
          initialValues={{
            email: user.data?.email,
            units: user.data?.units
          }}
          enableReinitialize
          onSubmit={(values, formikHelpers) =>
            handleSubmit(values, formikHelpers)
          }>
          {() => (
            <Form>
              <CardBody>
                <div className="grid grid-cols-6 gap-6">
                  <div className="col-span-3">
                    <FormikTextInput
                      name={nameOf<AccountSettingsFormValues>("email")}
                      label="generic.email"
                      loading={user.isLoading}
                    />
                  </div>
                  <div className="col-span-3"></div>
                  <div className="col-span-3">
                    <FormikSelect
                      name={nameOf<AccountSettingsFormValues>("units")}
                      label="generic.units"
                      options={units}
                      loading={user.isLoading}
                    />
                  </div>
                </div>
              </CardBody>
              <CardFooter className="text-right">
                <Button
                  size="lg"
                  type="submit"
                  disabled={!user}
                  isLoading={updateUserMutation.isLoading}>
                  <FormattedMessage id="generic.save" />
                </Button>
              </CardFooter>
            </Form>
          )}
        </Formik>
      </Card>
    </>
  );
}
