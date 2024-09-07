import { useMemo } from "react";
import { useIntl } from "react-intl";
import { UnitsType } from "../../api/model/generated";
import { useCurrentUserQuery } from "../queries/useCurrentUserQuery";

type Units = {
  unitsType: UnitsType;
  speed: string;
  volume: string;
  length: string;
  lengthK: string;
};

export function useCurrentUnits() {
  const currentUser = useCurrentUserQuery();
  const intl = useIntl();

  const units = useMemo((): Units => {
    if (currentUser.data?.units === UnitsType.Imperial) {
      return {
        unitsType: UnitsType.Imperial,
        speed: intl.formatMessage({ id: "generic.units.mph" }),
        length: intl.formatMessage({ id: "generic.units.ft" }),
        lengthK: intl.formatMessage({ id: "generic.units.miles" }),
        volume: intl.formatMessage({ id: "generic.units.gal" })
      };
    }

    return {
      unitsType: UnitsType.Metric,
      speed: intl.formatMessage({ id: "generic.units.kph" }),
      length: intl.formatMessage({ id: "generic.units.m" }),
      lengthK: intl.formatMessage({ id: "generic.units.km" }),
      volume: intl.formatMessage({ id: "generic.units.l" })
    };
  }, [currentUser.data?.units, intl]);

  return units;
}
