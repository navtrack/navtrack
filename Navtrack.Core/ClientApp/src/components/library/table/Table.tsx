import React, { ReactNode } from "react";
import TablePagination from "./TablePagination";
import { TableResponse } from "apis/types/TableResponse";

export type Props<T extends object> = {
  currentPage: number;
  setCurrentPage: (page: number) => void;
  columns: TableColumn<T>[];
  data: TableResponse<T>;
};

export default function Table<T extends object>(props: Props<T>) {
  return (
    <>
      <table className="w-full text-left whitespace-no-wrap">
        <thead className="text-xs p-2 text-gray-900 uppercase">
          <tr>
            {props.columns.map((column) => (
              <th className="py-2 px-1" key={column.title}>
                {column.title}
              </th>
            ))}
          </tr>
        </thead>
        <tbody className="text-gray-800">
          {props.data.results.map((row, i) => (
            <tr className="border-b border-t" key={i}>
              {props.columns.map((column) => (
                <th className="py-1 px-1 font-normal text-sm" key={column.title}>
                  {column.renderer(row)}
                </th>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
      <TablePagination
        currentPage={props.data.page}
        setCurrentPage={props.setCurrentPage}
        data={props.data}
      />
    </>
  );
}

export type TableColumn<T extends object> = {
  title: string;
  renderer: (row: T) => ReactNode;
};
