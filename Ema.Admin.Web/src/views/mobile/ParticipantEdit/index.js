import React, { useEffect } from 'react';
import { NavLink as RouterLink } from 'react-router-dom';
import * as Yup from 'yup';
import { Formik } from 'formik';
import {
  Box,
  Container,
  TextField,
  makeStyles,
  Fab,
  Typography,
  InputLabel,
  MenuItem,
  FormControl,
  Select
} from '@material-ui/core';
import { ArrowBackIos, Save } from '@material-ui/icons';
import Page from 'src/components/Page';
import adminIJoin from 'src/api/adminIJoin';
import userIJoin from 'src/api/userIJoin';
import BackdropSpinner from 'src/components/Backdrop';
import AlertMessage from 'src/components/AlertMessage';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { fetchParticipant } from 'src/actions/participant';

const useStyles = makeStyles((theme) => ({
  root: {
    backgroundColor: theme.palette.background.dark,
    height: '100%',
    paddingBottom: theme.spacing(3),
    paddingTop: theme.spacing(3),
    position: 'relative',
    minHeight: 500
  },
  lable: {
    color: theme.palette.text.primary
  },
  add: {
    position: 'absolute',
    bottom: theme.spacing(2),
    right: theme.spacing(2)
  },
  back: {
    position: 'absolute',
    bottom: theme.spacing(2),
    left: theme.spacing(2)
  },
  formControl: {
    marginTop: theme.spacing(1),
    minWidth: 250
  }
}));

const statusValueAll = [
  { name: 'Enrolled', value: 'Enrolled' },
  { name: 'Waitlist', value: 'Waitlist' },
  { name: 'Cancelled', value: 'Cancelled' },
  { name: 'Deleted', value: 'Deleted' }
];

const statusValueCheckIn = [
  { name: 'Check-In', value: 'Check-In' },
  { name: 'Enrolled', value: 'Enrolled' },
  { name: 'Waitlist', value: 'Waitlist' },
  { name: 'Cancelled', value: 'Cancelled' },
  { name: 'Deleted', value: 'Deleted' }
];
const statusValueCheckOut = [{ name: 'Check-Out', value: 'Check-Out' }];

const ParticipantEdit = () => {
  const classes = useStyles();
  const [isDisableSave, setDisableSave] = React.useState(false);
  const [upLoading, setUpLoading] = React.useState(false);
  const [resMsg, setResMsg] = React.useState('');
  const [openSnackbar, setOpenSnackbar] = React.useState(false);
  const [severity, setSeverity] = React.useState('');
  const navigate = useNavigate();
  const session = useSelector((state) => state.selected.session);
  const sessionUser = useSelector((state) => state.selected.sessionUser);

  const dispatch = useDispatch();

  let statusValue = [];

  if (sessionUser.registrationStatus === 'Enrolled') {
    statusValue = statusValueCheckIn;
  } else if (sessionUser.registrationStatus === 'Check-In') {
    statusValue = statusValueCheckOut;
  } else {
    statusValue = statusValueAll;
  }

  let registrationStatusInit = '';

  if (sessionUser.registrationStatus === 'Enrolled') {
    registrationStatusInit = sessionUser.registrationStatus;
  } else if (sessionUser.registrationStatus === 'Check-In') {
    registrationStatusInit = 'Check-Out';
  } else {
    registrationStatusInit = sessionUser.registrationStatus;
  }

  useEffect(() => {
    if (!session.sessionId) navigate('/app/today');
    if (!sessionUser.userId) navigate('/app/participant');
  });

  const handleSnackbarClose = () => {
    setOpenSnackbar(false);
  };

  const handleFetchParticipant = () => {
    dispatch(
      fetchParticipant({
        sessionId: session.sessionId,
        userId: ''
      })
    );
  };

  const handleUserMobile = async (values) => {
    if (values.registrationStatus === 'Enrolled') {
      await userIJoin
        .post('Participant/Add', {
          sessionId: sessionUser.sessionId,
          userId: sessionUser.userId
        })
        .then((res) => {
          console.log(res.data);
          handleAdminWeb(values);
        })
        .catch((err) => {
          console.log(err.message);
          setResMsg(
            `${values.userId} Mobile Participant Add Error. ${err.message}`
          );
          setSeverity('error');
          setUpLoading(false);
          setOpenSnackbar(true);
        });
    } else {
      if (values.registrationStatus === 'Check-In') {
        await userIJoin
          .post(`UserRegistration/CheckIn`, {
            sessionId: sessionUser.sessionId,
            userId: sessionUser.userId,
            checkInBy: 'Mobile Admin'
          })
          .then((res) => {
            console.log(res.data);
            handleAdminWeb(values);
          })
          .catch((err) => {
            console.log(err.message);
            setResMsg(
              `${values.userId} Mobile Participant Check-In Error. ${err.message}`
            );
            setSeverity('error');
            setUpLoading(false);
            setOpenSnackbar(true);
          });
      } else if (values.registrationStatus === 'Check-Out') {
        await userIJoin
          .post(`UserRegistration/CheckOut`, {
            sessionId: sessionUser.sessionId,
            userId: sessionUser.userId,
            checkOutBy: 'Mobile Admin'
          })
          .then((res) => {
            console.log(res.data);
            handleAdminWeb(values);
          })
          .catch((err) => {
            console.log(err.message);
            setResMsg(
              `${values.userId} Mobile Participant Check-Out Error. ${err.message}`
            );
            setSeverity('error');
            setUpLoading(false);
            setOpenSnackbar(true);
          });
      } else {
        await userIJoin
          .post('Participant/Delete', {
            sessionId: sessionUser.sessionId,
            userId: sessionUser.userId
          })
          .then((res) => {
            console.log(res.data);
            handleAdminWeb(values);
          })
          .catch((err) => {
            console.log(err.message);
            setResMsg(
              `${values.userId} Mobile Participant Delete Error. ${err.message}`
            );
            setSeverity('error');
            setUpLoading(false);
            setOpenSnackbar(true);
          });
      }
    }
  };

  const handleAdminWeb = async (values) => {
    if (values.registrationStatus === 'Deleted') {
      await adminIJoin
        .post('/Participant/Delete', {
          sessionId: sessionUser.sessionId,
          userId: sessionUser.userId
        })
        .then((res) => {
          setDisableSave(true);
          console.log(res.data);
          setResMsg(`${values.userId} Delete success.`);
          setSeverity('success');
          setUpLoading(false);
          setOpenSnackbar(true);
          handleFetchParticipant();
          setTimeout(function () {
            navigate('/app/participant');
          }, 4000);
        })
        .catch((err) => {
          setResMsg(`${values.userId} Delete Error. ${err.message}`);
          setSeverity('error');
          setUpLoading(false);
          setOpenSnackbar(true);
        });
    } else {
      await adminIJoin
        .put(`/Participant/${sessionUser.sessionId}`, {
          ...sessionUser,
          RegistrationStatus: values.registrationStatus,
          UpdateBy: 'Mobile Admin'
        })
        .then((res) => {
          console.log(res.data);
          setResMsg(`${values.userId} Edit status success.`);
          setSeverity('success');
          setUpLoading(false);
          setOpenSnackbar(true);
          handleFetchParticipant();
        })
        .catch((err) => {
          setResMsg(`${values.userId} Edit Error. ${err.message}`);
          setSeverity('error');
          setUpLoading(false);
          setOpenSnackbar(true);
        });
    }
  };

  return (
    <Page className={classes.root} title="Check In">
      <Box
      // display="flex"
      // flexDirection="column"
      // height="100%"
      // justifyContent="center"
      >
        <Container maxWidth="sm">
          <Formik
            initialValues={{
              userId: sessionUser.userId,
              userName: 'รัฐวิชญ์',
              registrationStatus: registrationStatusInit
            }}
            validationSchema={Yup.object().shape({
              userId: Yup.string().max(9).required('Emp Id is required'),
              userName: Yup.string().max(255).required('Emp Name is required'),
              registrationStatus: Yup.string()
                .max(255)
                .required('Status is required')
            })}
            onSubmit={async (values, { resetForm }) => {
              setUpLoading(true);

              handleUserMobile(values);
            }}
          >
            {({
              errors,
              handleBlur,
              handleChange,
              handleSubmit,
              isSubmitting,
              touched,
              values
            }) => (
              <form onSubmit={handleSubmit}>
                <Box mb={3}>
                  <Typography color="textPrimary" gutterBottom variant="h3">
                    {`Edit Participant`}
                  </Typography>
                  <Typography color="textSecondary" variant="h4">
                    {`${session.courseName} (${session.courseId})`}
                  </Typography>
                  <Typography color="textSecondary" gutterBottom variant="h4">
                    {`${session.sessionName} (${session.sessionId})`}
                  </Typography>
                </Box>
                <TextField
                  error={Boolean(touched.userId && errors.userId)}
                  fullWidth
                  helperText={touched.userId && errors.userId}
                  label="Emp ID"
                  margin="normal"
                  name="userId"
                  onBlur={handleBlur}
                  onChange={handleChange}
                  value={values.userId}
                  variant="outlined"
                  inputProps={{ maxLength: 8 }}
                  disabled
                />
                <TextField
                  error={Boolean(touched.userName && errors.userName)}
                  fullWidth
                  helperText={touched.userName && errors.userName}
                  label="Emp Name"
                  margin="normal"
                  name="userName"
                  onBlur={handleBlur}
                  onChange={handleChange}
                  value={values.userName}
                  variant="outlined"
                  disabled
                />

                <FormControl variant="outlined" className={classes.formControl}>
                  <InputLabel id="registrationStatusLabel">Status</InputLabel>
                  <Select
                    labelId="registrationStatusLabel"
                    value={values.registrationStatus}
                    onBlur={handleBlur}
                    onChange={handleChange}
                    label="Age"
                    name="registrationStatus"
                    disabled={isDisableSave}
                  >
                    {statusValue.map((ddl) => (
                      <MenuItem key={ddl.value} value={ddl.value}>
                        {ddl.name}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
                <Box>
                  <Fab
                    color="secondary"
                    className={classes.back}
                    component={RouterLink}
                    to={'/app/participant'}
                  >
                    <ArrowBackIos />
                  </Fab>

                  <Fab
                    color="primary"
                    className={classes.add}
                    type="submit"
                    disabled={isSubmitting || isDisableSave}
                  >
                    <Save />
                  </Fab>
                </Box>
              </form>
            )}
          </Formik>
        </Container>
      </Box>
      {upLoading && <BackdropSpinner isLoading={upLoading} />}
      <AlertMessage
        openSnackbar={openSnackbar}
        handleSnackbarClose={handleSnackbarClose}
        message={resMsg}
        severity={severity}
      />
    </Page>
  );
};

export default ParticipantEdit;
