import React, { useState, useRef, useEffect } from 'react';
import {
  Box,
  Container,
  makeStyles,
  Card,
  CardContent,
  CardActions,
  Typography
} from '@material-ui/core';
import Page from 'src/components/Page';
import { fetchSessions, updateSession } from 'src/actions/session';
import Toolbar from './Toolbar';
import { useDispatch, useSelector } from 'react-redux';
import Spinner from 'src/components/Spinner';
import AlertMessage from 'src/components/AlertMessage';
import SessionList from './SessionList';
import PerfectScrollbar from 'react-perfect-scrollbar';
import QrCodeView from '../QrCodeView';
import { useReactToPrint } from 'react-to-print';
import adminIJoin from 'src/api/adminIJoin';
import BackdropSpinner from 'src/components/Backdrop';
import userIJoin from 'src/api/userIJoin';

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

const GenQrCodeListView = () => {
  const classes = useStyles();
  const [openSnackbar, setOpenSnackbar] = useState(false);
  const [upLoading, setUpLoading] = React.useState(false);
  const dispatch = useDispatch();
  const session = useSelector((state) => state.session);
  const { loading, errmsg } = useSelector((state) => state.sessionStatus);
  const [qrData, setQrData] = useState({});

  const componentRef = useRef();

  const onSearchSubmit = (sessionId) => {
    console.log(sessionId);
    dispatch(fetchSessions(sessionId));
    setOpenSnackbar(true);
  };

  const handleSnackbarClose = () => {
    setOpenSnackbar(false);
  };

  const onTablePrint = (row) => {
    console.log(row);
    setQrData({ ...row, headerQr: 'Check In' });
    // setOpenQr(true);

    setTimeout(function () {
      handlePrintQr();
    }, 1000);

    setTimeout(function () {
      setQrData({});
    }, 3000);
  };

  const onTableCancel = async (row) => {
    console.log(row);
    handleCancelMobile(row);
  };
  const handleCancelMobile = async (row) => {
    setUpLoading(true);
    await userIJoin
      .post('Sessions/Update', { ...row, isCancel: '1' })
      .then((res) => {
        console.log(res.data);
        handleCancelAdmin(row);
      })
      .catch((err) => {
        console.log(err.message);
        setUpLoading(false);
      });
  };
  const handleCancelAdmin = async (row) => {
    await adminIJoin
      .put(`/Sessions/${row.sessionId}`, {
        ...row,
        isCancel: '1'
      })
      .then((res) => {
        console.log(res.data);
        dispatch(updateSession({ ...row, isCancel: '1' }));
        setUpLoading(false);
      })
      .catch((err) => {
        console.log(err.message);
        setUpLoading(false);
      });
  };

  const onTableUndoCancel = async (row) => {
    console.log(row);
    handleUndoCancelMobile(row);
  };
  const handleUndoCancelMobile = async (row) => {
    setUpLoading(true);
    await userIJoin
      .post('Sessions/Update', { ...row, isCancel: '0' })
      .then((res) => {
        console.log(res.data);
        handleUndoCancelAdmin(row);
      })
      .catch((err) => {
        console.log(err.message);
        setUpLoading(false);
      });
  };
  const handleUndoCancelAdmin = async (row) => {
    await adminIJoin
      .put(`/Sessions/${row.sessionId}`, {
        ...row,
        isCancel: '0'
      })
      .then((res) => {
        console.log(res.data);
        dispatch(updateSession({ ...row, isCancel: '0' }));
        setUpLoading(false);
      })
      .catch((err) => {
        console.log(err.message);
        setUpLoading(false);
      });
  };

  const handlePrintQr = useReactToPrint({
    content: () => componentRef.current
  });

  useEffect(() => {
    dispatch(fetchSessions(''));
    setOpenSnackbar(true);
  }, [dispatch]);

  // console.log(session);

  return (
    <React.Fragment>
      <Page className={classes.root} title="Generate QR Code">
        <Container maxWidth={false}>
          <Toolbar onSearchSubmit={onSearchSubmit} />
          <Box mt={3}>
            {loading ? (
              <Spinner />
            ) : (
              <React.Fragment>
                {session.length > 0 ? (
                  <Card elevation={5}>
                    <CardContent className={classes.content}>
                      <PerfectScrollbar>
                        <SessionList
                          session={session}
                          onTablePrint={onTablePrint}
                          onTableCancel={onTableCancel}
                          onTableUndoCancel={onTableUndoCancel}
                        />
                      </PerfectScrollbar>
                    </CardContent>
                    <CardActions className={classes.actions}></CardActions>
                  </Card>
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
        {qrData.headerQr && <QrCodeView ref={componentRef} qrData={qrData} />}
        {upLoading && <BackdropSpinner isLoading={upLoading} />}
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

export default GenQrCodeListView;
