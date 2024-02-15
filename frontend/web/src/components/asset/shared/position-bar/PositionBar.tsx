import { useDateTime } from "@navtrack/shared/hooks/util/useDateTime";
import { useDistance } from "@navtrack/shared/hooks/util/useDistance";
import { FormattedMessage } from "react-intl";
import { PositionModel } from "@navtrack/shared/api/model/generated";
import {
  showCoordinate,
  showHeading,
  showProperty
} from "@navtrack/shared/utils/coordinates";

type PositionBarProps = {
  position: PositionModel;
};

export function PositionBar(props: PositionBarProps) {
  const { showDateTime } = useDateTime();
  const { showSpeed, showAltitude } = useDistance();

  return (
    <table className="w-full">
      <thead>
        <tr className="text-left text-xs uppercase tracking-wider text-gray-500">
          <th className="font-medium">
            <FormattedMessage id="generic.date" />
          </th>
          <th className="font-medium">
            <FormattedMessage id="generic.latitude" />
          </th>
          <th className="font-medium">
            <FormattedMessage id="generic.longitude" />
          </th>
          <th className="font-medium">
            <FormattedMessage id="generic.speed" />
          </th>
          <th className="font-medium">
            <FormattedMessage id="generic.altitude" />
          </th>
          <th className="font-medium">
            <FormattedMessage id="generic.heading" />
          </th>
          <th className="font-medium">
            <FormattedMessage id="generic.satellites" />
          </th>
          <th className="font-medium">
            <FormattedMessage id="generic.hdop" />
          </th>
        </tr>
      </thead>
      <tbody>
        <tr className="text-sm font-medium">
          <td className="min-w-40">{showDateTime(props.position.dateTime)}</td>
          <td className="min-w-24">
            {showCoordinate(props.position.latitude)}
          </td>
          <td className="min-w-24">
            {showCoordinate(props.position.longitude)}
          </td>
          <td className="min-w-24">{showSpeed(props.position.speed)}</td>
          <td className="min-w-24">{showAltitude(props.position.altitude)}</td>
          <td className="min-w-20">{showHeading(props.position.heading)}</td>
          <td className="min-w-24">
            {showProperty(props.position.satellites)}
          </td>
          <td className="min-w-14">{showProperty(props.position.hdop)}</td>
        </tr>
      </tbody>
    </table>
  );
}
