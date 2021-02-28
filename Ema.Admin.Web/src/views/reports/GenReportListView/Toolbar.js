import React, { useState } from 'react';
import PropTypes from 'prop-types';
import clsx from 'clsx';
import {
  Box,
  Button,
  Card,
  CardContent,
  TextField,
  // InputAdornment,
  // SvgIcon,
  Grid,
  makeStyles,
  CardActions
} from '@material-ui/core';
// import { Search as SearchIcon } from 'react-feather';

const useStyles = makeStyles((theme) => ({
  root: {},
  searchButton: {
    marginLeft: theme.spacing(1)
  },
  actions: {
    justifyContent: 'flex-end'
  }
}));

const Toolbar = ({ className, onSearchSubmit, ...rest }) => {
  const classes = useStyles();

  const [sessionId, setSessionId] = useState('');

  // const onFormSubmit = (e) => {
  //   e.preventDefault();
  //   onSearchSubmit(sessionId);
  // };

  const onHandleClear = () => {};

  return (
    <div className={clsx(classes.root, className)} {...rest}>
      <Box>
        <Card>
          <CardContent>
            {/* <form onSubmit={onFormSubmit}> */}
            <Grid container spacing={1}>
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  onChange={(e) => {
                    setSessionId(e.target.value);
                  }}
                  label="Course Name"
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  onChange={(e) => {
                    setSessionId(e.target.value);
                  }}
                  label="Course ID"
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  onChange={(e) => {
                    setSessionId(e.target.value);
                  }}
                  label="Session Name"
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  onChange={(e) => {
                    setSessionId(e.target.value);
                  }}
                  label="Session ID"
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  onChange={(e) => {
                    setSessionId(e.target.value);
                  }}
                  label="Date"
                />
              </Grid>
            </Grid>
            {/* </form> */}
          </CardContent>
          <CardActions className={classes.actions}>
            <Button
              className={classes.searchButton}
              color="primary"
              variant="contained"
              onClick={() => {
                onSearchSubmit(sessionId);
              }}
            >
              Search
            </Button>
            <Button
              className={classes.searchButton}
              color="primary"
              variant="contained"
              onClick={() => {
                onHandleClear();
              }}
            >
              Clear
            </Button>
          </CardActions>
        </Card>
      </Box>
    </div>
  );
};

Toolbar.propTypes = {
  className: PropTypes.string
};

export default Toolbar;
