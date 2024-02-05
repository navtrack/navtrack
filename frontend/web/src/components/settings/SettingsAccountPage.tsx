import { Form, Formik, FormikHelpers } from "formik";
import { useCallback, useMemo } from "react";
import { FormikTextInput } from "../ui/form/text-input/FormikTextInput";
import { FormattedMessage, useIntl } from "react-intl";
import { FormikSelect } from "../ui/form/select/FormikSelect";
import { useNotification } from "../ui/notification/useNotification";
import { useUpdateUserMutation } from "@navtrack/shared/hooks/mutations/users/useUpdateUserMutation";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { nameOf } from "@navtrack/shared/utils/typescript";
import { SelectOption } from "../ui/form/select/Select";
import { UnitsType } from "@navtrack/shared/api/model/generated";
import { Button } from "../ui/button/Button";
import { Card } from "../ui/card/Card";
import { CardHeader } from "../ui/card/CardHeader";
import { Heading } from "../ui/heading/Heading";
import { CardBody } from "../ui/card/CardBody";
import { CardFooter } from "../ui/card/CardFooter";
import { Skeleton } from "../ui/skeleton/Skeleton";
import { useCurrentUserQuery } from "@navtrack/shared/hooks/queries/useCurrentUserQuery";

type AccountSettingsFormValues = {
  email?: string;
  units?: UnitsType;
};

export function SettingsAccountPage() {
  const user = useCurrentUserQuery();
  const updateUserMutation = useUpdateUserMutation();
  const { showNotification } = useNotification();
  const intl = useIntl();

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
              description: intl.formatMessage({
                id: "settings.account.success"
              })
            });
          },
          onError: (error) => mapErrors(error, formikHelpers)
        }
      );
    },
    [intl, showNotification, updateUserMutation]
  );

  const units: SelectOption[] = useMemo(
    () => [
      {
        label: intl.formatMessage({ id: "generic.units.metric" }),
        value: `${UnitsType.Metric}`
      },
      {
        label: intl.formatMessage({ id: "generic.units.imperial" }),
        value: `${UnitsType.Imperial}`
      }
    ],
    [intl]
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
                <Skeleton loading={!user}>
                  <div className="grid grid-cols-6 gap-6">
                    <div className="col-span-3">
                      <FormikTextInput
                        name={nameOf<AccountSettingsFormValues>("email")}
                        label="generic.email"
                      />
                    </div>
                    <div className="col-span-3"></div>
                    <div className="col-span-3">
                      <FormikSelect
                        name={nameOf<AccountSettingsFormValues>("units")}
                        label="generic.units"
                        options={units}
                      />
                    </div>
                  </div>
                </Skeleton>
              </CardBody>
              <CardFooter className="text-right">
                <Button
                  size="lg"
                  type="submit"
                  disabled={!user}
                  loading={updateUserMutation.isLoading}>
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
