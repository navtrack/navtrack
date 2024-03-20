import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { FormattedMessage } from "react-intl";
import { LoadingIndicator } from "../loading-indicator/LoadingIndicator";
import { Card } from "../card/Card";
import { Icon } from "../icon/Icon";
import { faArrowDown, faArrowUp } from "@fortawesome/free-solid-svg-icons";
import { TableProps, useTable } from "./useTable";

export function TableV2<T>(props: TableProps<T>) {
  const table = useTable(props);

  return (
    <Card className="overflow-hidden">
      <div className={classNames("overflow-y-auto", props.className)}>
        <table className="w-full border-separate border-spacing-0">
          <thead>
            <tr>
              {props.columns.map((column, index) => (
                <th
                  onClick={() =>
                    column.sortable ? table.handleHeaderClick(index) : null
                  }
                  key={column.labelId}
                  className="sticky top-0 cursor-pointer border-b border-gray-900/5 bg-gray-50 p-2 text-left text-xs font-medium uppercase tracking-wider text-gray-500">
                  <div className="flex items-center">
                    {column.labelId !== undefined && (
                      <FormattedMessage id={column.labelId} />
                    )}
                    {column.sortable && (
                      <div className="w-6">
                        {table.sort.column === index && (
                          <Icon
                            className="ml-2"
                            icon={
                              table.sort.direction === "asc"
                                ? faArrowUp
                                : faArrowDown
                            }
                          />
                        )}
                      </div>
                    )}
                  </div>
                </th>
              ))}
            </tr>
          </thead>
          <tbody className="text-xs text-gray-900">
            {table.sortedRows === undefined ? (
              <tr>
                <td className="p-2 text-center" colSpan={props.columns.length}>
                  <LoadingIndicator className="text-xl" />
                </td>
              </tr>
            ) : table.sortedRows.length === 0 ? (
              <tr>
                <td className="p-2 text-center" colSpan={props.columns.length}>
                  <FormattedMessage id="ui.table.no-items" />
                </td>
              </tr>
            ) : (
              table.sortedRows.map((row, rowIndex) => (
                <tr
                  key={`row${rowIndex}`}
                  onClick={() =>
                    table.selectionEnabled
                      ? table.setIndex(rowIndex)
                      : undefined
                  }
                  ref={(el) => (table.tableRows.current[rowIndex] = el)}
                  className={classNames(
                    c(
                      table.selectionEnabled &&
                        rowIndex === table.selectedIndex,
                      "bg-gray-200",
                      c(rowIndex % 2 !== 0, "bg-gray-50")
                    ),
                    c(
                      table.selectionEnabled,
                      "cursor-pointer hover:bg-gray-100"
                    )
                  )}>
                  {props.columns.map((column, columnIndex) => (
                    <td
                      key={`row${rowIndex}col${columnIndex}`}
                      className={classNames(
                        "px-2 py-1",
                        c(
                          rowIndex + 1 !== props.rows?.length,
                          "border-b border-gray-900/5"
                        )
                      )}>
                      {column.row(row)}
                    </td>
                  ))}
                </tr>
              ))
            )}
          </tbody>
          {table.hasFooter && (
            <tfoot>
              <tr>
                {props.columns
                  .filter(
                    (x) =>
                      x.footer !== undefined || x.footerColSpan !== undefined
                  )
                  .map((column, index) => (
                    <td
                      colSpan={column.footerColSpan}
                      key={`footer${index}`}
                      className={classNames(
                        "sticky bottom-0 h-6 border-t border-gray-900/5 bg-gray-50 px-2 py-1 text-left text-xs text-gray-900",
                        column.footerClassName
                      )}>
                      {column.footer?.(props.rows)}
                    </td>
                  ))}
              </tr>
            </tfoot>
          )}
        </table>
      </div>
    </Card>
  );
}
