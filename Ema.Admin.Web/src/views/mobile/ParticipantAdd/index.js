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
  Typography
} from '@material-ui/core';
import { ArrowBackIos, Save } from '@material-ui/icons';
import Page from 'src/components/Page';
import adminIJoin from 'src/api/adminIJoin';
import BackdropSpinner from 'src/components/Backdrop';
import AlertMessage from 'src/components/AlertMessage';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { fetchParticipant } from 'src/actions/participant';
import userIJoin from 'src/api/userIJoin';

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
  }
}));

const ParticipantAdd = () => {
  const classes = useStyles();
  const [upLoading, setUpLoading] = React.useState(false);
  const [resMsg, setResMsg] = React.useState('');
  const [openSnackbar, setOpenSnackbar] = React.useState(false);
  const [severity, setSeverity] = React.useState('');
  const navigate = useNavigate();
  const session = useSelector((state) => state.selected.session);

  const dispatch = useDispatch();

  useEffect(() => {
    if (!session.sessionId) navigate('/app/today');
  });

  const handleSnackbarClose = () => {
    setOpenSnackbar(false);
  };

  const handleUserMobile = async (values) => {
    await userIJoin
      .post('Participant/Add', {
        sessionId: session.sessionId,
        userId: values.userId
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
  };

  const handleAdminWeb = async (values) => {
    await adminIJoin
      .post('Participant/Add', {
        sessionId: session.sessionId,
        userId: values.userId,
        registrationStatus: 'Enrolled'
      })
      .then((res) => {
        console.log(res.data);
        setResMsg(`${values.userId} Add user success.`);
        setSeverity('success');
        setUpLoading(false);
        setOpenSnackbar(true);
        dispatch(
          fetchParticipant({
            sessionId: session.sessionId,
            userId: ''
          })
        );
      })
      .catch((err) => {
        setResMsg(err.message);
        if (err.response.status === 409) {
          // throw new Error(`${err.config.url} not found`);
          setResMsg(`${values.userId} This user already exists.`);
        }
        setSeverity('error');
        setUpLoading(false);
        setOpenSnackbar(true);
      });
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
              userId: ''
            }}
            validationSchema={Yup.object().shape({
              userId: Yup.string().max(9).required('Emp Id is required')
            })}
            onSubmit={async (values, { resetForm }) => {
              setUpLoading(true);
              handleUserMobile(values);

              resetForm({ values: '' });
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
                    {`Add Participant`}
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
                  inputProps={{ maxLength: 9 }}
                />
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
                    disabled={isSubmitting}
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

export default ParticipantAdd;
