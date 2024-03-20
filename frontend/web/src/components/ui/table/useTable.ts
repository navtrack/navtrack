import {
  ReactNode,
  useCallback,
  useEffect,
  useMemo,
  useRef,
  useState
} from "react";
import { useKeyPress } from "@navtrack/shared/hooks/util/useKeyPress";

export interface ITableColumn<T> {
  labelId?: string;
  row: (row: T) => ReactNode;
  rowClassName?: string;
  footer?: (rows?: T[]) => ReactNode;
  footerClassName?: string;
  footerColSpan?: number;
  sort?: "asc" | "desc";
  sortValue?: (row: T) => string | number | null | undefined;
  sortable?: boolean;
}

export type TableProps<T> = {
  columns: ITableColumn<T>[];
  rows?: T[];
  className?: string;
  equals?: (a: T, b: T) => boolean;
  selectedItem?: T;
  setSelectedItem?: (item?: T) => void;
  rowClick?: (row: T) => void;
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

export function useTable<T>(props: TableProps<T>) {
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

      if (column.sortValue !== undefined) {
        const aValue = column.sortValue(a);
        const bValue = column.sortValue(b);

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

  return {
    handleHeaderClick,
    sort,
    sortedRows,
    hasFooter,
    selectionEnabled,
    setIndex,
    tableRows,
    selectedIndex
  };
}
