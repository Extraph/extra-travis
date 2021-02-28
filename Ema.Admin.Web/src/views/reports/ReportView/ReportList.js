import React from 'react';
// import { Typography } from '@material-ui/core';
// import { makeStyles } from '@material-ui/styles';
import Table from 'src/components/TableReportDialog';

const tableHead = [
  { id: 'no', label: 'No.', align: 'center' },
  { id: 'userId', label: 'Emp. ID', align: 'center', minWidth: 50 },
  {
    id: 'userNameSurname',
    label: 'Name - Lastname',
    align: 'left',
    minWidth: 150
  },
  { id: 'userDepartment', label: 'Department', align: 'left', minWidth: 150 },
  { id: 'userCompany', label: 'Company', align: 'left', minWidth: 100 },

  { id: 'date', label: 'Date', align: 'center', minWidth: 100 },
  { id: 'checkInTime', label: 'Check-in', align: 'center', minWidth: 100 },
  { id: 'checkOutTime', label: 'Check-out', align: 'center', minWidth: 100 },
  {
    id: 'trainingStatus',
    label: 'Training Status',
    align: 'center',
    minWidth: 100
  }
  // {
  //   id: 'startDateTime',
  //   label: 'Start Date Time',
  //   align: 'center',
  //   minWidth: 150,
  //   format: 'datetime'
  // },
  // {
  //   id: 'endDateTime',
  //   label: 'End Date Time',
  //   align: 'center',
  //   minWidth: 150,
  //   format: 'datetime'
  // }
];

const ReportList = ({ data }) => {
  return (
    <React.Fragment>
      <Table tableHeaderColor="info" tableHead={tableHead} tableData={data} />
    </React.Fragment>
  );
};

export default ReportList;
