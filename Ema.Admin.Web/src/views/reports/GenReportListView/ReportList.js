import React from 'react';
// import { Typography } from '@material-ui/core';
// import { makeStyles } from '@material-ui/styles';
import Table from 'src/components/TableReport';

const tableHead = [
  { id: 'courseId', label: 'Course ID', align: 'center', minWidth: 150 },
  { id: 'sessionId', label: 'Session ID', align: 'center', minWidth: 150 },
  { id: 'courseName', label: 'Course Name', align: 'left', minWidth: 150 },
  { id: 'sessionName', label: 'Session Name', align: 'left', minWidth: 150 },
  { id: 'companyCode', label: 'Company', align: 'center', minWidth: 200 },
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

const ReportList = ({ report, onReportView }) => {
  return (
    <React.Fragment>
      <Table
        tableHeaderColor="info"
        tableHead={tableHead}
        tableData={report}
        onReportView={onReportView}
      />
    </React.Fragment>
  );
};

export default ReportList;
