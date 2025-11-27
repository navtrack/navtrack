import { DayOfWeek } from "@navtrack/shared/api/model";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";
import { addHours, parse, set } from "date-fns";
import { useCallback } from "react";

const workHoursStart = set(new Date(), {
  hours: 8,
  minutes: 0,
  seconds: 0,
  milliseconds: 0
});
const workHoursEnd = set(new Date(), {
  hours: 18,
  minutes: 0,
  seconds: 0,
  milliseconds: 0
});

const dayOfWeekMapping: Record<number, DayOfWeek> = {
  0: DayOfWeek.Sunday,
  1: DayOfWeek.Monday,
  2: DayOfWeek.Tuesday,
  3: DayOfWeek.Wednesday,
  4: DayOfWeek.Thursday,
  5: DayOfWeek.Friday,
  6: DayOfWeek.Saturday
};

export function useOrganizationWorkSchedule() {
  const currentOrganization = useCurrentOrganization();

  const getStartTime = useCallback(
    (date: Date) => {
      const day = date.getDay();
      const startTime = currentOrganization.data?.workSchedule?.days?.find(
        (x) => x.dayOfWeek === dayOfWeekMapping[day]
      )?.startTime;

      const result = startTime ? parse(startTime, "HH:mm:ss", date) : undefined;

      return result;
    },
    [currentOrganization.data]
  );

  const getEndTime = useCallback(
    (date: Date) => {
      const day = date.getDay();
      const endTime = currentOrganization.data?.workSchedule?.days?.find(
        (x) => x.dayOfWeek === dayOfWeekMapping[day]
      )?.endTime;

      const result = endTime ? parse(endTime, "HH:mm:ss", date) : undefined;

      return result;
    },
    [currentOrganization.data]
  );

  return { getStartTime, getEndTime };
}
