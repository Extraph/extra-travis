import React, { useState } from 'react';
import PropTypes from 'prop-types';
import clsx from 'clsx';
import {
  Box,
  Button,
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

  const [searchTerm, setSearchTerm] = useState('');

  const onFormSubmit = (e) => {
    e.preventDefault();
    onSearchSubmit(searchTerm);
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
              onChange={(e) => {
                setSearchTerm(e.target.value);
              }}
              placeholder="Course ID or Name"
              variant="outlined"
            />
          </form>
          <Button
            className={classes.searchButton}
            color="primary"
            variant="contained"
            onClick={() => {
              onSearchSubmit(searchTerm);
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
