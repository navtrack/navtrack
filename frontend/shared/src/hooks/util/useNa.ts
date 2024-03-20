import { useIntl } from "react-intl";
import { useCallback } from "react";

export function useNa() {
  const intl = useIntl();

  const na = useCallback(
    (value?: string) => {
      if (!value) {
        return intl.formatMessage({ id: "generic.na" });
      }
      return value;
    },
    [intl]
  );

  return na;
}
