import React from 'react';
import {
  Table,
  TableHead,
  TableRow,
  TableBody,
  TableCell,
  TableFooter,
  TablePagination,
  IconButton,
  Fab,
  Menu,
  MenuItem,
  Tooltip
} from '@material-ui/core';
import TablePaginationActions from './TablePaginationActions';
import { CropFree, Cancel, Print, Undo } from '@material-ui/icons';
import { makeStyles, withStyles } from '@material-ui/core/styles';
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

function Row(props) {
  const {
    row,
    tableHead,
    onTablePrint,
    onTableCancel,
    onTableUndoCancel
  } = props;
  const classes = useStyles();

  const [anchorEl, setAnchorEl] = React.useState(null);
  const [anchorUndoEl, setAnchorUndoEl] = React.useState(null);

  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleUndoClick = (event) => {
    setAnchorUndoEl(event.currentTarget);
  };

  const handleUndoClose = () => {
    setAnchorUndoEl(null);
  };

  const StyledMenuItem = withStyles((theme) => ({
    root: {
      // '&:focus': {
      //   backgroundColor: theme.palette.error.main,
      //   '& .MuiListItemIcon-root, & .MuiListItemText-primary': {
      //     color: theme.palette.common.white
      //   }
      // }
    }
  }))(MenuItem);

  return (
    <React.Fragment>
      <TableRow hover>
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
          {row['isCancel'] !== '1' ? (
            <Fab variant="extended" onClick={() => onTablePrint(row)}>
              <Print className={classes.extendedIcon} />
              QR
            </Fab>
          ) : (
            'Cancelled'
          )}
        </StyledTableCell>

        <StyledTableCell align="center">
          {row['isCancel'] === '1' ? (
            <Tooltip title="Undo Cancel Session">
              <IconButton onClick={handleUndoClick}>
                <Undo />
              </IconButton>
            </Tooltip>
          ) : (
            <Tooltip title="Cancel Session">
              <IconButton onClick={handleClick}>
                <Cancel />
              </IconButton>
            </Tooltip>
          )}

          <Menu
            anchorEl={anchorEl}
            keepMounted
            open={Boolean(anchorEl)}
            onClose={handleClose}
          >
            <StyledMenuItem
              onClick={() => {
                onTableCancel(row);
                handleClose();
              }}
            >
              Comfirm Cancel!!!
            </StyledMenuItem>
          </Menu>

          <Menu
            anchorEl={anchorUndoEl}
            keepMounted
            open={Boolean(anchorUndoEl)}
            onClose={handleUndoClose}
          >
            <StyledMenuItem
              onClick={() => {
                onTableUndoCancel(row);
                handleUndoClose();
              }}
            >
              Comfirm Undo!!!
            </StyledMenuItem>
          </Menu>
        </StyledTableCell>
      </TableRow>
    </React.Fragment>
  );
}

export default function CustomTable(props) {
  const classes = useStyles();

  const {
    tableHead,
    tableData,
    onTablePrint,
    onTableCancel,
    onTableUndoCancel
  } = props;

  const [page, setPage] = React.useState(0);
  const [rowsPerPage, setRowsPerPage] = React.useState(10);

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
                <IconButton className={classes.tableActionButton}>
                  <CropFree />
                </IconButton>
              </StyledTableCell>
              <StyledTableCell align="center">
                <IconButton className={classes.tableActionButton}>
                  <Cancel />
                </IconButton>
              </StyledTableCell>
            </TableRow>
          </TableHead>
        ) : null}
        <TableBody>
          {tableData
            .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
            .map((row, index) => {
              return (
                <Row
                  key={index}
                  row={row}
                  tableHead={tableHead}
                  onTablePrint={onTablePrint}
                  onTableCancel={onTableCancel}
                  onTableUndoCancel={onTableUndoCancel}
                />
              );
            })}

          {emptyRows > 0 && (
            <TableRow style={{ height: 38 * emptyRows }}>
              <StyledTableCell colSpan={tableHead.length + 1} />
            </TableRow>
          )}
        </TableBody>
        <TableFooter>
          <TableRow>
            <TablePagination
              labelRowsPerPage={'Rows'}
              rowsPerPageOptions={[10, 25]}
              colSpan={tableHead.length + 2}
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
