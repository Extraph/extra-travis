import React from 'react';
import { Link as RouterLink } from 'react-router-dom';
import clsx from 'clsx';
import PropTypes from 'prop-types';
import {
  AppBar,
  // Badge,
  Box,
  Hidden,
  IconButton,
  Toolbar,
  makeStyles,
  // withStyles,
  Avatar,
  Typography
} from '@material-ui/core';
import { AccountCircle, Input, Menu } from '@material-ui/icons';
import { useDispatch, useSelector } from 'react-redux';
import { signout } from 'src/actions/authActions';

const useStyles = makeStyles((theme) => ({
  root: {},
  logo: {
    width: theme.spacing(13.2),
    height: theme.spacing(6),
    backgroundColor: theme.palette.text.primary,
    boxShadow: theme.shadows[5]
  },
  menuIcon: {
    // marginLeft: theme.spacing(7)
  },
  name: {
    // marginLeft: theme.spacing(1)
  }
}));

// const StyledBadge = withStyles((theme) => ({
//   badge: {
//     backgroundColor: '#44b700',
//     color: '#44b700',
//     boxShadow: `0 0 0 2px ${theme.palette.background.paper}`,
//     '&::after': {
//       position: 'absolute',
//       top: 0,
//       left: 0,
//       width: '100%',
//       height: '100%',
//       borderRadius: '50%',
//       animation: '$ripple 1.2s infinite ease-in-out',
//       border: '1px solid currentColor',
//       content: '""'
//     }
//   },
//   '@keyframes ripple': {
//     '0%': {
//       transform: 'scale(.8)',
//       opacity: 1
//     },
//     '100%': {
//       transform: 'scale(2.4)',
//       opacity: 0
//     }
//   }
// }))(Badge);

const TopBar = ({ className, onMobileNavOpen, ...rest }) => {
  const classes = useStyles();
  const dispatch = useDispatch();
  const auth = useSelector((state) => state.auth.authenticated);

  // console.log(auth);

  const handleLogout = () => {
    dispatch(signout());
  };

  return (
    <AppBar className={clsx(classes.root, className)} elevation={0} {...rest}>
      <Toolbar>
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
        <Hidden lgUp>
          <IconButton
            className={classes.menuIcon}
            color="inherit"
            onClick={onMobileNavOpen}
          >
            <Menu />
          </IconButton>
        </Hidden>
        <Box flexGrow={1} />
        {/* <Hidden mdDown> */}

        {/* <StyledBadge
          overlap="circle"
          anchorOrigin={{
            vertical: 'bottom',
            horizontal: 'right'
          }}
          variant="dot"
        >
          <Avatar
            alt="Rattawit"
            src="/static/images/avatars/41446589_10212912224819889_7258837223817084928_n.jpg"
          />
        </StyledBadge> */}
        <IconButton color="inherit">
          <AccountCircle />
        </IconButton>
        <Typography className={classes.name} variant="h4">
          {auth.userId}
        </Typography>
        <IconButton
          component={RouterLink}
          to="/login"
          onClick={handleLogout}
          color="inherit"
        >
          <Input />
        </IconButton>
        {/* </Hidden> */}
      </Toolbar>
    </AppBar>
  );
};

TopBar.propTypes = {
  className: PropTypes.string,
  onMobileNavOpen: PropTypes.func
};

export default TopBar;
