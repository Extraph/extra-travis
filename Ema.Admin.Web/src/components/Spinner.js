import React from 'react';
import { makeStyles, withStyles } from '@material-ui/core/styles';
import { LinearProgress } from '@material-ui/core';

const BorderLinearProgress = withStyles((theme) => ({
  root: {
    height: 5,
    borderRadius: 5
  },
  bar: {
    borderRadius: 5
  }
}))(LinearProgress);

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
    padding: theme.spacing(6)
  }
}));

const Spinner = () => {
  const classes = useStyles();

  return (
    <div className={classes.root}>
      <BorderLinearProgress />
    </div>
  );
};

export default Spinner;
