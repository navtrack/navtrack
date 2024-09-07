import { useIntl } from "react-intl";
import { useCallback } from "react";
import { format, parseISO } from "date-fns";

export function useDateTime() {
  const intl = useIntl();

  const showDate = useCallback(
    (
      date?: string | Date | null,
      customFormat?: string,
      customMessage?: string
    ): string => {
      if (date !== undefined && date !== null) {
        return format(
          typeof date === "string" ? parseISO(date) : date,
          customFormat ?? "yyyy-MM-dd"
        );
      }

      return customMessage ?? intl.formatMessage({ id: "generic.na" });
    },
    [intl]
  );

  const showTime = useCallback(
    (date?: string): string =>
      date
        ? format(parseISO(date), "HH:mm:ss")
        : intl.formatMessage({ id: "generic.na" }),
    [intl]
  );

  const showDateTime = useCallback(
    (date?: string): string =>
      date
        ? `${showDate(date)} ${showTime(date)}`
        : intl.formatMessage({ id: "generic.na" }),
    [intl, showDate, showTime]
  );

  function getDate(dateTime: string) {
    return parseISO(dateTime);
  }

  return { showDate, showTime, showDateTime, getDate };
}
