import { Form, Formik, FormikHelpers } from "formik";
import { FormattedMessage } from "react-intl";
import { Card } from "../../../ui/card/Card";
import { CardBody } from "../../../ui/card/CardBody";
import { Heading } from "../../../ui/heading/Heading";
import { Button } from "../../../ui/button/Button";
import { useUpdateOrganizationMutation } from "@navtrack/shared/hooks/queries/organizations/useUpdateOrganizationMutation";
import { useCallback, useMemo, useState } from "react";
import { mapErrors } from "@navtrack/shared/utils/formik";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";
import { TimePicker } from "../../../ui/form/time-picker/TimePicker";
import { CardFooter } from "../../../ui/card/CardFooter";
import { DayOfWeek, WorkScheduleDayModel } from "@navtrack/shared/api/model";
import { format } from "date-fns";
import { useNotification } from "../../../ui/notification/useNotification";

export type RenameOrganizationFormValues = {
  name: string;
};

const days = [
  { dayOfWeek: DayOfWeek.Monday, labelId: "generic.days.monday" },
  { dayOfWeek: DayOfWeek.Tuesday, labelId: "generic.days.tuesday" },
  { dayOfWeek: DayOfWeek.Wednesday, labelId: "generic.days.wednesday" },
  { dayOfWeek: DayOfWeek.Thursday, labelId: "generic.days.thursday" },
  { dayOfWeek: DayOfWeek.Friday, labelId: "generic.days.friday" },
  { dayOfWeek: DayOfWeek.Saturday, labelId: "generic.days.saturday" },
  { dayOfWeek: DayOfWeek.Sunday, labelId: "generic.days.sunday" }
];

type WorkScheduleFormValues = {
  items: WorkScheduleItem[];
};

type WorkScheduleItem = {
  labelId: string;
  day: WorkScheduleDayModel;
};

export function OrganizationSettingsWorkSchedulePage() {
  const currentOrganization = useCurrentOrganization();
  const updateOrganization = useUpdateOrganizationMutation();
  const [showSuccess, setShowSuccess] = useState(false);
  const notification = useNotification();

  const [startTime, setStartTime] = useState<string | undefined>();
  const [endTime, setEndTime] = useState<string | undefined>();

  const submit = useCallback(
    (
      values: WorkScheduleDayModel[],
      formikHelpers: FormikHelpers<WorkScheduleFormValues>
    ) => {
      setShowSuccess(false);
      if (currentOrganization.data) {
        updateOrganization.mutate(
          {
            organizationId: currentOrganization.data.id,
            data: { workSchedules: values }
          },
          {
            onSuccess: () => {
              notification.showNotification({
                type: "success",
                description: "work-schedule.update-success"
              });
            },
            onError: (error) => {
              mapErrors(error, formikHelpers);
            }
          }
        );
      }
    },
    [currentOrganization.data, updateOrganization]
  );

  const initialValues: WorkScheduleFormValues = useMemo(() => {
    const values: WorkScheduleFormValues = {
      items: days.map((x) => {
        const existingDay = currentOrganization.data?.workSchedule?.days?.find(
          (d) => d.dayOfWeek === x.dayOfWeek
        ) ?? { dayOfWeek: x.dayOfWeek };

        return {
          labelId: x.labelId,
          day: existingDay
        };
      })
    };

    return values;
  }, [currentOrganization.data?.workSchedule?.days]);

  return (
    <Formik<WorkScheduleFormValues>
      initialValues={initialValues}
      onSubmit={(values, formikHelpers) =>
        submit(
          values.items.map((x) => x.day),
          formikHelpers
        )
      }
      enableReinitialize>
      {({ setValues, values }) => (
        <Form>
          <Card>
            <CardBody>
              <Heading type="h2">
                <FormattedMessage id="generic.work-schedule" />
              </Heading>
              <div className="mt-4 grid grid-cols-12 gap-x-6 gap-y-3 font-medium text-gray-900 text-sm">
                <div className="col-span-2 col-start-3">
                  <FormattedMessage id="generic.start-time" />
                </div>
                <div className="col-span-2">
                  <FormattedMessage id="generic.end-time" />
                </div>
                {days.map((day) => {
                  const value = values.items.find(
                    (x) => x.day.dayOfWeek === day.dayOfWeek
                  );

                  return (
                    <div
                      className="col-start-1 grid grid-cols-12 gap-x-6 gap-y-3 col-span-12"
                      key={day.dayOfWeek}>
                      <div className="col-span-2 col-start-1 flex items-center">
                        <FormattedMessage id={day.labelId} />
                      </div>
                      <div className="col-span-2">
                        <TimePicker
                          value={value?.day.startTime}
                          disabled={currentOrganization.isLoading}
                          onChange={(event) => {
                            setValues({
                              items: values.items.map((item) => ({
                                ...item,
                                day:
                                  item.day.dayOfWeek === day.dayOfWeek
                                    ? {
                                        ...item.day,
                                        startTime: event.target.value
                                      }
                                    : item.day
                              }))
                            });
                          }}
                        />
                      </div>
                      <div className="col-span-2">
                        <TimePicker
                          value={value?.day.endTime}
                          disabled={currentOrganization.isLoading}
                          onChange={(event) => {
                            setValues({
                              items: values.items.map((item) => ({
                                ...item,
                                day:
                                  item.day.dayOfWeek === day.dayOfWeek
                                    ? {
                                        ...item.day,
                                        endTime: event.target.value
                                      }
                                    : item.day
                              }))
                            });
                          }}
                        />
                      </div>
                    </div>
                  );
                })}
                <div className="col-start1 col-span-6 border-t border-gray-900/10"></div>
                <div className="col-start-1 grid grid-cols-12 gap-x-6 gap-y-3 col-span-12">
                  <div className="col-span-2 col-start-1 flex items-center">
                    <FormattedMessage id="generic.set-all" />
                  </div>
                  <div className="col-span-2">
                    <TimePicker
                      value={startTime}
                      disabled={currentOrganization.isLoading}
                      onChange={(event) => {
                        setStartTime(event.target.value);
                        setValues({
                          items: values.items.map((item) => ({
                            ...item,
                            day: {
                              ...item.day,
                              startTime: event.target.value
                            }
                          }))
                        });
                      }}
                    />
                  </div>
                  <div className="col-span-2">
                    <TimePicker
                      disabled={currentOrganization.isLoading}
                      value={endTime}
                      onChange={(event) => {
                        setEndTime(event.target.value);
                        setValues({
                          items: values.items.map((item) => ({
                            ...item,
                            day: {
                              ...item.day,
                              endTime: event.target.value
                            }
                          }))
                        });
                      }}
                    />
                  </div>
                </div>
              </div>
            </CardBody>
            <CardFooter className="text-right">
              <Button
                disabled={currentOrganization.isLoading}
                type="submit"
                size="lg"
                isLoading={updateOrganization.isPending}>
                <FormattedMessage id="generic.save" />
              </Button>
            </CardFooter>
          </Card>
        </Form>
      )}
    </Formik>
  );
}
