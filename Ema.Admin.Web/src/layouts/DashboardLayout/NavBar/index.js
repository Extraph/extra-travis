import React, { useEffect } from 'react';
import { Link as RouterLink, useLocation } from 'react-router-dom';
import PropTypes from 'prop-types';
import {
  Avatar,
  Box,
  Button,
  Divider,
  Drawer,
  Hidden,
  List,
  Typography,
  makeStyles,
  Collapse,
  ListItem
  // ListItemText
} from '@material-ui/core';
import {
  // AlertCircle as AlertCircleIcon,
  Printer as PrinterIcon,
  // Lock as LockIcon,
  // Settings as SettingsIcon,
  Monitor as MonitorIcon,
  // User as UserIcon,
  // UserPlus as UserPlusIcon,
  Download as DownloadIcon,
  Calendar as CalendarIcon,
  FileText as FileTextIcon,
  ChevronUp as ChevronUpIcon,
  ChevronDown as ChevronDownIcon,
  Folder as FolderIcon
} from 'react-feather';
import NavItem from './NavItem';
import { useSelector } from 'react-redux';

const user = {
  avatar:
    '/static/images/avatars/41446589_10212912224819889_7258837223817084928_n.jpg',
  jobTitle: 'Senior Developer',
  name: 'Rattawit P.'
};

const masterItems = [
  {
    href: '/app/master/coursetype',
    icon: MonitorIcon,
    title: 'Course Type Manage'
  },
  {
    href: '/app/master/company',
    icon: MonitorIcon,
    title: 'Company Manage'
  },
  {
    href: '/app/master/roleassign',
    icon: MonitorIcon,
    title: 'Role Assignment'
  }
];
const sessionItems = [
  {
    href: '/app/import',
    icon: DownloadIcon,
    title: 'Import Data'
  },
  {
    href: '/app/genqrcode',
    icon: PrinterIcon,
    title: 'Generate QR Code'
  }
];
const userItems = [
  {
    href: '/app/today',
    icon: MonitorIcon,
    title: 'Today Class'
  },
  {
    href: '/app/next',
    icon: CalendarIcon,
    title: 'Next Classes'
  },
  {
    href: '/app/dash',
    icon: CalendarIcon,
    title: 'Next Dash Board'
  }
];
const reportItems = [
  {
    href: '/app/report',
    icon: FileTextIcon,
    title: 'Report'
  }
];

const useStyles = makeStyles((theme) => ({
  mobileDrawer: {
    width: 256
  },
  desktopDrawer: {
    width: 256,
    top: 64,
    height: 'calc(100% - 64px)'
  },
  avatar: {
    cursor: 'pointer',
    width: 64,
    height: 64
  },
  button: {
    color: theme.palette.text.secondary,
    fontWeight: theme.typography.fontWeightMedium,
    justifyContent: 'flex-start',
    letterSpacing: 0,
    padding: '10px 8px',
    textTransform: 'none',
    width: '100%'
  },
  icon: {
    marginRight: theme.spacing(1)
  },
  item: {
    display: 'flex',
    paddingTop: 0,
    paddingBottom: 0
  },
  list: {
    paddingTop: 0,
    paddingBottom: 0
  },
  title: {
    marginRight: 'auto'
  }
}));

const NavBar = ({ onMobileClose, openMobile }) => {
  const classes = useStyles();
  const location = useLocation();

  const auth = useSelector((state) => state.auth);

  const [openMaster, setOpenMaster] = React.useState(true);
  const [openSession, setOpenSession] = React.useState(true);
  const [openUser, setOpenUser] = React.useState(true);
  const [openReport, setOpenReport] = React.useState(true);

  const handleMasterClick = () => {
    setOpenMaster(!openMaster);
  };
  const handleSessionClick = () => {
    setOpenSession(!openSession);
  };
  const handleUserClick = () => {
    setOpenUser(!openUser);
  };
  const handleReportClick = () => {
    setOpenReport(!openReport);
  };

  useEffect(() => {
    if (openMobile && onMobileClose) {
      onMobileClose();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [location.pathname]);

  const { roleName } = auth.authenticated;

  const content = (
    <Box height="100%" display="flex" flexDirection="column">
      <Box alignItems="center" display="flex" flexDirection="column" p={2}>
        <Avatar
          className={classes.avatar}
          component={RouterLink}
          src={user.avatar}
          to="/app/account"
        />
        <Typography className={classes.name} color="textPrimary" variant="h5">
          {user.name}
        </Typography>
        <Typography color="textSecondary" variant="body2">
          {user.jobTitle}
        </Typography>
      </Box>
      <Divider />
      <Box p={2}>
        {(roleName === 'Super Admin' || roleName === 'Admin') && (
          <>
            <ListItem
              className={classes.item}
              disableGutters
              onClick={handleSessionClick}
            >
              <Button className={classes.button}>
                <FolderIcon className={classes.icon} size="20" />
                <span className={classes.title}>Manage Session</span>
                {openSession ? <ChevronUpIcon /> : <ChevronDownIcon />}
              </Button>
            </ListItem>
            <Collapse in={openSession} timeout="auto" unmountOnExit>
              <List className={classes.list}>
                {sessionItems.map((item) => (
                  <NavItem
                    href={item.href}
                    key={item.title}
                    title={item.title}
                    icon={item.icon}
                  />
                ))}
              </List>
            </Collapse>
          </>
        )}

        {(roleName === 'Super Admin' ||
          roleName === 'Admin' ||
          roleName === 'Instructor') && (
          <>
            <ListItem
              className={classes.item}
              disableGutters
              onClick={handleUserClick}
            >
              <Button className={classes.button}>
                <FolderIcon className={classes.icon} size="20" />
                <span className={classes.title}>Manage User</span>
                {openUser ? <ChevronUpIcon /> : <ChevronDownIcon />}
              </Button>
            </ListItem>
            <Collapse in={openUser} timeout="auto" unmountOnExit>
              <List className={classes.list}>
                {userItems.map((item) => (
                  <NavItem
                    href={item.href}
                    key={item.title}
                    title={item.title}
                    icon={item.icon}
                  />
                ))}
              </List>
            </Collapse>
          </>
        )}

        {(roleName === 'Super Admin' ||
          roleName === 'Admin' ||
          roleName === 'Report Admin') && (
          <>
            <ListItem
              className={classes.item}
              disableGutters
              onClick={handleReportClick}
            >
              <Button className={classes.button}>
                <FolderIcon className={classes.icon} size="20" />
                <span className={classes.title}>Report</span>
                {openReport ? <ChevronUpIcon /> : <ChevronDownIcon />}
              </Button>
            </ListItem>
            <Collapse in={openReport} timeout="auto" unmountOnExit>
              <List className={classes.list}>
                {reportItems.map((item) => (
                  <NavItem
                    href={item.href}
                    key={item.title}
                    title={item.title}
                    icon={item.icon}
                  />
                ))}
              </List>
            </Collapse>
          </>
        )}

        {roleName === 'Super Admin' && (
          <>
            <ListItem
              className={classes.item}
              disableGutters
              onClick={handleMasterClick}
            >
              <Button className={classes.button}>
                <FolderIcon className={classes.icon} size="20" />
                <span className={classes.title}>Manage Master Data</span>
                {openMaster ? <ChevronUpIcon /> : <ChevronDownIcon />}
              </Button>
            </ListItem>
            <Collapse in={openMaster} timeout="auto" unmountOnExit>
              <List className={classes.list}>
                {masterItems.map((item) => (
                  <NavItem
                    href={item.href}
                    key={item.title}
                    title={item.title}
                    icon={item.icon}
                  />
                ))}
              </List>
            </Collapse>
          </>
        )}
      </Box>
      <Box flexGrow={1} />
    </Box>
  );

  return (
    <>
      <Hidden lgUp>
        <Drawer
          anchor="left"
          classes={{ paper: classes.mobileDrawer }}
          onClose={onMobileClose}
          open={openMobile}
          variant="temporary"
        >
          {content}
        </Drawer>
      </Hidden>
      <Hidden mdDown>
        <Drawer
          anchor="left"
          classes={{ paper: classes.desktopDrawer }}
          open
          variant="persistent"
        >
          {content}
        </Drawer>
      </Hidden>
    </>
  );
};

NavBar.propTypes = {
  onMobileClose: PropTypes.func,
  openMobile: PropTypes.bool
};

NavBar.defaultProps = {
  onMobileClose: () => {},
  openMobile: false
};

export default NavBar;
