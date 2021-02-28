import React, { useState, useEffect } from 'react';
import axios from 'axios';
import PropTypes from 'prop-types';
import clsx from 'clsx';
import {
  Box,
  Button,
  Card,
  CardContent,
  CardActions,
  // CardHeader,
  // Divider,
  FormControlLabel,
  Grid,
  makeStyles,
  FormControl,
  FormLabel,
  RadioGroup,
  Radio,
  LinearProgress,
  Typography,
  Stepper,
  Step,
  StepLabel,
  StepContent,
  Paper,
  InputLabel,
  MenuItem,
  Select,
  FormHelperText
} from '@material-ui/core';

// import { Download as DownloadIcon } from 'react-feather';
import adminIJoin from 'src/api/adminIJoin';
import { DropzoneArea } from 'material-ui-dropzone';
import DescriptionIcon from '@material-ui/icons/Description';
import BackdropSpinner from 'src/components/Backdrop';
import KlcInvalidList from './KlcInvalidList';
import AlertMessage from 'src/components/AlertMessage';
import PerfectScrollbar from 'react-perfect-scrollbar';
import KlcFail from './KlcFail';
import KlcTotal from './KlcTotal';
import KlcPass from './KlcPass';
import KlcImportHisList from './KlcImportHisList';
import KlcCheckList from './KlcCheckList';
// import { useDispatch } from 'react-redux';
// import {
//   fetchSessions,
//   fetchToday,
//   fetchNextSevenDay
// } from 'src/actions/session';

const useStyles = makeStyles((theme) => ({
  root: {},
  content: {
    padding: 0
  },
  actions: {
    justifyContent: 'flex-end',
    padding: 0
  },
  item: {
    display: 'flex',
    flexDirection: 'column'
  },
  extendedIcon: {
    marginRight: theme.spacing(1)
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
  },
  formControl: {
    marginTop: theme.spacing(1),
    minWidth: 250
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

const KlcFileUpload = ({ className, ...rest }) => {
  const classes = useStyles();
  // const dispatch = useDispatch();
  const [filesKlc, setFilesKlc] = useState(null);
  const [upLoading, setUpLoading] = React.useState(false);
  const [upLoadValue, setUpLoadValue] = React.useState(0);
  const [importType, setImportType] = React.useState(
    'Upload session and participants'
  );
  const [activeStep, setActiveStep] = React.useState(0);
  const [uploadFileKlcResponse, setUploadFileKlcResponse] = React.useState({
    success: false,
    message: '',
    dataInvalid: []
  });
  const [importResponse, setImportResponse] = React.useState({
    success: false,
    message: '',
    dataList: []
  });
  const [resMsg, setResMsg] = React.useState('');
  const [openSnackbar, setOpenSnackbar] = React.useState(false);

  const [dataCompany, setDataCompany] = useState([]); //table data
  const [companyId, setCompanyId] = React.useState('');
  const [companyCode, setCompanyCode] = React.useState('');

  const [iserror, setIserror] = useState(false);

  const handleNext = () => {
    setActiveStep((prevActiveStep) => prevActiveStep + 1);
  };

  const handleBack = () => {
    setActiveStep((prevActiveStep) => prevActiveStep - 1);
  };

  const handleReset = () => {
    setActiveStep(0);
  };

  const handleRadioChange = (event) => {
    setImportType(event.target.value);
  };

  const handleKlcChange = (files) => {
    setFilesKlc(files);
  };

  const handlePreviewIcon = (fileObject, classes) => {
    const iconProps = {
      className: classes.image
    };

    return <DescriptionIcon {...iconProps} />;
  };

  const handleUpload = () => {
    setUpLoadValue(0);
    setUpLoading(true);
    const data = new FormData();
    for (var x = 0; x < filesKlc.length; x++) {
      data.append('file', filesKlc[x]);
    }
    axios
      .post(
        `${process.env.REACT_APP_ADMIN_IJOIN_BASE_URL}/UploadFile/UploadFileKlc`,
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
        setUploadFileKlcResponse(res.data);
        setUpLoading(false);
        setFilesKlc(null);
        handleNext();
      })
      .catch((err) => {
        // console.log(err.message);
        setResMsg(err.message);
        setUpLoading(false);
        setOpenSnackbar(true);
      });
  };

  const handleImport = async () => {
    // console.log(companyId);
    // console.log(companyCode);

    let errorList = [];
    if (companyId === '') {
      errorList.push('Organized By is required.');
    }

    if (errorList.length < 1) {
      setUpLoading(true);
      console.log(uploadFileKlcResponse.fileUploadId);
      await adminIJoin
        .post('/UploadFile/ImportKlcData', {
          id: uploadFileKlcResponse.fileUploadId,
          importType,
          companyId,
          companyCode
        })
        .then((res) => {
          console.log(res.data);
          setImportResponse(res.data);
          setUpLoading(false);

          if (res.data.success) {
            handleNext();
            // dispatch(fetchSessions(''));
            // dispatch(fetchToday(''));
            // dispatch(fetchNextSevenDay(''));
          } else {
            setResMsg(res.data.message);
            setOpenSnackbar(true);
          }
        })
        .catch((err) => {
          setUpLoading(false);
          setResMsg(err.message);
          setOpenSnackbar(true);
        });
    } else {
      setIserror(true);
    }
  };

  const handleSnackbarClose = () => {
    setOpenSnackbar(false);
  };

  const handleGetCompany = async () => {
    await adminIJoin
      .get('/UploadFile/GetUserCompany')
      .then((res) => {
        setDataCompany(res.data);

        setCompanyId(res.data[0].companyId);
        setCompanyCode(res.data[0].companyCode);
      })
      .catch((err) => {
        console.log(err.message);
      });
  };

  useEffect(() => {
    handleGetCompany();
  }, []);

  return (
    <form className={clsx(classes.root, className)} {...rest}>
      <Card>
        <Stepper activeStep={activeStep} orientation="vertical">
          <Step key={0}>
            <StepLabel>{'Upload File Data From KLC'}</StepLabel>
            <StepContent>
              <Grid container spacing={6} wrap="wrap">
                <Grid className={classes.item} item md={8} sm={6} xs={12}>
                  <DropzoneArea
                    filesLimit={1}
                    onChange={handleKlcChange.bind(this)}
                    dropzoneText={
                      'Drag and drop a KLC excel file here or click to choose'
                    }
                    showFileNames={true}
                    acceptedFiles={[
                      'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
                      'application/vnd.ms-excel'
                    ]}
                    getPreviewIcon={handlePreviewIcon}
                  />
                </Grid>
                <Grid className={classes.item} item md={4} sm={6} xs={12}>
                  <FormControl component="fieldset">
                    <FormLabel component="legend">Import Type</FormLabel>
                    <RadioGroup value={importType} onChange={handleRadioChange}>
                      <FormControlLabel
                        value="Upload session and participants"
                        control={<Radio color="primary" />}
                        label="Upload session and participants."
                      />
                      <FormControlLabel
                        value="Add new participants of existing session"
                        control={<Radio color="primary" />}
                        label="Add new participants of existing session."
                      />
                    </RadioGroup>
                  </FormControl>
                </Grid>
              </Grid>
              <div className={classes.actionsContainer}>
                <div>
                  <Button
                    color="primary"
                    disabled={
                      filesKlc === null || filesKlc.length === 0 ? true : false
                    }
                    variant="contained"
                    onClick={handleUpload}
                    className={classes.button}
                  >
                    UPLOAD DATA
                  </Button>
                  {upLoading && (
                    <Grid className={classes.item} item md={12} sm={12} xs={12}>
                      <LinearProgressWithLabel
                        value={upLoadValue}
                        color="secondary"
                      />
                      <BackdropSpinner isLoading={upLoading} />
                      {/* <Spinner /> */}
                    </Grid>
                  )}
                </div>
              </div>
            </StepContent>
          </Step>
          <Step key={1}>
            <StepLabel>{'Check Result Format'}</StepLabel>
            <StepContent>
              <Box mb={3}>
                <FormControl
                  variant="outlined"
                  className={classes.formControl}
                  error={iserror}
                >
                  <InputLabel id="organizedByLabel">Organized By</InputLabel>
                  <Select
                    labelId="organizedByLabel"
                    value={companyId}
                    onChange={(event) => {
                      // console.log(event);
                      setIserror(false);
                      setCompanyId(event.target.value);

                      const code = dataCompany.find(
                        (x) => x.companyId === event.target.value
                      ).companyCode;
                      setCompanyCode(code);
                    }}
                    label="Organized By"
                    disabled={upLoading}
                  >
                    {dataCompany.map((ddl) => (
                      <MenuItem key={ddl.companyId} value={ddl.companyId}>
                        {ddl.companyCode}
                      </MenuItem>
                    ))}
                  </Select>
                  <FormHelperText>Organized By is required.</FormHelperText>
                </FormControl>
              </Box>

              <Grid container spacing={3}>
                <Grid item lg={3} sm={6} xl={3} xs={12}>
                  <KlcTotal number={uploadFileKlcResponse.totalNo} />
                </Grid>
                <Grid item lg={3} sm={6} xl={3} xs={12}>
                  <KlcPass number={uploadFileKlcResponse.validNo} />
                </Grid>
                <Grid item lg={3} sm={6} xl={3} xs={12}>
                  <KlcFail number={uploadFileKlcResponse.invalidNo} />
                </Grid>

                <Grid item lg={12} sm={12} xl={12} xs={12}>
                  {uploadFileKlcResponse.success &&
                    uploadFileKlcResponse.dataCheck.length > 0 && (
                      <Card elevation={5}>
                        <CardContent className={classes.content}>
                          <PerfectScrollbar>
                            <KlcCheckList klcResponse={uploadFileKlcResponse} />
                          </PerfectScrollbar>
                        </CardContent>
                        <CardActions className={classes.actions}></CardActions>
                      </Card>
                    )}
                </Grid>

                <Grid item lg={12} sm={12} xl={12} xs={12}>
                  {uploadFileKlcResponse.success &&
                    uploadFileKlcResponse.dataInvalid.length > 0 && (
                      <Card elevation={5}>
                        <CardContent className={classes.content}>
                          <PerfectScrollbar>
                            <KlcInvalidList
                              klcResponse={uploadFileKlcResponse}
                            />
                          </PerfectScrollbar>
                        </CardContent>
                        <CardActions className={classes.actions}></CardActions>
                      </Card>
                    )}
                </Grid>
              </Grid>

              <div className={classes.actionsContainer}>
                <div>
                  <Button onClick={handleBack} className={classes.button}>
                    CANCEL
                  </Button>
                  <Button
                    variant="contained"
                    color="primary"
                    onClick={handleImport}
                    className={classes.button}
                    disabled={
                      uploadFileKlcResponse.success &&
                      uploadFileKlcResponse.dataInvalid.length > 0
                    }
                  >
                    IMPORT
                  </Button>
                  {upLoading && (
                    <Grid className={classes.item} item md={12} sm={12} xs={12}>
                      <BackdropSpinner isLoading={upLoading} />
                    </Grid>
                  )}
                </div>
              </div>
            </StepContent>
          </Step>
          <Step key={2}>
            <StepLabel>{'Import History'}</StepLabel>
            <StepContent>
              <Card elevation={5}>
                <CardContent className={classes.content}>
                  <PerfectScrollbar>
                    <KlcImportHisList importResponse={importResponse} />
                  </PerfectScrollbar>
                </CardContent>
                <CardActions className={classes.actions}></CardActions>
              </Card>

              <div className={classes.actionsContainer}>
                <div>
                  <Button
                    variant="contained"
                    color="primary"
                    onClick={handleNext}
                    className={classes.button}
                  >
                    DONE
                  </Button>
                </div>
              </div>
            </StepContent>
          </Step>
        </Stepper>
        {activeStep === 3 && (
          <Paper square elevation={1} className={classes.resetContainer}>
            <Typography>
              All steps import klc data completed - you&apos;re finished
            </Typography>
            <Button
              variant="contained"
              color="primary"
              onClick={handleReset}
              className={classes.button}
            >
              add new klc data
            </Button>
          </Paper>
        )}
      </Card>

      <AlertMessage
        openSnackbar={openSnackbar}
        handleSnackbarClose={handleSnackbarClose}
        message={resMsg}
        severity={'error'}
      />
    </form>
  );
};

KlcFileUpload.propTypes = {
  className: PropTypes.string
};

export default KlcFileUpload;
