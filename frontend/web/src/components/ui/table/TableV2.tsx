import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { FormattedMessage } from "react-intl";
import { Card } from "../card/Card";
import { Icon } from "../icon/Icon";
import { faArrowDown, faArrowUp } from "@fortawesome/free-solid-svg-icons";
import { TableProps } from "./useTable";
import { LoadingIndicator } from "@navtrack/shared/components/components/ui/loading-indicator/LoadingIndicator";

export function TableV2<T>(props: TableProps<T>) {
  const hasFooter = props.columns?.some(
    (column) => column.footer !== undefined
  );

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
                    !!column.value ? props.headerClickHandler!(index) : null
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
                            {props.sort?.column === index && (
                              <Icon
                                className="ml-2"
                                icon={
                                  props.sort?.direction === "asc"
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
            {props.rows === undefined ? (
              <tr>
                <td className="p-3 text-center" colSpan={props.columns.length}>
                  <LoadingIndicator className="text-xl" />
                </td>
              </tr>
            ) : props.rows.length === 0 ? (
              <tr>
                <td className="p-3 text-center" colSpan={props.columns.length}>
                  <FormattedMessage id="ui.table.no-items" />
                </td>
              </tr>
            ) : (
              props.rows.map((row, rowIndex) => (
                <tr
                  key={`row${rowIndex}`}
                  onClick={() => {
                    if (props.selection) {
                      props.setSelectedIndex?.(rowIndex);
                    }
                    props.rowClickHandler?.(row);
                  }}
                  ref={(el) => {
                    if (props.tableRows) {
                      props.tableRows.current[rowIndex] = el;
                    }
                  }}
                  className={classNames(
                    c(
                      props.selection && rowIndex === props.selectedIndex,
                      "bg-gray-200",
                      c(rowIndex % 2 !== 0, "bg-gray-50")
                    ),
                    c(props.selection, "cursor-pointer hover:bg-gray-200"),
                    c(
                      props.rowClickHandler !== undefined,
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
          {hasFooter && (
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
                    props.rows === undefined ? (
                      <div className="flex">
                        <LoadingIndicator />
                      </div>
                    ) : null}
                    {column.footer?.(props.getColumnTotal?.(column))}
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
