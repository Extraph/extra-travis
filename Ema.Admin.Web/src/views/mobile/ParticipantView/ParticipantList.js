import React from 'react';
// import { Typography } from '@material-ui/core';
// import { makeStyles } from '@material-ui/styles';
import Table from 'src/components/TableMobile';

const tableHead = [
  { id: 'userId', label: 'Emp ID', align: 'center' },
  { id: '', label: 'Emp Name', align: 'center' },
  { id: 'registrationStatus', label: 'Status', align: 'center' },
  {
    id: 'checkInDateTime',
    label: 'In',
    align: 'center'
  },
  {
    id: 'checkOutDateTime',
    label: 'Out',
    align: 'center'
  }
];

const ParticipantList = ({ tableData, onTableNavigate }) => {
  return (
    <React.Fragment>
      <Table
        tableHeaderColor="info"
        tableHead={tableHead}
        tableData={tableData}
        onTableNavigate={onTableNavigate}
      />
    </React.Fragment>
  );
};

export default ParticipantList;
