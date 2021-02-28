import React from 'react';
import clsx from 'clsx';
import PropTypes from 'prop-types';
import {
  Avatar,
  Card,
  CardContent,
  Grid,
  Typography,
  makeStyles
} from '@material-ui/core';
import InsertChartIcon from '@material-ui/icons/InsertChart';

const useStyles = makeStyles((theme) => ({
  root: {
    height: '100%'
  },
  avatar: {
    backgroundColor: theme.palette.success.main,
    height: 56,
    width: 56
  },
  success: {
    color: theme.palette.success.main
  }
}));

const KlcTotal = ({ className, number, ...rest }) => {
  const classes = useStyles();

  return (
    <Card elevation={4} className={clsx(classes.root, className)} {...rest}>
      <CardContent>
        <Grid container justify="space-between" spacing={3}>
          <Grid item>
            <Typography color="textSecondary" gutterBottom variant="h6">
              TOTAL
            </Typography>
            <Typography className={classes.success} variant="h3">
              {number}
            </Typography>
          </Grid>
          <Grid item>
            <Avatar className={classes.avatar}>
              <InsertChartIcon />
            </Avatar>
          </Grid>
        </Grid>
      </CardContent>
    </Card>
  );
};

KlcTotal.propTypes = {
  className: PropTypes.string
};

export default KlcTotal;
