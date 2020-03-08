import React from "react";
import { useTable, usePagination, useSortBy } from "react-table";
import classNames from "classnames";

export default function ReactTable({ columns, data }) {
  const isLoading = false;

  const {
    getTableProps,
    getTableBodyProps,
    headerGroups,
    prepareRow,
    page,
    canPreviousPage,
    canNextPage,
    pageCount,
    gotoPage,
    nextPage,
    previousPage,
    setPageSize,
    state: { pageIndex, pageSize }
  } = useTable(
    {
      columns,
      data,
      initialState: { pageIndex: 0 }
    },
    useSortBy,
    usePagination
  );

  return (
    <div className="w-full">
      <table {...getTableProps()} className="w-full">
        <thead className="border-b border-t bg-gray-100 text-left text-xs p-2 text-gray-700 uppercase">
          {headerGroups.map(headerGroup => (
            <tr {...headerGroup.getHeaderGroupProps()} className="border-b">
              {headerGroup.headers.map(column => (
                <th {...column.getHeaderProps(column.getSortByToggleProps())} className="p-2 font-medium">
                  {column.render("Header")}
                  <span>
                    {column.isSorted
                      ? column.isSortedDesc
                        ? <i className="fas fa-sort-down fa-lg" />
                        : <i className="fas fa-sort-up fa-lg" />
                      : ''}
                  </span>
                </th>
              ))}
            </tr>
          ))}
        </thead>
        <tbody {...getTableBodyProps()} className="text-sm">
          {page.map(
            row =>
              prepareRow(row) || (
                <tr {...row.getRowProps()} className="border-b">
                  {row.cells.map(cell => {
                    return <td {...cell.getCellProps()} className="p-3">{cell.render("Cell")}</td>;
                  })}
                </tr>
              )
          )}
        </tbody>
      </table>
      {isLoading ?
        <div className="py-4 text-center">
          Loading...
          </div>
        :
        data.length > 0 ?
          <div className="flex flex-grow p-4 items-center text-sm">
            <div className="flex-grow">
              <div className="inline-block relative shadow rounded">
                <select className="block appearance-none w-full bg-white px-4 py-1 pr-8 cursor-pointer focus:outline-none bg-gray-200 hover:bg-gray-300"
                  onChange={(e) => setPageSize(e.target.value)}>
                  {[10, 20, 50, 100].map(pageSize => <option key={pageSize} value={pageSize}>Show {pageSize} items</option>)}
                </select>
                <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 pt-1">
                  <i className="fas fa-chevron-down" />
                </div>
              </div>
            </div>
            <div className="flex-grow">
              Showing {pageSize * pageIndex + 1} to {pageSize * (pageIndex + 1)}  of {data.length} entries.
            </div>
            <div className="flex-grow flex justify-end">
              <div className="shadow bg-gray-100 rounded">
                <button className={classNames("bg-gray-200 w-10 p-1 focus:outline-none", { "cursor-not-allowed": !canPreviousPage, "hover:bg-gray-300": canPreviousPage })}
                  onClick={() => gotoPage(0)} >
                  <i className="fas fa-angle-double-left" />
                </button>
                <button className={classNames("bg-gray-200 w-10 p-1 focus:outline-none", { "cursor-not-allowed": !canPreviousPage, "hover:bg-gray-300": canPreviousPage })}
                  onClick={() => previousPage()}>
                  <i className="fas fa-angle-left" />
                </button>
                {/* TODO: auto width */}
                <button className="bg-gray-200 w-10 p-1 focus:outline-none pointer-events-none" >
                  {pageIndex + 1}
                </button>
                <button className={classNames("bg-gray-200 w-10 p-1 focus:outline-none", { "cursor-not-allowed": !canNextPage, "hover:bg-gray-300": canNextPage })}
                  onClick={() => nextPage()}>
                  <i className="fas fa-angle-right" />
                </button>
                <button className={classNames("bg-gray-200 w-10 p-1 focus:outline-none", { "cursor-not-allowed": !canNextPage, "hover:bg-gray-300": canNextPage })}
                  onClick={() => gotoPage(pageCount - 1)}>
                  <i className="fas fa-angle-double-right" />
                </button>
              </div>
            </div>
          </div>
          :
          <div className="py-4 text-center">No items.</div>}
    </div>
  );
}