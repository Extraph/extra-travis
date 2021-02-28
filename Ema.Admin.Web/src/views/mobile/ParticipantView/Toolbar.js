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
  },
  addButton: {
    marginTop: theme.spacing(2)
  }
}));

const Toolbar = ({ className, onSearchSubmit, ...rest }) => {
  const classes = useStyles();

  const [empId, setEmpId] = useState('');

  const onFormSubmit = (e) => {
    e.preventDefault();
    onSearchSubmit(empId);
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
                setEmpId(e.target.value);
              }}
              placeholder="Emp ID"
              variant="outlined"
            />
          </form>
          <Button
            className={classes.searchButton}
            color="primary"
            variant="contained"
            onClick={() => {
              onSearchSubmit(empId);
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
