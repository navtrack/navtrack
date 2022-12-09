import moment from "moment";
import { useCallback } from "react";
import { useIntl } from "react-intl";

export const useDateTime = () => {
  const intl = useIntl();

  const showDate = useCallback(
    (date?: string): string =>
      date
        ? moment(date).format("YYYY-MM-DD")
        : intl.formatMessage({ id: "generic.na" }),
    [intl]
  );

  const showTime = useCallback(
    (date?: string): string =>
      date
        ? moment(date).format("HH:mm:ss")
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

  const showDuration = useCallback((minutes?: number) => {
    minutes = minutes ?? 0;

    return minutes > 60
      ? `${Math.floor(minutes / 60)}h ${Math.round(minutes % 60)}m`
      : `${Math.round(minutes)}m`;
  }, []);

  return { showDate, showTime, showDateTime, showDuration };
};
