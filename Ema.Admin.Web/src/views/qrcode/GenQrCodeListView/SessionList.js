import React from 'react';
// import { Typography } from '@material-ui/core';
// import { makeStyles } from '@material-ui/styles';
import { forwardRef } from 'react';
import MaterialTable from 'material-table';
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
import { TablePagination } from '@material-ui/core';
import TablePaginationActions from '../../../components/TablePaginationActions';
import { Cancel, Print, Undo, AddPhotoAlternate } from '@material-ui/icons';
import moment from 'moment';

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

const SessionList = ({
  session,
  onTablePrint,
  onTableCancel,
  onTableUndoCancel,
  onTableCoverPhoto
}) => {
  var columns = [
    { field: 'courseId', title: 'Course ID', searchable: true },
    { field: 'sessionId', title: 'Session ID', searchable: true },
    { field: 'courseName', title: 'Course Name', searchable: false },
    { field: 'sessionName', title: 'Session Name', searchable: false },
    {
      field: 'companyCode',
      title: 'Company',
      align: 'center',
      searchable: false
    },
    {
      field: 'startDateTime',
      title: 'Start Date Time',
      align: 'center',
      render: (rowData) =>
        moment(rowData.startDateTime).format('DD/MM/YYYY hh:mm A'),
      searchable: false
    },
    {
      field: 'endDateTime',
      title: 'End Date Time',
      align: 'center',
      render: (rowData) =>
        moment(rowData.endDateTime).format('DD/MM/YYYY hh:mm A'),
      searchable: false
    },
    {
      title: 'Cover Photo',
      field: 'coverPhotoUrl',
      align: 'center',
      render: (rowData) => (
        <img
          src={rowData.coverPhotoUrl}
          style={{ width: 80, borderRadius: '5%' }}
          alt=""
        />
      )
    }
  ];

  return (
    <React.Fragment>
      <MaterialTable
        title=""
        columns={columns}
        data={session}
        icons={tableIcons}
        actions={[
          {
            icon: () => <AddPhotoAlternate />,
            tooltip: 'Upload Cover Photo',
            onClick: (event, rowData) => onTableCoverPhoto(rowData)
          },
          {
            icon: () => <Print />,
            tooltip: 'Print QR',
            onClick: (event, rowData) => onTablePrint(rowData)
          },
          (rowData) => ({
            icon: () => <Cancel />,
            tooltip: 'Cancel Session',
            onClick: (event, rowData) => onTableCancel(rowData),
            disabled: rowData.isCancel === '1'
          }),
          (rowData) => ({
            icon: () => <Undo />,
            tooltip: 'Undo Session',
            onClick: (event, rowData) => onTableUndoCancel(rowData),
            disabled: rowData.isCancel === '0'
          })
        ]}
        options={{
          // actionsColumnIndex: -1,
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
            searchTooltip: 'Search By : Course ID, Session ID',
            searchPlaceholder: 'Search By : Course ID, Session ID'
          }
        }}
      />
    </React.Fragment>
  );
};

export default SessionList;
