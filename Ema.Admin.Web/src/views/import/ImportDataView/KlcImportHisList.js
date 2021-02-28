import React from 'react';
import { Typography } from '@material-ui/core';
import { makeStyles } from '@material-ui/styles';
import Table from 'src/components/Table';

const tableHead = [
  { id: 'fileName', label: 'File Name', align: 'center', minWidth: 150 },
  { id: 'status', label: 'Status', align: 'center', minWidth: 150 },
  { id: 'importBy', label: 'Import By', align: 'center', minWidth: 150 },
  {
    id: 'importTotalrecords',
    label: 'Total Records',
    align: 'center',
    minWidth: 150
  },
  {
    id: 'importType',
    label: 'Import Type',
    align: 'center',
    minWidth: 150
  },
  {
    id: 'importMessage',
    label: 'Import Message',
    align: 'center',
    minWidth: 150
  },
  {
    id: 'createdatetime',
    label: 'Create Datetime',
    align: 'center',
    minWidth: 150,
    format: 'datetime'
  }
];

const useStyles = makeStyles((theme) => ({
  danger: {
    color: theme.palette.error.main
  }
}));

const KlcImportHisList = (props) => {
  const classes = useStyles();
  const { onTableRowSelect } = props;
  const { dataList, message, success } = props.importResponse;

  return (
    <React.Fragment>
      {success ? (
        <Table
          onTableRowSelect={onTableRowSelect}
          tableHeaderColor="info"
          tableHead={tableHead}
          tableData={dataList}
        />
      ) : (
        <Typography className={classes.danger} variant="h6">
          {message}
        </Typography>
      )}
    </React.Fragment>
  );
};

export default KlcImportHisList;
