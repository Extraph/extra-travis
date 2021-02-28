import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { Backdrop, CircularProgress } from '@material-ui/core';

const useStyles = makeStyles((theme) => ({
  backdrop: {
    zIndex: theme.zIndex.drawer + 1,
    color: '#fff'
  }
}));

const BackdropSpinner = (props) => {
  const classes = useStyles();

  return (
    <div>
      <Backdrop className={classes.backdrop} open={props.isLoading}>
        <CircularProgress color="primary" size={60} />
      </Backdrop>
    </div>
  );
};

BackdropSpinner.defaultProps = {
  isLoading: false
};

export default BackdropSpinner;
