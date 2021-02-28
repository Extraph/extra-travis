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
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
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

const CreditHourView = () => {
  const classes = useStyles();
  const [upLoading, setUpLoading] = React.useState(false);
  const [resMsg, setResMsg] = React.useState('');
  const [openSnackbar, setOpenSnackbar] = React.useState(false);
  const [severity, setSeverity] = React.useState('');
  const navigate = useNavigate();

  const session = useSelector((state) => state.selected.session);
  const editFrom = useSelector((state) => state.selected.editFrom);

  useEffect(() => {
    if (!session.sessionId) navigate('/app/today');
  });

  const handleSnackbarClose = () => {
    setOpenSnackbar(false);
  };

  const handleCreditHourMobile = async (values) => {
    setUpLoading(true);
    await userIJoin
      .post('Sessions/Update', {
        ...session,
        updateBy: 'Mobile Admin',
        courseCreditHoursInit: values.courseCreditHoursInit,
        courseCreditHours: values.courseCreditHours,
        passingCriteriaException: (
          parseFloat(values.passingCriteriaException) / 100
        ).toString()
      })
      .then((res) => {
        console.log(res.data);
        handleCreditHourAdmin(values);
      })
      .catch((err) => {
        console.log(err.message);
        setResMsg(
          `${session.sessionId} Mobile edit credit hour error. ${err.message}`
        );
        setSeverity('error');
        setUpLoading(false);
        setOpenSnackbar(true);
      });
  };

  const handleCreditHourAdmin = async (values) => {
    setUpLoading(true);
    await adminIJoin
      .put(`/Sessions/${session.sessionId}`, {
        ...session,
        updateBy: 'Mobile Admin',
        courseCreditHoursInit: values.courseCreditHoursInit,
        courseCreditHours: values.courseCreditHours,
        passingCriteriaException: (
          parseFloat(values.passingCriteriaException) / 100
        ).toString()
      })
      .then((res) => {
        console.log(res.data);
        setResMsg(`${session.sessionId} Edit credit hour success.`);
        setSeverity('success');
        setUpLoading(false);
        setOpenSnackbar(true);
      })
      .catch((err) => {
        setResMsg(`${session.sessionId} Edit Error. ${err.message}`);
        setSeverity('error');
        setUpLoading(false);
        setOpenSnackbar(true);
      });
  };

  return (
    <Page className={classes.root} title="Edit Credit Hour">
      <Box
      // display="flex"
      // flexDirection="column"
      // height="100%"
      // justifyContent="center"
      >
        <Container maxWidth="sm">
          <Formik
            initialValues={{
              courseCreditHoursInit: session.courseCreditHoursInit,
              courseCreditHours: session.courseCreditHours,
              passingCriteriaException:
                parseFloat(session.passingCriteriaException) * 100
            }}
            validationSchema={Yup.object().shape({
              // courseCreditHoursInit: Yup.number()
              //   .integer()
              //   .required('Course Credit Hours is required'),
              // courseCreditHours: Yup.number()
              //   .integer()
              //   .required('Course Credit Hours is required'),
              // passingCriteriaException: Yup.number()
              //   .integer()
              //   .required('Passing Criteria is required')
            })}
            onSubmit={async (values, { resetForm }) => {
              // console.log({
              //   SegmentId: segment.id,
              //   UserId: values.userId
              // });
              handleCreditHourMobile(values);
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
                    {`Edit Credit hours`}
                  </Typography>
                  <Typography color="textSecondary" variant="h4">
                    {`${session.courseName} (${session.courseId})`}
                  </Typography>
                  <Typography color="textSecondary" gutterBottom variant="h4">
                    {`${session.sessionName} (${session.sessionId})`}
                  </Typography>
                </Box>
                <TextField
                  error={Boolean(
                    touched.courseCreditHoursInit &&
                      errors.courseCreditHoursInit
                  )}
                  fullWidth
                  helperText={
                    touched.courseCreditHoursInit &&
                    errors.courseCreditHoursInit
                  }
                  label="ชั่วโมงอบรมที่บันทึกจริง"
                  margin="normal"
                  name="courseCreditHoursInit"
                  onBlur={handleBlur}
                  onChange={handleChange}
                  value={values.courseCreditHoursInit}
                  variant="outlined"
                  inputProps={{ maxLength: 3 }}
                />
                <TextField
                  error={Boolean(
                    touched.passingCriteriaException &&
                      errors.passingCriteriaException
                  )}
                  fullWidth
                  helperText={
                    touched.passingCriteriaException &&
                    errors.passingCriteriaException
                  }
                  label="แก้ไขเปอร์เซ็นเพื่อคำนวนใหม่"
                  margin="normal"
                  name="passingCriteriaException"
                  onBlur={handleBlur}
                  onChange={handleChange}
                  value={values.passingCriteriaException}
                  variant="outlined"
                  inputProps={{ maxLength: 3 }}
                />

                <TextField
                  error={Boolean(
                    touched.courseCreditHours && errors.courseCreditHours
                  )}
                  fullWidth
                  helperText={
                    touched.courseCreditHours && errors.courseCreditHours
                  }
                  label="แก้ไขชั่วโมงอบรมเพื่อคำนวนใหม่"
                  margin="normal"
                  name="courseCreditHours"
                  onBlur={handleBlur}
                  onChange={handleChange}
                  value={values.courseCreditHours}
                  variant="outlined"
                  inputProps={{ maxLength: 3 }}
                />

                <Box mt={3} mb={3}>
                  <Typography color="textPrimary" variant="h4">
                    {`ชัวโมงที่ต้องอบรมอย่างน้อย ${
                      (values.courseCreditHours *
                        parseFloat(
                          values.passingCriteriaException
                            ? values.passingCriteriaException
                            : 0
                        )) /
                      100
                    } hrs`}
                  </Typography>
                </Box>

                <Box>
                  <Fab
                    color="secondary"
                    className={classes.back}
                    component={RouterLink}
                    to={editFrom === 'today' ? '/app/today' : '/app/next'}
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

export default CreditHourView;
