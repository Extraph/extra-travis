import React, { useState, useEffect } from 'react';
import { NavLink as RouterLink } from 'react-router-dom';
import {
  Box,
  Container,
  Card,
  CardContent,
  CardActions,
  makeStyles,
  Typography,
  Fab
} from '@material-ui/core';
import { Add, ArrowBackIos } from '@material-ui/icons';
import Page from 'src/components/Page';
import Spinner from 'src/components/Spinner';
import Toolbar from './Toolbar';
import { useDispatch, useSelector } from 'react-redux';
import {
  fetchParticipant,
  fetchUserRegistration
} from 'src/actions/participant';
import { useNavigate } from 'react-router-dom';
import AlertMessage from 'src/components/AlertMessage';
import PerfectScrollbar from 'react-perfect-scrollbar';
import ParticipantList from './ParticipantList';
import ParticipantNextList from './ParticipantNextList';
import { selectSessionUser } from 'src/actions/selected';
import moment from 'moment';

const useStyles = makeStyles((theme) => ({
  root: {
    backgroundColor: theme.palette.background.dark,
    height: '100%',
    paddingBottom: theme.spacing(3),
    paddingTop: theme.spacing(3),
    position: 'relative',
    minHeight: 500
  },
  content: {
    padding: 0
  },
  actions: {
    justifyContent: 'flex-end',
    padding: 0
  },
  add: {
    position: 'absolute',
    bottom: theme.spacing(2),
    right: theme.spacing(2)
  },
  back: {
    position: 'absolute',
    bottom: theme.spacing(2),
    left: theme.spacing(2)
  }
}));

const ParticipantListView = (props) => {
  const classes = useStyles();
  const [openSnackbar, setOpenSnackbar] = useState(false);
  const dispatch = useDispatch();
  const participant = useSelector((state) => state.participant);
  const userRegis = useSelector((state) => state.userRegis);
  // const { checkInNumber, checkOutNumber, data } = participantObj;

  const session = useSelector((state) => state.selected.session);
  const editFrom = useSelector((state) => state.selected.editFrom);

  const { loading, errmsg } = useSelector((state) => state.participantStatus);
  const navigate = useNavigate();

  const onSearchSubmit = (empId) => {
    console.log(empId);
    dispatch(fetchParticipant({ sessionId: session.sessionId, userId: empId }));
    dispatch(
      fetchUserRegistration({ sessionId: session.sessionId, userId: empId })
    );
    setOpenSnackbar(true);
  };
  const handleSnackbarClose = () => {
    setOpenSnackbar(false);
  };

  const handleEditNavigate = (row) => {
    if (roleName !== 'Instructor') {
      dispatch(selectSessionUser(row));
      navigate('/app/participant/edit');
    }
  };

  useEffect(() => {
    if (!session.sessionId) navigate('/app/today');
  });

  const participantEnroll = participant.filter(
    (par) => par.registrationStatus === 'Enrolled'
  );

  const userRegisIn = userRegis.filter((user) => user.isCheckIn === '1');
  const userRegisOut = userRegis.filter((user) => user.isCheckOut === '1');

  // console.log(participant);
  // console.log(participantEnroll);
  // console.log(userRegis);

  let participantRegis = [];
  if (editFrom === 'today') {
    for (let parti of participant) {
      let partiChkIn = { ...parti, checkInDateTime: '', checkOutDateTime: '' };

      for (const regis of userRegis) {
        if (parti.userId === regis.userId) {
          partiChkIn = {
            ...partiChkIn,
            registrationStatus:
              regis.isCheckOut === '1' ? 'Check-Out' : 'Check-In'
          };

          if (partiChkIn.registrationStatus === 'Check-In')
            partiChkIn = {
              ...partiChkIn,
              checkInDateTime: moment(regis.checkInDateTime).format('hh:mm A')
            };

          if (partiChkIn.registrationStatus === 'Check-Out') {
            partiChkIn = {
              ...partiChkIn,
              checkInDateTime: moment(regis.checkInDateTime).format('hh:mm A'),
              checkOutDateTime: moment(regis.checkOutDateTime).format('hh:mm A')
            };
          }
        }
      }

      participantRegis = [...participantRegis, partiChkIn];
    }
  }

  // console.log(participantRegis);
  // console.log(participant);

  const auth = useSelector((state) => state.auth);
  const { roleName } = auth.authenticated;

  return (
    <Page className={classes.root} title="Participant">
      <Container maxWidth={false}>
        <Box mb={3}>
          <Typography color="textSecondary" variant="h4">
            {`${session.courseName} (${session.courseId})`}
          </Typography>
          <Typography color="textSecondary" gutterBottom variant="h4">
            {`${session.sessionName} (${session.sessionId})`}
          </Typography>

          <Typography color="textSecondary" gutterBottom variant="h5">
            {`จำนวนผู้สมัคร (Enrolled) (${participantEnroll.length})`}
          </Typography>
        </Box>
        <Toolbar onSearchSubmit={onSearchSubmit} />
        <Box mt={3} mb={3}>
          {loading ? (
            <Spinner />
          ) : (
            <React.Fragment>
              {participant.length > 0 ? (
                <Card elevation={5}>
                  <CardContent className={classes.content}>
                    <PerfectScrollbar>
                      {editFrom === 'today' ? (
                        <ParticipantList
                          tableData={
                            editFrom === 'today'
                              ? participantRegis
                              : participant
                          }
                          onTableNavigate={handleEditNavigate}
                        />
                      ) : (
                        <ParticipantNextList
                          tableData={
                            editFrom === 'today'
                              ? participantRegis
                              : participant
                          }
                          onTableNavigate={handleEditNavigate}
                        />
                      )}
                    </PerfectScrollbar>
                  </CardContent>
                  <CardActions className={classes.actions}></CardActions>
                </Card>
              ) : (
                <Typography color="error" gutterBottom variant="h5">
                  {`ไม่พบข้อมูลที่ค้นหา`}
                </Typography>
              )}
            </React.Fragment>
          )}
        </Box>

        {editFrom === 'today' && (
          <Box mb={3}>
            <Typography color="textSecondary" gutterBottom variant="h5">
              {`* จำนวนคนที่ Check-In แล้วทั้งหมด (${userRegisIn.length})`}
            </Typography>
            <Typography color="textSecondary" gutterBottom variant="h5">
              {`* จำนวนคนที่ Check-Out แล้วทั้งหมด (${userRegisOut.length})`}
            </Typography>
          </Box>
        )}

        <Box>
          <Fab
            color="secondary"
            className={classes.back}
            component={RouterLink}
            to={editFrom === 'today' ? '/app/today' : '/app/next'}
          >
            <ArrowBackIos />
          </Fab>
          {roleName !== 'Instructor' && (
            <Fab
              color="primary"
              className={classes.add}
              component={RouterLink}
              to={'/app/participant/add'}
            >
              <Add />
            </Fab>
          )}
        </Box>
      </Container>
      {errmsg && (
        <AlertMessage
          openSnackbar={openSnackbar}
          handleSnackbarClose={handleSnackbarClose}
          message={errmsg.toString()}
          severity={'error'}
        />
      )}
    </Page>
  );
};

export default ParticipantListView;
