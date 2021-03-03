import React, { useState, useEffect } from 'react';
import {
  Box,
  Container,
  makeStyles,
  // Card,
  // CardContent,
  // CardActions,
  Typography
} from '@material-ui/core';
import { forwardRef } from 'react';
import MaterialTable from 'material-table';
import { TablePagination } from '@material-ui/core';
import TablePaginationActions from '../../../components/TablePaginationActions';
import Page from 'src/components/Page';
import { fetchReportSessions } from 'src/actions/session';
// import Toolbar from './Toolbar';
import { useDispatch, useSelector } from 'react-redux';
import Spinner from 'src/components/Spinner';
import AlertMessage from 'src/components/AlertMessage';
// import ReportList from './ReportList';
// import PerfectScrollbar from 'react-perfect-scrollbar';
import FullScreenReport from '../ReportView/index';
// import { useReactToPrint } from 'react-to-print';
// import adminIJoin from 'src/api/adminIJoin';
import BackdropSpinner from 'src/components/Backdrop';
// import userIJoin from 'src/api/userIJoin';
import adminIJoin from 'src/api/adminIJoin';
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
import { Assignment } from '@material-ui/icons';
import moment from 'moment';
const useStyles = makeStyles((theme) => ({
  root: {
    backgroundColor: theme.palette.background.dark,
    minHeight: '100%',
    paddingBottom: theme.spacing(3),
    paddingTop: theme.spacing(3)
  },
  content: {
    padding: 0
  },
  actions: {
    justifyContent: 'flex-end',
    padding: 0
  }
}));

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

const GenReportListView = () => {
  const classes = useStyles();
  const [openSnackbar, setOpenSnackbar] = useState(false);
  const [openReport, setOpenReport] = React.useState(false);
  const [reportLoading, setReportLoading] = React.useState(false);
  const dispatch = useDispatch();
  const report = useSelector((state) => state.report);
  const { loading, errmsg } = useSelector((state) => state.reportStatus);

  const [reportData, setReportData] = React.useState({});
  const [reportDataList, setReportDataList] = React.useState([]);

  var columns = [
    { field: 'courseId', title: 'Course ID', searchable: true },
    { field: 'sessionId', title: 'Session ID', searchable: true },
    { field: 'courseName', title: 'Course Name', searchable: true },
    { field: 'sessionName', title: 'Session Name', searchable: true },
    { field: 'companyCode', title: 'Company', searchable: true },
    {
      field: 'startDateTime',
      title: 'Start Date Time',

      render: (rowData) =>
        moment(rowData.startDateTime).format('DD/MM/YYYY hh:mm A'),
      searchable: false
    },
    {
      field: 'endDateTime',
      title: 'End Date Time',
      render: (rowData) =>
        moment(rowData.endDateTime).format('DD/MM/YYYY hh:mm A'),
      searchable: false
    }
  ];

  const handleOpenReport = async (row) => {
    console.log(row);
    setReportData(row);

    // dispatch(fetchSessionReport(row.sessionId));
    setReportLoading(true);
    await adminIJoin
      .post('Sessions/Report', {
        sessionId: row.sessionId
      })
      .then((res) => {
        // console.log(res.data);
        setReportDataList(res.data);
        setOpenReport(true);
        setReportLoading(false);
      })
      .catch((err) => {
        // console.log(err.message);

        setReportLoading(false);
        setOpenSnackbar(true);
      });
  };

  const handleCloseReport = () => {
    setOpenReport(false);
  };

  // const onSearchSubmit = (sessionId) => {
  //   console.log(sessionId);
  //   dispatch(fetchReportSessions(sessionId));
  //   setOpenSnackbar(true);
  // };

  const handleSnackbarClose = () => {
    setOpenSnackbar(false);
  };

  useEffect(() => {
    dispatch(fetchReportSessions(''));
    setOpenSnackbar(true);
  }, [dispatch]);

  // console.log(session);

  return (
    <React.Fragment>
      <Page className={classes.root} title="Generate Report">
        <Container maxWidth={false}>
          {/* <Toolbar onSearchSubmit={onSearchSubmit} /> */}
          <Box>
            {loading ? (
              <Spinner />
            ) : (
              <React.Fragment>
                {report.length > 0 ? (
                  <MaterialTable
                    title=""
                    columns={columns}
                    data={report}
                    icons={tableIcons}
                    actions={[
                      {
                        icon: () => <Assignment />,
                        tooltip: 'View',
                        onClick: (event, rowData) => handleOpenReport(rowData)
                        // alert('You saved ' + rowData.name)
                      }
                    ]}
                    options={{
                      actionsColumnIndex: -1,
                      searchFieldAlignment: 'left',
                      searchFieldStyle: {
                        width: 700
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
                        searchTooltip:
                          'Search By : Course ID, Session ID, Course Name, Session Name, Company',
                        searchPlaceholder:
                          'Search By : Course ID, Session ID, Course Name, Session Name, Company'
                      }
                    }}
                  />
                ) : (
                  <Typography color="error" gutterBottom variant="h5">
                    {`ไม่พบข้อมูลที่ค้นหา`}
                  </Typography>
                )}
              </React.Fragment>
            )}
          </Box>
        </Container>

        <Box mt={3}></Box>

        {reportLoading ? (
          <BackdropSpinner isLoading={reportLoading} />
        ) : (
          <FullScreenReport
            reportHeaderData={reportData}
            reportListData={reportDataList}
            isOpen={openReport}
            onClose={handleCloseReport}
          />
        )}

        {reportLoading && <BackdropSpinner isLoading={reportLoading} />}

        {errmsg && (
          <AlertMessage
            openSnackbar={openSnackbar}
            handleSnackbarClose={handleSnackbarClose}
            message={errmsg.toString()}
            severity={'error'}
          />
        )}
      </Page>
    </React.Fragment>
  );
};

export default GenReportListView;
