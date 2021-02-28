import React from 'react';
import { makeStyles, withStyles } from '@material-ui/core/styles';

import {
  Table,
  TableHead,
  TableRow,
  TableBody,
  TableCell,
  TableFooter,
  TablePagination,
  Fab
} from '@material-ui/core';
import TablePaginationActions from './TablePaginationActions';
import { Assignment } from '@material-ui/icons';
import moment from 'moment';

const StyledTableCell = withStyles((theme) => ({
  body: {
    fontSize: 16
  }
}))(TableCell);

const useStyles = makeStyles((theme) => ({
  tableActionButton: {
    // width: '18px',
    // height: '18px',
    // padding: '0'
  },
  tableActionButtonIcon: {
    // width: '12px',
    // height: '12px'
  },
  extendedIcon: {
    marginRight: theme.spacing(1)
  }
}));

export default function CustomTable(props) {
  const classes = useStyles();

  const { tableHead, tableData, onReportView } = props;

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
                  <StyledTableCell
                    key={column.id}
                    align="center"
                    style={{ minWidth: column.minWidth }}
                  >
                    {column.label}
                  </StyledTableCell>
                );
              })}

              <StyledTableCell align="center">
                {/* <Fab variant="extended">
                  <Report className={classes.extendedIcon} />
                  QR
                </Fab> */}
                Action
              </StyledTableCell>
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
                    return (
                      <StyledTableCell key={column.id} align={column.align}>
                        {column.format === 'datetime'
                          ? moment(value).format('DD/MM/YYYY hh:mm A')
                          : value}
                      </StyledTableCell>
                    );
                  })}

                  <StyledTableCell align="center">
                    <Fab variant="extended" onClick={() => onReportView(row)}>
                      <Assignment className={classes.extendedIcon} />
                      View
                    </Fab>
                  </StyledTableCell>
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
              colSpan={tableHead.length + 1}
              count={tableData.length}
              rowsPerPage={rowsPerPage}
              page={page}
              SelectProps={{
                inputProps: { 'aria-label': 'rows per page' }
              }}
              onChangePage={handleChangePage}
              onChangeRowsPerPage={handleChangeRowsPerPage}
              ActionsComponent={TablePaginationActions}
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
