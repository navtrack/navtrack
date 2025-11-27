import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { FormattedMessage } from "react-intl";
import { Card } from "../card/Card";
import { Icon } from "../icon/Icon";
import { faArrowDown, faArrowUp } from "@fortawesome/free-solid-svg-icons";
import { TableProps, useTable } from "./useTable";
import { LoadingIndicator } from "@navtrack/shared/components/components/ui/loading-indicator/LoadingIndicator";

export function TableV2<T>(props: TableProps<T>) {
  const table = useTable(props);

  return (
    <Card className="overflow-hidden">
      <div
        className={classNames("overflow-y-auto", props.className)}
        style={{ height: props.height }}>
        <table className="w-full border-separate border-spacing-0 h-full">
          <thead>
            <tr>
              {props.columns.map((column, index) => (
                <th
                  onClick={() =>
                    !!column.value ? table.handleHeaderClick(index) : null
                  }
                  key={`${column.labelId}${index}`}
                  className={classNames(
                    "sticky top-0 border-b border-gray-900/5 bg-gray-50 px-3 py-2 text-left text-xs font-medium uppercase tracking-wider text-gray-500",
                    c(!!column.value, "cursor-pointer"),
                    column.headerClassName
                  )}
                  style={{ zIndex: 1 }}>
                  <div className="flex items-center h-full">
                    {column.header !== undefined ? (
                      column.header(index)
                    ) : (
                      <>
                        {column.labelId !== undefined && (
                          <FormattedMessage id={column.labelId} />
                        )}
                        {!!column.value && (
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
                      </>
                    )}
                  </div>
                </th>
              ))}
            </tr>
          </thead>
          <tbody className="text-sm text-gray-900">
            {table.sortedRows === undefined ||
            (props.isLoading && table.sortedRows.length === 0) ? (
              <tr>
                <td className="p-3 text-center" colSpan={props.columns.length}>
                  <LoadingIndicator className="text-xl" />
                </td>
              </tr>
            ) : table.sortedRows.length === 0 ? (
              <tr>
                <td className="p-3 text-center" colSpan={props.columns.length}>
                  <FormattedMessage id="ui.table.no-items" />
                </td>
              </tr>
            ) : (
              table.sortedRows.map((row, rowIndex) => (
                <tr
                  key={`row${rowIndex}`}
                  onClick={() => {
                    if (table.selectionEnabled) {
                      table.setIndex(rowIndex);
                    }
                    props.rowClick?.(row);
                  }}
                  ref={(el) => {
                    table.tableRows.current[rowIndex] = el;
                  }}
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
                    ),
                    c(
                      props.rowClick !== undefined,
                      "hover:cursor-pointer hover:bg-gray-100"
                    )
                  )}>
                  {props.columns.map((column, columnIndex) => (
                    <td
                      key={`row${rowIndex}col${columnIndex}`}
                      className={classNames(
                        "px-3 py-2",
                        c(
                          rowIndex + 1 !== props.rows?.length,
                          "border-b border-gray-900/5"
                        ),
                        column.rowClassName
                      )}>
                      {column.row(row, rowIndex)}
                    </td>
                  ))}
                </tr>
              ))
            )}
          </tbody>
          {table.hasFooter && (
            <tfoot>
              <tr>
                {props.columns.map((column, index) => (
                  <td
                    colSpan={column.footerColSpan}
                    key={`footer${index}`}
                    className={classNames(
                      "font-bold sticky bottom-0 h-7 border-t border-gray-900/5 bg-gray-50 px-2 py-1 text-left text-xs text-gray-900",
                      column.footerClassName
                    )}>
                    {index === 0 &&
                    column.footer === undefined &&
                    props.isLoading &&
                    (table.sortedRows ?? []).length > 0 ? (
                      <div className="flex">
                        <LoadingIndicator />
                      </div>
                    ) : null}
                    {column.footer?.(table.getColumnTotal(column))}
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
