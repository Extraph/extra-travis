import React from 'react';
import {
  Table,
  TableHead,
  TableRow,
  TableBody,
  TableCell,
  TableFooter,
  TablePagination,
  Link
} from '@material-ui/core';
// import { NavLink as RouterLink } from 'react-router-dom';
// import { makeStyles } from '@material-ui/core/styles';
import moment from 'moment';

// const useStyles = makeStyles((theme) => ({
//   tableActionButton: {
//     // width: '18px',
//     // height: '18px',
//     // padding: '0'
//   },
//   tableActionButtonIcon: {
//     // width: '12px',
//     // height: '12px'
//   }
// }));

export default function CustomTable(props) {
  // const classes = useStyles();

  const { tableHead, tableData, onTableNavigate } = props;

  const [page, setPage] = React.useState(0);
  const [rowsPerPage, setRowsPerPage] = React.useState(5);

  const emptyRows =
    rowsPerPage - Math.min(rowsPerPage, tableData.length - page * rowsPerPage);

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };
  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  return (
    <div>
      <Table size="small">
        {tableHead !== undefined ? (
          <TableHead>
            <TableRow>
              {tableHead.map((column) => {
                return (
                  <TableCell
                    key={column.id}
                    align="center"
                    style={{ minWidth: column.minWidth }}
                  >
                    {column.label}
                  </TableCell>
                );
              })}
            </TableRow>
          </TableHead>
        ) : null}
        <TableBody>
          {tableData
            .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
            .map((row, index) => {
              return (
                <TableRow hover key={index}>
                  {tableHead.map((column) => {
                    const value = row[column.id];

                    if (
                      column.id === 'registrationStatus' &&
                      value !== 'Check-Out'
                    ) {
                      return (
                        <TableCell key={column.id} align={column.align}>
                          <Link
                            // component={RouterLink}
                            // to={'/app/participant/edit'}
                            onClick={() => onTableNavigate(row)}
                          >
                            {value}
                          </Link>
                        </TableCell>
                      );
                    } else {
                      return (
                        <TableCell key={column.id} align={column.align}>
                          {column.format === 'datetime'
                            ? moment(value).format('DD/MM/YYYY hh:mm A')
                            : value}
                        </TableCell>
                      );
                    }
                  })}
                </TableRow>
              );
            })}

          {emptyRows > 0 && (
            <TableRow style={{ height: 38 * emptyRows }}>
              <TableCell colSpan={tableHead.length + 1} />
            </TableRow>
          )}
        </TableBody>
        <TableFooter>
          <TableRow>
            <TablePagination
              labelRowsPerPage={'Rows'}
              rowsPerPageOptions={[5, 10, 25]}
              colSpan={tableHead.length}
              count={tableData.length}
              rowsPerPage={rowsPerPage}
              page={page}
              SelectProps={{
                inputProps: { 'aria-label': 'rows per page' }
              }}
              onChangePage={handleChangePage}
              onChangeRowsPerPage={handleChangeRowsPerPage}
              // ActionsComponent={TablePaginationActions}
            />
          </TableRow>
        </TableFooter>
      </Table>
    </div>
  );
}

CustomTable.defaultProps = {
  isRowSelect: false
};
