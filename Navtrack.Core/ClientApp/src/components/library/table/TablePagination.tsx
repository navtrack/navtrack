import React from "react";
import classNames from "classnames";
import { getRangeArray } from "framework/util/NumberUtil";
import { TableResponse } from "apis/types/TableResponse";

export type Props<T extends object> = {
  currentPage: number;
  setCurrentPage: (page: number) => void;
  data: TableResponse<T>;
};

export default function TablePagination<T extends object>(props: Props<T>) {
  const startIndex = (props.currentPage - 1) * props.data.perPage + 1;
  const endIndex = startIndex + props.data.perPage - 1;

  const setPage = (page: number): void => {
    if (page >= 1 && page <= props.data.maxPage) {
      props.setCurrentPage(page);
    }
  };

  return (
    <div className="flex justify-center items-center py-3 text-sm select-none">
      <div className="text-xs" style={{ flexGrow: 1, flexBasis: 150 }}>
        {startIndex} to {endIndex} of {props.data.totalResults} results
      </div>
      <div className="flex justify-center items-center" style={{ flexGrow: 3 }}>
        <div className="mr-4 cursor-pointer" onClick={() => setPage(props.currentPage - 1)}>
          <i className="fas fa-angle-left mr-2" />
          <span>Previous</span>
        </div>
        {getPages(props.currentPage, props.data.maxPage).map((x) => (
          <div
            key={x}
            className={classNames(
              "flex mx-1 cursor-pointer px-3 py-1 rounded-lg border border-transparent hover:border-gray-200",
              props.currentPage === x ? "bg-gray-900 text-white border-gray-900" : ""
            )}
            onClick={() => setPage(x)}>
            {x}
          </div>
        ))}
        <div className="ml-4 cursor-pointer" onClick={() => setPage(props.currentPage + 1)}>
          <span>Next</span>
          <i className="fas fa-angle-right ml-2" />
        </div>
      </div>
      <div style={{ flexGrow: 1, flexBasis: 150 }}></div>
    </div>
  );
}

const getPages = (currentPage: number, maxPage: number): number[] => {
  let start = getStartPage(currentPage, maxPage);
  let end = getEndPage(currentPage, maxPage);

  return getRangeArray(start, end);
};

function getStartPage(currentPage: number, maxPage: number): number {
  const maxSidePages = 4 - (maxPage - currentPage);
  const startPage = currentPage - Math.max(2, maxSidePages);

  return Math.max(1, startPage);
}

function getEndPage(currentPage: number, maxPage: number): number {
  const endPage = currentPage + Math.max(2, 5 - currentPage);

  return Math.min(endPage, maxPage);
}
