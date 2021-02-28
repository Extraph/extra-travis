import React from 'react';
import { Typography } from '@material-ui/core';
import { makeStyles } from '@material-ui/styles';
import Table from 'src/components/Table';

const tableHead = [
  {
    id: 'invalidMessage',
    label: 'Invalid Message',
    align: 'center',
    minWidth: 150
  },
  { id: 'courseType', label: 'Course Type', align: 'center', minWidth: 150 },
  {
    id: 'courseId',
    label: 'Course ID (*required)',
    align: 'center',
    minWidth: 150
  },
  {
    id: 'courseName',
    label: 'Course Name (*required)',
    align: 'center',
    minWidth: 200
  },
  {
    id: 'courseNameTh',
    label: 'Course Name (TH)',
    align: 'center',
    minWidth: 200
  },
  {
    id: 'sessionId',
    label: 'Session ID (*required)',
    align: 'center',
    minWidth: 150
  },
  {
    id: 'sessionName',
    label: 'Session Name (*required)',
    align: 'center',
    minWidth: 150
  },
  {
    id: 'segmentNo',
    label: 'Segment No. (*required)',
    align: 'center',
    minWidth: 150
  },
  { id: 'segmentName', label: 'Segment Name', align: 'center', minWidth: 150 },
  {
    id: 'startDate',
    label: 'Start Date (*required)',
    align: 'center',
    minWidth: 150
  },
  {
    id: 'endDate',
    label: 'End Date (*required)',
    align: 'center',
    minWidth: 150
  },
  {
    id: 'startTime',
    label: 'Start Time (*required)',
    align: 'center',
    minWidth: 150
  },
  {
    id: 'endTime',
    label: 'End Time (*required)',
    align: 'center',
    minWidth: 150
  },
  {
    id: 'courseOwnerEmail',
    label: 'Course Owner Email (*required)',
    align: 'center',
    minWidth: 150
  },
  {
    id: 'courseOwnerContactNo',
    label: 'Course Owner Contact No.',
    align: 'center',
    minWidth: 150
  },
  { id: 'venue', label: 'Venue', align: 'center', minWidth: 150 },
  { id: 'instructor', label: 'Instructor', align: 'center', minWidth: 150 },
  {
    id: 'courseCreditHours',
    label: 'Course Credit Hours',
    align: 'center',
    minWidth: 150
  },
  {
    id: 'passingCriteriaException',
    label: 'Passing Criteria Exception',
    align: 'center',
    minWidth: 150
  },
  { id: 'userCompany', label: 'User Company', align: 'center', minWidth: 150 },
  {
    id: 'userId',
    label: 'User ID (*required)',
    align: 'center',
    minWidth: 150
  },
  {
    id: 'registrationStatus',
    label: 'Registration Status (*required)',
    align: 'center',
    minWidth: 150
  }
];

const useStyles = makeStyles((theme) => ({
  danger: {
    color: theme.palette.error.main
  }
}));

const KlcInvalidList = (props) => {
  const classes = useStyles();
  const { onTableRowSelect } = props;
  const { dataInvalid, message, success } = props.klcResponse;

  return (
    <React.Fragment>
      {success ? (
        <Table
          onTableRowSelect={onTableRowSelect}
          tableHeaderColor="info"
          tableHead={tableHead}
          tableData={dataInvalid}
        />
      ) : (
        <Typography className={classes.danger} variant="h6">
          {message}
        </Typography>
      )}
    </React.Fragment>
  );
};

export default KlcInvalidList;
