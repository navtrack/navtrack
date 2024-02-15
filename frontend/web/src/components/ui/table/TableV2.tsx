import { c, classNames } from "@navtrack/shared/utils/tailwind";
import {
  ReactNode,
  useCallback,
  useEffect,
  useMemo,
  useRef,
  useState
} from "react";
import { FormattedMessage } from "react-intl";
import { LoadingIndicator } from "../loading-indicator/LoadingIndicator";
import { Card } from "../card/Card";
import { Icon } from "../icon/Icon";
import { faArrowDown, faArrowUp } from "@fortawesome/free-solid-svg-icons";
import { useKeyPress } from "@navtrack/shared/hooks/util/useKeyPress";

export interface ITableColumn<T> {
  labelId?: string;
  rowClassName?: string;
  value?: (row: T) => string | number | null | undefined;
  render: (row: T) => ReactNode;
  footer?: (rows?: T[]) => ReactNode;
  sort?: "asc" | "desc";
  sortable?: boolean;
}

type TableProps<T> = {
  columns: ITableColumn<T>[];
  rows?: T[];
  className?: string;
  equals?: (a: T, b: T) => boolean;
  selectedItem?: T;
  setSelectedItem?: (item?: T) => void;
};

function getInitialSort<T>(columns: ITableColumn<T>[]): {
  column: number;
  direction: "asc" | "desc";
} {
  for (let index = 0; index < columns.length; index++) {
    const column = columns[index];
    if (column.sort !== undefined) {
      return { column: index, direction: column.sort };
    }
  }

  return { column: 0, direction: "asc" };
}

function getInitialSelectedIndex<T>(props: TableProps<T>): number | undefined {
  if (
    props.selectedItem !== undefined &&
    props.equals !== undefined &&
    props.rows !== undefined
  ) {
    return props.rows.findIndex((row) =>
      props.equals?.(row, props.selectedItem!)
    );
  }

  return 0;
}

export function TableV2<T>(props: TableProps<T>) {
  const selectionEnabled = props.setSelectedItem !== undefined;
  const hasFooter = props.columns.some((column) => column.footer !== undefined);
  const [selectedIndex, setSelectedIndex] = useState(
    getInitialSelectedIndex(props)
  );
  const tableRows = useRef<Array<HTMLDivElement | null>>([]);

  const [sort, setSort] = useState<{
    column: number;
    direction: "asc" | "desc";
  }>(getInitialSort(props.columns));

  const sortedRows = useMemo(() => {
    if (props.rows === undefined) {
      return undefined;
    }

    const sorted = [...(props.rows ?? [])];

    sorted.sort((a, b) => {
      const column = props.columns[sort.column];

      if (column.value !== undefined) {
        const aValue = column.value(a);
        const bValue = column.value(b);

        if (
          aValue !== null &&
          bValue !== null &&
          aValue !== undefined &&
          bValue !== undefined
        ) {
          if (aValue < bValue) {
            return sort.direction === "asc" ? -1 : 1;
          }
          if (aValue > bValue) {
            return sort.direction === "asc" ? 1 : -1;
          }
        }
      }

      return 0;
    });

    return sorted;
  }, [props.columns, props.rows, sort.column, sort.direction]);

  const scrollToElement = useCallback((index: number) => {
    tableRows.current[index]?.scrollIntoView({
      behavior: "smooth",
      block: "center"
    });
  }, []);

  const setIndex = useCallback(
    (index: number) => {
      scrollToElement(index);
      setSelectedIndex(index);
    },
    [scrollToElement]
  );

  const selectedItem = useMemo(() => {
    return selectedIndex !== undefined
      ? sortedRows?.[selectedIndex]
      : undefined;
  }, [selectedIndex, sortedRows]);

  useEffect(() => {
    if (
      selectionEnabled &&
      selectedItem !== undefined &&
      (props.selectedItem === undefined ||
        !props.equals?.(props.selectedItem, selectedItem))
    ) {
      props.setSelectedItem?.(selectedItem);
    }
  }, [props, selectedIndex, selectedItem, selectionEnabled]);

  const setPreviousIndex = useCallback(() => {
    if (selectedIndex !== undefined) {
      const newLocationIndex = selectedIndex - 1;
      if (newLocationIndex >= 0) {
        setIndex(newLocationIndex);
      }
    } else {
      setIndex(0);
    }
  }, [selectedIndex, setIndex]);

  const setNextIndex = useCallback(() => {
    if (selectedIndex !== undefined) {
      const newLocationIndex = selectedIndex + 1;
      if (props.rows && newLocationIndex < props.rows.length) {
        setIndex(newLocationIndex);
      }
    } else {
      setIndex(0);
    }
  }, [props.rows, selectedIndex, setIndex]);

  useKeyPress("ArrowDown", selectionEnabled ? setNextIndex : undefined);
  useKeyPress("ArrowUp", selectionEnabled ? setPreviousIndex : undefined);

  const handleHeaderClick = useCallback(
    (index: number) => {
      setSort({
        column: index,
        direction:
          sort.column === index && sort.direction === "asc" ? "desc" : "asc"
      });
      if (selectedIndex !== undefined) {
        setIndex(0);
      }
    },
    [selectedIndex, setIndex, sort.column, sort.direction]
  );

  return (
    <Card className="overflow-hidden">
      <div className={classNames("overflow-y-auto", props.className)}>
        <table className="w-full border-separate border-spacing-0">
          <thead>
            <tr>
              {props.columns.map((column, index) => (
                <th
                  onClick={() =>
                    column.sortable ? handleHeaderClick(index) : null
                  }
                  key={column.labelId}
                  className="sticky top-0 cursor-pointer border-b border-gray-900/5 bg-gray-50 p-2 text-left text-xs font-medium uppercase tracking-wider text-gray-500">
                  <div className="flex items-center">
                    {column.labelId !== undefined && (
                      <FormattedMessage id={column.labelId} />
                    )}
                    {column.sortable && (
                      <div className="w-6">
                        {sort.column === index && (
                          <Icon
                            className="ml-2"
                            icon={
                              sort.direction === "asc" ? faArrowUp : faArrowDown
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
            {sortedRows === undefined ? (
              <tr>
                <td className="p-2 text-center" colSpan={props.columns.length}>
                  <LoadingIndicator className="text-xl" />
                </td>
              </tr>
            ) : sortedRows.length === 0 ? (
              <tr>
                <td className="p-2 text-center" colSpan={props.columns.length}>
                  <FormattedMessage id="ui.table.no-items" />
                </td>
              </tr>
            ) : (
              sortedRows.map((row, rowIndex) => (
                <tr
                  key={`row${rowIndex}`}
                  onClick={() =>
                    selectionEnabled ? setIndex(rowIndex) : undefined
                  }
                  ref={(el) => (tableRows.current[rowIndex] = el)}
                  className={classNames(
                    c(
                      selectionEnabled && rowIndex === selectedIndex,
                      "bg-gray-200",
                      c(rowIndex % 2 !== 0, "bg-gray-50")
                    ),
                    c(selectionEnabled, "hover:bg-gray-200")
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
                      {column.render(row)}
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
                    key={`footer${index}`}
                    className="sticky bottom-0 border-t border-gray-900/5 bg-gray-50 px-2 py-1 text-left text-xs font-medium text-gray-900">
                    {column.footer !== undefined
                      ? column.footer(props.rows)
                      : null}
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
