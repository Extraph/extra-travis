import React, { useState, useRef, useEffect } from 'react';
import {
  Box,
  Container,
  makeStyles,
  Card,
  CardContent,
  CardActions,
  Typography,
  Dialog,
  DialogActions,
  DialogContent,
  // DialogTitle,
  Button,
  LinearProgress
} from '@material-ui/core';
import Page from 'src/components/Page';
import { fetchSessions, updateSession } from 'src/actions/session';
// import Toolbar from './Toolbar';
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
import { DropzoneArea } from 'material-ui-dropzone';
// import DescriptionIcon from '@material-ui/icons/Description';
import axios from 'axios';

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
  },
  button: {
    marginTop: theme.spacing(1),
    marginRight: theme.spacing(1)
  },
  actionsContainer: {
    marginBottom: theme.spacing(2)
  },
  resetContainer: {
    padding: theme.spacing(3)
  }
}));

function LinearProgressWithLabel(props) {
  return (
    <Box display="flex" alignItems="center">
      <Box width="100%" mr={1}>
        <LinearProgress variant="determinate" {...props} />
      </Box>
      <Box minWidth={35}>
        <Typography variant="body2" color="textSecondary">{`${Math.round(
          props.value
        )}%`}</Typography>
      </Box>
    </Box>
  );
}

const GenQrCodeListView = () => {
  const classes = useStyles();
  const [openSnackbar, setOpenSnackbar] = useState(false);
  const [upLoading, setUpLoading] = React.useState(false);
  const dispatch = useDispatch();
  const session = useSelector((state) => state.session);
  const { loading, errmsg } = useSelector((state) => state.sessionStatus);
  const [qrData, setQrData] = useState({});

  const componentRef = useRef();

  // const onSearchSubmit = (sessionId) => {
  //   console.log(sessionId);
  //   dispatch(fetchSessions(sessionId));
  //   setOpenSnackbar(true);
  // };

  const [openCover, setOpenCover] = React.useState(false);
  const [rowCoverData, setRowCoverData] = useState({});
  const [filesCover, setFilesCover] = useState(null);
  const [upLoadValue, setUpLoadValue] = React.useState(0);
  const [uploadFileCoverResponse, setUploadFileCoverResponse] = React.useState({
    success: false,
    fileUploadId: '',
    coverPhotoUrl: '',
    coverPhotoName: ''
  });

  const handleCoverChange = (files) => {
    setFilesCover(files);
  };

  // const handlePreviewIcon = (fileObject, classes) => {
  //   const iconProps = {
  //     className: classes.image
  //   };

  //   return <DescriptionIcon {...iconProps} />;
  // };
  const handleUpload = () => {
    console.log('upload???');
    setUpLoadValue(0);
    setUpLoading(true);
    const data = new FormData();
    for (var x = 0; x < filesCover.length; x++) {
      data.append('file', filesCover[x]);
    }
    axios
      .post(
        `${process.env.REACT_APP_ADMIN_IJOIN_BASE_URL}/UploadFile/UploadCoverPhoto`,
        data,
        {
          headers: { Authorization: `Bearer ${localStorage.getItem('token')}` },
          onUploadProgress: (ProgressEvent) => {
            setUpLoadValue((ProgressEvent.loaded / ProgressEvent.total) * 100);
          }
        }
      )
      .then((res) => {
        console.log(res.data);
        setUploadFileCoverResponse(res.data);
        setUpLoading(false);
        setFilesCover(null);
      })
      .catch((err) => {
        console.log(err.message);
        // setResMsg(err.message);
        setUpLoading(false);
        // setOpenSnackbar(true);
      });
  };

  const handleCoverOpen = () => {
    setOpenCover(true);
  };
  const handleCoverClose = () => {
    setOpenCover(false);
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
    }, 5000);
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

  const onTableCoverPhoto = async (row) => {
    console.log(row);
    setRowCoverData(row);
    handleCoverOpen(true);
  };

  const handleCoverPhotoMobile = async () => {
    setUpLoading(true);
    await userIJoin
      .post('Sessions/Update', {
        ...rowCoverData,
        coverPhotoUrl: uploadFileCoverResponse.coverPhotoUrl,
        coverPhotoName: uploadFileCoverResponse.coverPhotoName
      })
      .then((res) => {
        console.log(res.data);
        handleCoverPhotoAdmin(rowCoverData);
      })
      .catch((err) => {
        console.log(err.message);
        setUpLoading(false);
      });
  };
  const handleCoverPhotoAdmin = async (row) => {
    await adminIJoin
      .put(`/Sessions/${row.sessionId}`, {
        ...row,
        coverPhotoUrl: uploadFileCoverResponse.coverPhotoUrl,
        coverPhotoName: uploadFileCoverResponse.coverPhotoName
      })
      .then((res) => {
        console.log(res.data);
        dispatch(
          updateSession({
            ...row,
            coverPhotoUrl: uploadFileCoverResponse.coverPhotoUrl,
            coverPhotoName: uploadFileCoverResponse.coverPhotoName
          })
        );
        setUpLoading(false);
        setOpenCover(false);
        setUploadFileCoverResponse({
          success: false,
          fileUploadId: '',
          coverPhotoUrl: '',
          coverPhotoName: ''
        });
      })
      .catch((err) => {
        console.log(err.message);
        setUpLoading(false);
        setUploadFileCoverResponse({
          success: false,
          fileUploadId: '',
          coverPhotoUrl: '',
          coverPhotoName: ''
        });
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
          {/* <Toolbar onSearchSubmit={onSearchSubmit} /> */}
          <Box>
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
                          onTableCoverPhoto={onTableCoverPhoto}
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
        <Dialog onClose={handleCoverClose} open={openCover}>
          <DialogContent>
            <DropzoneArea
              filesLimit={1}
              onChange={handleCoverChange.bind(this)}
              dropzoneText={'Drag and drop a Cover Photo for Session.'}
              // showFileNames={true}
              showPreviews={true}
              acceptedFiles={['image/jpeg', 'image/png', 'image/bmp']}
              // getPreviewIcon={handlePreviewIcon}
            />
            <div className={classes.actionsContainer}>
              <div>
                <Button
                  color="primary"
                  disabled={
                    filesCover === null || filesCover.length === 0
                      ? true
                      : false
                  }
                  variant="contained"
                  onClick={handleUpload}
                  className={classes.button}
                >
                  UPLOAD
                </Button>
                {upLoading && (
                  <>
                    <LinearProgressWithLabel
                      value={upLoadValue}
                      color="secondary"
                    />
                    <BackdropSpinner isLoading={upLoading} />
                  </>
                )}
              </div>
            </div>
          </DialogContent>
          <DialogActions>
            <Button
              disabled={uploadFileCoverResponse.awsImageUrl === ''}
              autoFocus
              onClick={handleCoverPhotoMobile}
              color="primary"
            >
              SAVE
            </Button>
          </DialogActions>
        </Dialog>
      </Page>
    </React.Fragment>
  );
};

export default GenQrCodeListView;
