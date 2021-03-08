import React from 'react';
// import { Typography } from '@material-ui/core';
// import { makeStyles } from '@material-ui/styles';
import Table from 'src/components/TableMobile';

const tableHead = [
  { id: 'userId', label: 'Emp ID', align: 'center' },
  { id: '', label: 'Emp Name', align: 'center' },
  { id: 'registrationStatus', label: 'Status', align: 'center' }
];

const ParticipantNextList = ({ tableData, onTableNavigate }) => {
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

export default ParticipantNextList;
