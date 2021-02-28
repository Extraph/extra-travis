import React from 'react';
import { Link as RouterLink } from 'react-router-dom';
import clsx from 'clsx';
import PropTypes from 'prop-types';
import { AppBar, Toolbar, makeStyles, Avatar } from '@material-ui/core';
// import Logo from 'src/components/Logo';

const useStyles = makeStyles((theme) => ({
  root: {},
  toolbar: {
    // height: 64
  },
  logo: {
    width: theme.spacing(13.2),
    height: theme.spacing(6),
    backgroundColor: theme.palette.text.primary,
    boxShadow: theme.shadows[5]
    // marginTop: 12,
    // position: 'absolute',
    // zIndex: 9999
  }
}));

const TopBar = ({ className, ...rest }) => {
  const classes = useStyles();

  return (
    <AppBar className={clsx(classes.root, className)} elevation={0} {...rest}>
      <Toolbar className={classes.toolbar}>
        {/* <RouterLink to="/">
          <Logo />
        </RouterLink> */}
        <Avatar
          variant="square"
          className={classes.logo}
          component={RouterLink}
          src="/static/images/Krungsri_LOGO_H_RGB.png"
          to="/app/import"
        />
      </Toolbar>
    </AppBar>
  );
};

TopBar.propTypes = {
  className: PropTypes.string
};

export default TopBar;
