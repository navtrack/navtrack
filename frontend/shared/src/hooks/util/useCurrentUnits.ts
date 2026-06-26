import { useMemo } from "react";
import { useIntl } from "react-intl";
import { UnitsType } from "../../api/model";
import { useCurrentUserQuery } from "../queries/user/useCurrentUserQuery";

type Units = {
  unitsType: UnitsType;
  speed: string;
  volume: string;
  length: string;
  lengthK: string;
  fuelConsumption: string;
};

export function useCurrentUnits() {
  const currentUser = useCurrentUserQuery();
  const intl = useIntl();

  const units = useMemo((): Units => {
    if (currentUser.data?.units === UnitsType.Imperial) {
      return {
        unitsType: UnitsType.Imperial,
        speed: intl.formatMessage({ id: "units.mph" }),
        length: intl.formatMessage({ id: "units.ft" }),
        lengthK: intl.formatMessage({ id: "units.miles" }),
        volume: intl.formatMessage({ id: "units.gal" }),
        fuelConsumption: intl.formatMessage({ id: "units.mpg" })
      };
    }

    return {
      unitsType: UnitsType.Metric,
      speed: intl.formatMessage({ id: "units.kph" }),
      length: intl.formatMessage({ id: "units.m" }),
      lengthK: intl.formatMessage({ id: "units.km" }),
      volume: intl.formatMessage({ id: "units.l" }),
      fuelConsumption: intl.formatMessage({ id: "units.l100km" })
    };
  }, [currentUser.data?.units, intl]);

  return units;
}
