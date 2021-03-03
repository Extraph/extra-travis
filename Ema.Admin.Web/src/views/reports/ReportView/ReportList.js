import React from 'react';
// import { Typography } from '@material-ui/core';
// import { makeStyles } from '@material-ui/styles';
import { forwardRef } from 'react';
import AddBox from '@material-ui/icons/AddBox';
import ArrowDownward from '@material-ui/icons/ArrowDownward';
import Check from '@material-ui/icons/Check';
import ChevronLeft from '@material-ui/icons/ChevronLeft';
import ChevronRight from '@material-ui/icons/ChevronRight';
import Clear from '@material-ui/icons/Clear';
import DeleteOutline from '@material-ui/icons/DeleteOutline';
import Edit from '@material-ui/icons/Edit';
import FilterList from '@material-ui/icons/FilterList';
import FirstPage from '@material-ui/icons/FirstPage';
import LastPage from '@material-ui/icons/LastPage';
import Remove from '@material-ui/icons/Remove';
import SaveAlt from '@material-ui/icons/SaveAlt';
import Search from '@material-ui/icons/Search';
import ViewColumn from '@material-ui/icons/ViewColumn';
import MaterialTable from 'material-table';
import { TablePagination } from '@material-ui/core';
import TablePaginationActions from '../../../components/TablePaginationActions';
// import { Cancel, Print, Undo, AddPhotoAlternate } from '@material-ui/icons';

const tableIcons = {
  Add: forwardRef((props, ref) => <AddBox {...props} ref={ref} />),
  Check: forwardRef((props, ref) => <Check {...props} ref={ref} />),
  Clear: forwardRef((props, ref) => <Clear {...props} ref={ref} />),
  Delete: forwardRef((props, ref) => <DeleteOutline {...props} ref={ref} />),
  DetailPanel: forwardRef((props, ref) => (
    <ChevronRight {...props} ref={ref} />
  )),
  Edit: forwardRef((props, ref) => <Edit {...props} ref={ref} />),
  Export: forwardRef((props, ref) => <SaveAlt {...props} ref={ref} />),
  Filter: forwardRef((props, ref) => <FilterList {...props} ref={ref} />),
  FirstPage: forwardRef((props, ref) => <FirstPage {...props} ref={ref} />),
  LastPage: forwardRef((props, ref) => <LastPage {...props} ref={ref} />),
  NextPage: forwardRef((props, ref) => <ChevronRight {...props} ref={ref} />),
  PreviousPage: forwardRef((props, ref) => (
    <ChevronLeft {...props} ref={ref} />
  )),
  ResetSearch: forwardRef((props, ref) => <Clear {...props} ref={ref} />),
  Search: forwardRef((props, ref) => <Search {...props} ref={ref} />),
  SortArrow: forwardRef((props, ref) => <ArrowDownward {...props} ref={ref} />),
  ThirdStateCheck: forwardRef((props, ref) => <Remove {...props} ref={ref} />),
  ViewColumn: forwardRef((props, ref) => <ViewColumn {...props} ref={ref} />)
};

// const tableHead = [
//   { id: 'no', label: 'No.', align: 'center' },
//   { id: 'userId', label: 'Emp. ID', align: 'center', minWidth: 50 },
//   {
//     id: 'userNameSurname',
//     label: 'Name - Lastname',
//     align: 'left',
//     minWidth: 150
//   },
//   { id: 'userDepartment', label: 'Department', align: 'left', minWidth: 150 },
//   { id: 'userCompany', label: 'Company', align: 'left', minWidth: 100 },

//   { id: 'date', label: 'Date', align: 'center', minWidth: 100 },
//   { id: 'checkInTime', label: 'Check-in', align: 'center', minWidth: 100 },
//   { id: 'checkOutTime', label: 'Check-out', align: 'center', minWidth: 100 },
//   {
//     id: 'trainingStatus',
//     label: 'Training Status',
//     align: 'center',
//     minWidth: 100
//   }
//   // {
//   //   id: 'startDateTime',
//   //   label: 'Start Date Time',
//   //   align: 'center',
//   //   minWidth: 150,
//   //   format: 'datetime'
//   // },
//   // {
//   //   id: 'endDateTime',
//   //   label: 'End Date Time',
//   //   align: 'center',
//   //   minWidth: 150,
//   //   format: 'datetime'
//   // }
// ];

const ReportList = ({ data }) => {
  var columns = [
    { field: 'no', title: 'No.' },
    { field: 'userId', title: 'Emp. ID' },
    { field: 'userNameSurname', title: 'Name - Lastname' },
    { field: 'userDepartment', title: 'Department' },
    { field: 'userCompany', title: 'Company' },
    { field: 'date', title: 'Date' },
    { field: 'checkInTime', title: 'Check-in' },
    { field: 'checkOutTime', title: 'Check-out' },
    { field: 'trainingStatus', title: 'Training Status' }
  ];

  return (
    <React.Fragment>
      <MaterialTable
        title=""
        columns={columns}
        data={data}
        icons={tableIcons}
        options={{
          searchFieldAlignment: 'left',
          searchFieldStyle: {
            width: 600
          }
        }}
        components={{
          Pagination: (props) => (
            <TablePagination
              {...props}
              ActionsComponent={TablePaginationActions}
            />
          )
        }}
        localization={{
          toolbar: {
            searchTooltip: 'Search By : All',
            searchPlaceholder: 'Search By : All'
          }
        }}
      />
      {/* <Table tableHeaderColor="info" tableHead={tableHead} tableData={data} /> */}
    </React.Fragment>
  );
};

export default ReportList;
