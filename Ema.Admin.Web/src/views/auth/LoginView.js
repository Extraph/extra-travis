import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import * as Yup from 'yup';
import { Formik } from 'formik';
import {
  Box,
  Button,
  Container,
  TextField,
  Typography,
  makeStyles
} from '@material-ui/core';
import Page from 'src/components/Page';
import { useDispatch, useSelector } from 'react-redux';
import { signin } from 'src/actions/authActions';
import AlertMessage from 'src/components/AlertMessage';
import BackdropSpinner from 'src/components/Backdrop';

const useStyles = makeStyles((theme) => ({
  root: {
    height: '100%',
    paddingBottom: theme.spacing(3),
    paddingTop: theme.spacing(3)
  },
  margin: {
    margin: theme.spacing(1)
  }
}));

const LoginView = () => {
  const classes = useStyles();
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const [openSnackbar, setOpenSnackbar] = useState(false);
  // const [loading, setLoading] = React.useState(false);

  const auth = useSelector((state) => state.auth);
  const { loading, errmsg } = useSelector((state) => state.authStatus);

  console.log(auth);
  console.log(loading);

  const handleLogin = (values) => {
    // console.log(values);

    dispatch(
      signin(values, () => {
        navigate('/app/today');
      })
    );

    setOpenSnackbar(true);
  };

  const handleSnackbarClose = () => {
    setOpenSnackbar(false);
  };

  return (
    <Page className={classes.root} title="Login">
      <Box
        display="flex"
        flexDirection="column"
        height="100%"
        justifyContent="center"
      >
        <Container maxWidth="sm">
          <Formik
            initialValues={{
              username: '',
              password: ''
            }}
            validationSchema={Yup.object().shape({
              username: Yup.string().max(9).required('Employee Id is required'),
              password: Yup.string().required('Password is required')
            })}
            onSubmit={(values, { resetForm }) => {
              handleLogin(values);

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
                  <Typography color="textPrimary" variant="h2">
                    Krungsri Smart Registration (iJoin....)
                  </Typography>
                  {/* <Typography
                    color="textSecondary"
                    gutterBottom
                    variant="body2"
                  >
                    Sign in
                  </Typography> */}
                </Box>
                <TextField
                  error={Boolean(touched.username && errors.username)}
                  fullWidth
                  helperText={touched.username && errors.username}
                  label="Employee ID"
                  margin="normal"
                  name="username"
                  onBlur={handleBlur}
                  onChange={handleChange}
                  value={values.username}
                  variant="outlined"
                  inputProps={{ maxLength: 9 }}
                />
                <TextField
                  error={Boolean(touched.password && errors.password)}
                  fullWidth
                  helperText={touched.password && errors.password}
                  label="Password"
                  margin="normal"
                  name="password"
                  onBlur={handleBlur}
                  onChange={handleChange}
                  value={values.password}
                  type="password"
                  variant="outlined"
                />
                <Box my={2}>
                  <Button
                    color="primary"
                    disabled={isSubmitting}
                    fullWidth
                    size="large"
                    type="submit"
                    variant="contained"
                  >
                    Sign in
                  </Button>
                </Box>
              </form>
            )}
          </Formik>
        </Container>
      </Box>
      {loading === 1 && (
        <BackdropSpinner isLoading={loading === 1 ? true : false} />
      )}
      {errmsg && (
        <AlertMessage
          openSnackbar={openSnackbar}
          handleSnackbarClose={handleSnackbarClose}
          message={errmsg}
          severity={'error'}
        />
      )}
    </Page>
  );
};

export default LoginView;
