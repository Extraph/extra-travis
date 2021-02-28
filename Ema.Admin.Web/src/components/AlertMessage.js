import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { Snackbar } from '@material-ui/core';
import MuiAlert from '@material-ui/lab/Alert';

const useStyles = makeStyles((theme) => ({
  alertMessage: {
    fontWeight: 300
  }
  // snackbar: {
  //   [theme.breakpoints.down('xs')]: {
  //     bottom: 90
  //   }
  // }
}));

function Alert(props) {
  return <MuiAlert elevation={6} variant="filled" {...props} />;
}

const AlertMessage = (props) => {
  const classes = useStyles();
  const { openSnackbar, handleSnackbarClose, message, severity } = props;
  return (
    <div>
      <Snackbar
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'center'
        }}
        open={openSnackbar}
        autoHideDuration={10000}
        onClose={handleSnackbarClose}
        className={classes.snackbar}
      >
        <Alert
          className={classes.alertMessage}
          onClose={handleSnackbarClose}
          severity={severity}
        >
          {message}
        </Alert>
      </Snackbar>
    </div>
  );
};

export default AlertMessage;
