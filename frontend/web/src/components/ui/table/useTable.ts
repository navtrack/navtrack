import { ReactNode, useCallback, useMemo, useRef, useState } from "react";
import { useKeyPress } from "@navtrack/shared/hooks/util/useKeyPress";

export interface ITableColumn<T> {
  labelId?: string;
  header?: (index: number) => ReactNode;
  row: (row: T, index: number) => ReactNode;
  headerClassName?: string;
  rowClassName?: string;
  footer?: (total?: number) => ReactNode;
  footerClassName?: string;
  footerColSpan?: number;
  sort?: "asc" | "desc";
  value?: (row: T) => string | number | null | undefined;
}

export type TableProps<T> = {
  columns: ITableColumn<T>[];
  rows?: T[];
  className?: string;
  height?: string | number;
  rowClickHandler?: (row: T) => void;
  headerClickHandler?: (index: number) => void;
  getColumnTotal?: (column: ITableColumn<T>) => number | undefined;
  sort?: {
    column: number;
    direction: "asc" | "desc";
  };
  selection?: boolean;
  selectedIndex?: number;
  setSelectedIndex?: (index: number) => void;
  tableRows?: {
    current: Array<HTMLDivElement | null>;
  };
};

export type UseTableProps<T> = {
  columns: ITableColumn<T>[];
  rows?: T[];
  equals?: (a: T, b: T) => boolean;
  getColumnTotal?: (column: ITableColumn<T>) => number | undefined;
  selection?: boolean;
  selectedIndex?: number;
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

export function useTable<T>(props: UseTableProps<T>) {
  const [selectedIndex, setSelectedIndex] = useState(0);
  const tableRows = useRef<Array<HTMLDivElement | null>>([]);

  const [sort, setSort] = useState<{
    column: number;
    direction: "asc" | "desc";
  }>(getInitialSort(props.columns));

  const rows = useMemo(() => {
    if (props.rows === undefined) {
      return undefined;
    }

    const sorted = [...(props.rows ?? [])];

    sorted.sort((a, b) => {
      const column = props.columns[sort.column];

      if (column?.value !== undefined) {
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
    return selectedIndex !== undefined ? rows?.[selectedIndex] : undefined;
  }, [selectedIndex, rows]);

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

  useKeyPress("ArrowDown", props.selection ? setNextIndex : undefined);
  useKeyPress("ArrowUp", props.selection ? setPreviousIndex : undefined);

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

  const getColumnTotal = useCallback(
    (column: ITableColumn<T>) => {
      if (
        props.rows === undefined ||
        column.value === undefined ||
        props.rows[0] === undefined ||
        !(typeof column.value(props.rows[0]) === "number")
      ) {
        return undefined;
      }

      const total = props.rows.reduce(
        (sum, row) => sum + (column.value?.(row) as number),
        0
      );

      return total;
    },
    [props.rows]
  );

  return {
    selectedItem,
    props: {
      columns: props.columns,
      handleHeaderClick,
      getColumnTotal,
      sort,
      rows,
      selection: props.selection,
      setIndex,
      tableRows,
      selectedIndex,
      setSelectedIndex
    }
  };
}
