import { useDateTime } from "@navtrack/shared/hooks/util/useDateTime";
import { useDistance } from "@navtrack/shared/hooks/util/useDistance";
import { FormattedMessage } from "react-intl";
import { PositionModel } from "@navtrack/shared/api/model/generated";

type PositionBarProps = {
  position: PositionModel;
};

export function PositionBar(props: PositionBarProps) {
  const { showDateTime } = useDateTime();
  const { showSpeed, showAltitude } = useDistance();

  return (
    <table className="w-full">
      <tr className="text-xs">
        <td>
          <FormattedMessage id="generic.date" />
        </td>
        <td>
          <FormattedMessage id="generic.latitude" />
        </td>
        <td>
          <FormattedMessage id="generic.longitude" />
        </td>
        <td>
          <FormattedMessage id="generic.speed" />
        </td>
        <td>
          <FormattedMessage id="generic.altitude" />
        </td>
        <td>
          <FormattedMessage id="generic.heading" />
        </td>
        <td>
          <FormattedMessage id="generic.satellites" />
        </td>
        <td>
          <FormattedMessage id="generic.hdop" />
        </td>
      </tr>
      <tr className="text-sm font-semibold">
        <td>{showDateTime(props.position.dateTime)}</td>
        <td>{props.position.latitude}</td>
        <td>{props.position.longitude}</td>
        <td>{showSpeed(props.position.speed)}</td>
        <td>{showAltitude(props.position.altitude)}</td>
        <td>{props.position.heading ? `${props.position.heading}°` : "-"}</td>
        <td>{props.position.satellites ?? "-"}</td>
        <td>{props.position.hdop ?? "-"}</td>
      </tr>
    </table>
  );
}
