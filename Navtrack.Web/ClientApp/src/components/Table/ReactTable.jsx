import React from "react";
import { useTable, usePagination, useSortBy } from "react-table";
import {
  CardFooter,
  Pagination,
  PaginationItem,
  PaginationLink,
  Row,
  Col,
  DropdownToggle,
  DropdownMenu,
  DropdownItem,
  UncontrolledDropdown,
  CardBody
} from "reactstrap";

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
    pageOptions,
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
    <>
      <div className="card mb-0">
        <div className="table-responsive">
          <table {...getTableProps()} className="table">
            <thead className="thead-light">
              {headerGroups.map(headerGroup => (
                <tr {...headerGroup.getHeaderGroupProps()}>
                  {headerGroup.headers.map(column => (
                    <th {...column.getHeaderProps(column.getSortByToggleProps())} className="p-3">
                      {column.render("Header")}
                      <span>
                        {column.isSorted
                          ? column.isSortedDesc
                            ? <> <i className="fas fa-sort-down fa-lg" /></>
                            : <> <i className="fas fa-sort-up fa-lg" /></>
                          : ''}
                      </span>
                    </th>
                  ))}
                </tr>
              ))}
            </thead>
            <tbody {...getTableBodyProps()}>
              {page.map(
                row =>
                  prepareRow(row) || (
                    <tr {...row.getRowProps()}>
                      {row.cells.map(cell => {
                        return <td {...cell.getCellProps()} className="p-3">{cell.render("Cell")}</td>;
                      })}
                    </tr>
                  )
              )}
            </tbody>
          </table>
        </div>
        {isLoading ?
          <CardBody className="py-4 text-center">
            Loading...
          </CardBody>
          :
          data.length > 0 ?
            <CardFooter className="py-4">
              <Row className="align-items-center">
                <Col md="4">
                  <UncontrolledDropdown size="sm">
                    <DropdownToggle caret color="secondary">
                      Show {pageSize} items
                    </DropdownToggle>
                    <DropdownMenu>
                      {[10, 20, 50, 100].map(pageSize => (
                        <DropdownItem key={pageSize} onClick={() => setPageSize(pageSize)}>
                          Show {pageSize} items
                        </DropdownItem>
                      ))}
                    </DropdownMenu>
                  </UncontrolledDropdown>
                </Col>
                <Col md="4" className="text-center">
                  Page {pageIndex + 1} out of {pageOptions.length}
                </Col>
                <Col md="4">
                  <Pagination
                    className="pagination justify-content-end mb-0"
                    listClassName="justify-content-end mb-0">
                    <PaginationItem className={!canPreviousPage ? "disabled" : ""}>
                      <PaginationLink onClick={() => gotoPage(0)} tabIndex="-1">
                        <i className="fas fa-angle-double-left" />
                      </PaginationLink>
                    </PaginationItem>
                    <PaginationItem className={!canPreviousPage ? "disabled" : ""}>
                      <PaginationLink onClick={() => previousPage()} tabIndex="-1">
                        <i className="fas fa-angle-left" />
                      </PaginationLink>
                    </PaginationItem>
                    <PaginationItem className={!canNextPage ? "disabled" : ""}>
                      <PaginationLink onClick={() => nextPage()}>
                        <i className="fas fa-angle-right" />
                      </PaginationLink>
                    </PaginationItem>
                    <PaginationItem className={!canNextPage ? "disabled" : ""}>
                      <PaginationLink onClick={() => gotoPage(pageCount - 1)}>
                        <i className="fas fa-angle-double-right" />
                      </PaginationLink>
                    </PaginationItem>
                  </Pagination>
                </Col>
              </Row>
            </CardFooter>
            :
            <CardBody className="py-4 text-center">
              No items.
            </CardBody>}
      </div>
    </>
  );
}