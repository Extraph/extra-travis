import React, { useState, useEffect } from 'react';
import { forwardRef } from 'react';
import { Box, Container, makeStyles, TablePagination } from '@material-ui/core';
import TablePaginationActions from '../../../components/TablePaginationActions';
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
import Page from 'src/components/Page';
import adminIJoin from 'src/api/adminIJoin';
import AlertMessage from 'src/components/AlertMessage';
import Spinner from 'src/components/Spinner';

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

const CompanyView = () => {
  const classes = useStyles();
  const [loading, setLoading] = React.useState(false);
  var columns = [
    { title: 'Company Id', field: 'companyId', hidden: true },
    { title: 'Company Code', field: 'companyCode' }
  ];
  const [data, setData] = useState([]); //table data

  const [iserror, setIserror] = useState(false);
  const [errorMessages, setErrorMessages] = useState([]);

  useEffect(() => {
    async function fetchData() {
      setLoading(true);
      await adminIJoin
        .get('/Company')
        .then((res) => {
          setData(res.data);
          setLoading(false);
        })
        .catch((err) => {
          console.log(err.message);
          setLoading(false);
        });
    }
    fetchData();
  }, []);

  const handleRowUpdate = async (newData, oldData, resolve) => {
    //console.log(oldData);

    //validation
    let errorList = [];
    if (newData.companyCode === undefined) {
      errorList.push('Please enter company code.');
    }

    if (errorList.length < 1) {
      await adminIJoin
        .put(`/Company/${newData.companyId}`, newData)
        .then((res) => {
          const dataUpdate = [...data];
          const index = oldData.tableData.id;
          dataUpdate[index] = newData;
          setData([...dataUpdate]);
          resolve();
          setIserror(false);
          setErrorMessages([]);
        })
        .catch((error) => {
          if (error.response.status === 409) {
            // throw new Error(`${err.config.url} not found`);
            setErrorMessages([
              `${newData.companyCode} company code already exists.`
            ]);
          } else {
            setErrorMessages(['Update failed! Server error']);
          }
          setIserror(true);
          resolve();
        });
    } else {
      setErrorMessages(errorList);
      setIserror(true);
      resolve();
    }
  };

  const handleRowAdd = async (newData, resolve) => {
    //validation
    let errorList = [];
    if (newData.companyCode === undefined) {
      errorList.push('Please enter company code.');
    }

    if (errorList.length < 1) {
      //no error
      await adminIJoin
        .post('/Company', newData)
        .then((res) => {
          let dataToAdd = [...data];
          dataToAdd.push(newData);
          setData(dataToAdd);
          resolve();
          setErrorMessages([]);
          setIserror(false);
        })
        .catch((error) => {
          if (error.response.status === 409) {
            // throw new Error(`${err.config.url} not found`);
            setErrorMessages([
              `${newData.companyCode} company code already exists.`
            ]);
          } else {
            setErrorMessages(['Cannot add data. Server error!']);
          }
          setIserror(true);
          resolve();
        });
    } else {
      setErrorMessages(errorList);
      setIserror(true);
      resolve();
    }
  };

  const handleRowDelete = async (oldData, resolve) => {
    // console.log(oldData);
    await adminIJoin
      .delete(`/Company/${oldData.companyId}`)
      .then((res) => {
        const dataDelete = [...data];
        const index = oldData.tableData.id;
        dataDelete.splice(index, 1);
        setData([...dataDelete]);
        resolve();
      })
      .catch((error) => {
        setErrorMessages(['Delete failed! Server error']);
        setIserror(true);
        resolve();
      });
  };

  const handleSnackbarClose = () => {
    setIserror(false);
  };

  return (
    <React.Fragment>
      <Page className={classes.root} title="Company Management">
        <Container maxWidth={false}>
          <Box>
            {loading ? (
              <Spinner />
            ) : (
              <MaterialTable
                title=""
                columns={columns}
                data={data}
                icons={tableIcons}
                editable={{
                  onRowUpdate: (newData, oldData) =>
                    new Promise((resolve) => {
                      handleRowUpdate(newData, oldData, resolve);
                    }),
                  onRowAdd: (newData) =>
                    new Promise((resolve) => {
                      handleRowAdd(newData, resolve);
                    }),
                  onRowDelete: (oldData) =>
                    new Promise((resolve) => {
                      handleRowDelete(oldData, resolve);
                    })
                }}
                components={{
                  Pagination: (props) => (
                    <TablePagination
                      {...props}
                      ActionsComponent={TablePaginationActions}
                    />
                  )
                }}
              />
            )}
          </Box>
        </Container>
      </Page>
      <AlertMessage
        openSnackbar={iserror}
        handleSnackbarClose={handleSnackbarClose}
        message={errorMessages.map((msg, i) => {
          return <div key={i}>{msg}</div>;
        })}
        severity={'error'}
      />
    </React.Fragment>
  );
};

export default CompanyView;
