import { c } from "@navtrack/ui-shared/utils/tailwind";
import classNames from "classnames";
import { ReactNode } from "react";
import { FormattedMessage } from "react-intl";
import LoadingIndicator from "../loading-indicator/LoadingIndicator";

export interface ITableColumn<T> {
  labelId?: string;
  render: (row: T) => ReactNode;
}

interface ITable<T> {
  columns: ITableColumn<T>[];
  rows?: T[];
  loading?: boolean;
}

export default function Table<T>(props: ITable<T>) {
  return (
    <table className="w-full border">
      <thead className="border bg-gray-50 text-xs font-medium uppercase tracking-wider text-gray-500 ">
        <tr>
          {props.columns.map((column, index) => (
            <td key={`header${index}`} className="p-2">
              {column.labelId !== undefined && (
                <FormattedMessage id={column.labelId} />
              )}
            </td>
          ))}
        </tr>
      </thead>
      <tbody className="border-b text-sm text-gray-900">
        {props.loading ? (
          <tr className="border">
            <td className="p-2 text-center" colSpan={props.columns.length}>
              <LoadingIndicator className="text-base" />
            </td>
          </tr>
        ) : props.rows?.length === 0 ? (
          <tr className="border">
            <td className="p-2 text-center" colSpan={props.columns.length}>
              <FormattedMessage id="ui.table.no-items" />
            </td>
          </tr>
        ) : (
          props.rows?.map((row, rowIndex) => (
            <tr key={`row${rowIndex}`} className="border">
              {props.columns.map((column, columnIndex) => (
                <td
                  key={`row${rowIndex}col${columnIndex}`}
                  className={classNames(
                    "p-2",
                    c(rowIndex % 2 !== 0, "bg-gray-50")
                  )}>
                  {column.render(row)}
                </td>
              ))}
            </tr>
          ))
        )}
      </tbody>
    </table>
  );
}
