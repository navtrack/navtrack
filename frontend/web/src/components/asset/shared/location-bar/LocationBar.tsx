import { LocationModel } from "@navtrack/shared/api/model/generated";
import { useDateTime } from "@navtrack/shared/hooks/util/useDateTime";
import { useDistance } from "@navtrack/shared/hooks/util/useDistance";
import { ReactNode } from "react";
import { useIntl } from "react-intl";
import { LocationInfo } from "./LocationInfo";

type LocationBarProps = {
  location?: LocationModel;
  children?: ReactNode;
};

export function LocationBar(props: LocationBarProps) {
  const intl = useIntl();
  const { showDateTime } = useDateTime();
  const { showSpeed, showAltitude } = useDistance();

  return (
    <>
      {props.location && (
        <div className="flex grid grid-cols-9 gap-x-4">
          <LocationInfo
            titleId={"generic.date"}
            hideMargin={true}
            className="col-span-2">
            {showDateTime(props.location.dateTime)}
          </LocationInfo>
          <LocationInfo titleId={"generic.latitude"}>
            {props.location.latitude}
          </LocationInfo>
          <LocationInfo titleId={"generic.longitude"}>
            {props.location.longitude}
          </LocationInfo>
          <LocationInfo titleId={"generic.speed"}>
            {showSpeed(props.location.speed)}
          </LocationInfo>
          <LocationInfo titleId={"generic.altitude"}>
            {showAltitude(props.location.altitude)}
          </LocationInfo>
          <LocationInfo titleId={"generic.heading"}>
            {props.location.heading
              ? `${props.location.heading}°`
              : intl.formatMessage({ id: "generic.na" })}
          </LocationInfo>
          <LocationInfo titleId={"generic.satellites"}>
            {props.location.satellites ??
              intl.formatMessage({ id: "generic.na" })}
          </LocationInfo>
          <LocationInfo titleId={"generic.hdop"}>
            {props.location.hdop ?? intl.formatMessage({ id: "generic.na" })}
          </LocationInfo>
        </div>
      )}
    </>
  );
}
