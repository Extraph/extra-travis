import React, { useState } from 'react';
import PropTypes from 'prop-types';
import clsx from 'clsx';
import {
  Box,
  Button,
  // Card,
  // CardContent,
  TextField,
  InputAdornment,
  SvgIcon,
  makeStyles
} from '@material-ui/core';
import { Search as SearchIcon } from 'react-feather';

const useStyles = makeStyles((theme) => ({
  root: {},
  searchButton: {
    marginLeft: theme.spacing(1)
  }
}));

const Toolbar = ({ className, onSearchSubmit, ...rest }) => {
  const classes = useStyles();

  const [sessionId, setSessionId] = useState('');

  const onFormSubmit = (e) => {
    e.preventDefault();
    onSearchSubmit(sessionId);
  };

  return (
    <div className={clsx(classes.root, className)} {...rest}>
      <Box>
        {/* <Card>
          <CardContent> */}
        <Box
          // maxWidth={500}
          display="flex"
          justifyContent="flex-start"
          // alignItems="center"
        >
          <form onSubmit={onFormSubmit}>
            <TextField
              fullWidth
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <SvgIcon fontSize="small" color="action">
                      <SearchIcon />
                    </SvgIcon>
                  </InputAdornment>
                )
              }}
              // type="number"
              onChange={(e) => {
                setSessionId(e.target.value);
              }}
              placeholder="Search Session ID"
              variant="outlined"
            />
          </form>
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
        </Box>
        {/* </CardContent>
        </Card> */}
      </Box>
    </div>
  );
};

Toolbar.propTypes = {
  className: PropTypes.string
};

export default Toolbar;
