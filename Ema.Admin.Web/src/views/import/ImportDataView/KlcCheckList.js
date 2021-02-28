import React from 'react';
import { Typography } from '@material-ui/core';
import { makeStyles } from '@material-ui/styles';
import Table from 'src/components/Table';

const tableHead = [
  {
    id: 'courseId',
    label: 'Course ID',
    align: 'center',
    minWidth: 150
  },
  {
    id: 'courseName',
    label: 'Course Name',
    align: 'left',
    minWidth: 200
  },
  {
    id: 'sessionId',
    label: 'Session ID',
    align: 'center',
    minWidth: 150
  },
  {
    id: 'sessionName',
    label: 'Session Name',
    align: 'left',
    minWidth: 150
  },
  {
    id: 'startDateTime',
    label: 'Start Date Time',
    align: 'center',
    minWidth: 150,
    format: 'datetime'
  },
  {
    id: 'endDateTime',
    label: 'End Date Time',
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

const KlcCheckList = (props) => {
  const classes = useStyles();
  const { onTableRowSelect } = props;
  const { dataCheck, message, success } = props.klcResponse;

  return (
    <React.Fragment>
      {success ? (
        <Table
          onTableRowSelect={onTableRowSelect}
          tableHeaderColor="info"
          tableHead={tableHead}
          tableData={dataCheck}
        />
      ) : (
        <Typography className={classes.danger} variant="h6">
          {message}
        </Typography>
      )}
    </React.Fragment>
  );
};

export default KlcCheckList;
