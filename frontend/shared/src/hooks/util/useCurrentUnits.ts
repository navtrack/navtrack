import { useMemo } from "react";
import { useIntl } from "react-intl";
import { UnitsType } from "../../api/model/custom/UnitsType";
import { useCurrentUser } from "../app/useCurrentUser";

type Units = {
  unitsType: UnitsType;
  speed: string;
  length: string;
  lengthK: string;
};

export const useCurrentUnits = () => {
  const currentUser = useCurrentUser();
  const intl = useIntl();

  const units = useMemo((): Units => {
    if (currentUser?.units === UnitsType.Imperial) {
      return {
        unitsType: UnitsType.Imperial,
        speed: intl.formatMessage({ id: "generic.units.mph" }),
        length: intl.formatMessage({ id: "generic.units.ft" }),
        lengthK: intl.formatMessage({ id: "generic.units.miles" })
      };
    }

    return {
      unitsType: UnitsType.Metric,
      speed: intl.formatMessage({ id: "generic.units.kph" }),
      length: intl.formatMessage({ id: "generic.units.m" }),
      lengthK: intl.formatMessage({ id: "generic.units.km" })
    };
  }, [currentUser?.units, intl]);

  return units;
};
