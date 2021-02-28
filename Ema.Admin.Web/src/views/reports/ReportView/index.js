import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { withStyles } from '@material-ui/core/styles';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import IconButton from '@material-ui/core/IconButton';
import Typography from '@material-ui/core/Typography';
import CloseIcon from '@material-ui/icons/Close';
import Slide from '@material-ui/core/Slide';
import moment from 'moment';
import {
  Box,
  // Container,
  Card,
  CardContent,
  CardActions,
  Grid,
  TextField,
  DialogContent
} from '@material-ui/core';
import PerfectScrollbar from 'react-perfect-scrollbar';
import ReportList from './ReportList';

const useStyles = makeStyles((theme) => ({
  appBar: {
    position: 'relative'
  },
  title: {
    marginLeft: theme.spacing(2),
    flex: 1
  }
}));

const Transition = React.forwardRef(function Transition(props, ref) {
  return <Slide direction="up" ref={ref} {...props} />;
});

const DialogContentStyle = withStyles((theme) => ({
  root: {
    padding: theme.spacing(2),
    backgroundColor: theme.palette.background.dark
  }
}))(DialogContent);

export default function FullScreenReport({
  reportHeaderData,
  reportListData,
  isOpen,
  onClose
}) {
  const classes = useStyles();

  const handleClose = () => {
    onClose();
  };

  const {
    courseCreditHours,
    courseCreditHoursInit,
    courseId,
    courseName,
    // courseNameTh,
    // courseOwnerContactNo,
    courseOwnerEmail,
    // courseTypeId,
    instructor,
    passingCriteriaException,
    // passingCriteriaExceptionInit,
    sessionId,
    sessionName,
    startDateTime,
    endDateTime,
    venue
  } = reportHeaderData;

  const reportCancelled = reportListData.filter(
    (r) => r.trainingStatus === 'Cancelled'
  );
  const reportNoShow = reportListData.filter(
    (r) => r.trainingStatus === 'No Show'
  );
  const reportCompleted = reportListData.filter(
    (r) => r.trainingStatus === 'Completed'
  );
  const reportIncompleted = reportListData.filter(
    (r) => r.trainingStatus === 'Incompleted'
  );

  const eventText =
    moment(startDateTime).format('DD/MM/YYYY') ===
    moment(endDateTime).format('DD/MM/YYYY')
      ? moment(startDateTime).format('DD/MM/YYYY')
      : `${moment(startDateTime).format('DD/MM/YYYY')} - ${moment(
          endDateTime
        ).format('DD/MM/YYYY')}`;

  console.log(reportListData);

  return (
    <div>
      <Dialog
        fullScreen
        open={isOpen}
        onClose={handleClose}
        TransitionComponent={Transition}
      >
        <AppBar className={classes.appBar}>
          <Toolbar>
            <IconButton
              edge="start"
              color="inherit"
              onClick={handleClose}
              aria-label="close"
            >
              <CloseIcon />
            </IconButton>
            <Typography variant="h4" className={classes.title}></Typography>
            <Button autoFocus color="inherit" onClick={handleClose}>
              Close
            </Button>
          </Toolbar>
        </AppBar>
        <DialogContentStyle>
          <Box mt={1} ml={1} mr={1} mb={1}>
            <Card>
              <CardContent>
                <Grid container spacing={3}>
                  <Grid item xs={12} md={7}>
                    <Grid container spacing={1}>
                      <Grid item xs={12} md={6}>
                        <TextField
                          size="small"
                          fullWidth
                          value={courseName ? courseName : ''}
                          label="Course"
                          InputProps={{
                            readOnly: true
                          }}
                        />
                      </Grid>
                      <Grid item xs={12} md={6}>
                        <TextField
                          size="small"
                          fullWidth
                          value={courseId ? courseId : ''}
                          label="Course No."
                          InputProps={{
                            readOnly: true
                          }}
                        />
                      </Grid>
                      <Grid item xs={12} md={6}>
                        <TextField
                          size="small"
                          fullWidth
                          value={sessionName ? sessionName : ''}
                          label="Session"
                          InputProps={{
                            readOnly: true
                          }}
                        />
                      </Grid>
                      <Grid item xs={12} md={6}>
                        <TextField
                          size="small"
                          fullWidth
                          value={sessionId ? sessionId : ''}
                          label="Session No."
                          InputProps={{
                            readOnly: true
                          }}
                        />
                      </Grid>
                      <Grid item xs={12} md={6}>
                        <TextField
                          size="small"
                          fullWidth
                          value={eventText ? eventText : ''}
                          label="Date"
                          InputProps={{
                            readOnly: true
                          }}
                        />
                      </Grid>
                      <Grid item xs={12} md={6}>
                        <TextField
                          size="small"
                          fullWidth
                          value={`${moment(startDateTime).format(
                            'hh:mm A'
                          )} - ${moment(endDateTime).format('hh:mm A')}`}
                          label="Time"
                          InputProps={{
                            readOnly: true
                          }}
                        />
                      </Grid>
                      <Grid item xs={12} md={6}>
                        <TextField
                          size="small"
                          fullWidth
                          value={venue ? venue : ''}
                          label="Venue"
                          InputProps={{
                            readOnly: true
                          }}
                        />
                      </Grid>
                      <Grid item xs={12} md={6}>
                        <TextField
                          size="small"
                          fullWidth
                          value={
                            courseCreditHoursInit ? courseCreditHoursInit : ''
                          }
                          label="Credit Hours"
                          InputProps={{
                            readOnly: true
                          }}
                        />
                      </Grid>
                      <Grid item xs={12} md={6}>
                        <TextField
                          size="small"
                          fullWidth
                          value={
                            passingCriteriaException
                              ? parseFloat(passingCriteriaException) * 100
                              : ''
                          }
                          label="Passing Criteria"
                          InputProps={{
                            readOnly: true
                          }}
                        />
                      </Grid>
                      <Grid item xs={12} md={6}>
                        <TextField
                          size="small"
                          fullWidth
                          value={courseCreditHours ? courseCreditHours : ''}
                          label="Credit Hours for Calculation"
                          InputProps={{
                            readOnly: true
                          }}
                        />
                      </Grid>

                      <Grid item xs={12} md={6}>
                        <TextField
                          size="small"
                          fullWidth
                          value={instructor ? instructor : ''}
                          label="Instructor"
                          InputProps={{
                            readOnly: true
                          }}
                        />
                      </Grid>
                    </Grid>
                  </Grid>
                  <Grid item xs={12} md={5}>
                    <Card elevation={5}>
                      <CardContent>
                        <Grid container spacing={0}>
                          <Grid item xs={6} md={6}>
                            <Typography
                              color="textSecondary"
                              gutterBottom
                              variant="subtitle1"
                            >
                              No. of Participants
                            </Typography>
                          </Grid>
                          <Grid item xs={6} md={6}></Grid>
                          <Grid item xs={6} md={6}>
                            <Typography
                              color="textSecondary"
                              gutterBottom
                              variant="subtitle1"
                            >
                              Total Participants
                            </Typography>
                          </Grid>
                          <Grid item xs={6} md={6}>
                            <Typography
                              color="textSecondary"
                              gutterBottom
                              variant="subtitle1"
                            >
                              {`${reportListData.length} Persons`}
                            </Typography>
                          </Grid>
                          <Grid item xs={6} md={6}>
                            <Typography
                              color="textSecondary"
                              gutterBottom
                              variant="subtitle1"
                            >
                              Completed
                            </Typography>
                          </Grid>
                          <Grid item xs={6} md={6}>
                            <Typography
                              color="textSecondary"
                              gutterBottom
                              variant="subtitle1"
                            >
                              {`${reportCompleted.length} Persons`}
                            </Typography>
                          </Grid>
                          <Grid item xs={6} md={6}>
                            <Typography
                              color="textSecondary"
                              gutterBottom
                              variant="subtitle1"
                            >
                              Incompleted
                            </Typography>
                          </Grid>
                          <Grid item xs={6} md={6}>
                            <Typography
                              color="textSecondary"
                              gutterBottom
                              variant="subtitle1"
                            >
                              {`${reportIncompleted.length} Persons`}
                            </Typography>
                          </Grid>
                          <Grid item xs={6} md={6}>
                            <Typography
                              color="textSecondary"
                              gutterBottom
                              variant="subtitle1"
                            >
                              No Show
                            </Typography>
                          </Grid>
                          <Grid item xs={6} md={6}>
                            <Typography
                              color="textSecondary"
                              gutterBottom
                              variant="subtitle1"
                            >
                              {`${reportNoShow.length} Persons`}
                            </Typography>
                          </Grid>
                          <Grid item xs={6} md={6}>
                            <Typography
                              color="textSecondary"
                              gutterBottom
                              variant="subtitle1"
                            >
                              Cancelled
                            </Typography>
                          </Grid>
                          <Grid item xs={6} md={6}>
                            <Typography
                              color="textSecondary"
                              gutterBottom
                              variant="subtitle1"
                            >
                              {`${reportCancelled.length} Persons`}
                            </Typography>
                          </Grid>
                          <Grid item xs={6} md={6}>
                            <Typography
                              color="textSecondary"
                              gutterBottom
                              variant="subtitle1"
                            >
                              Course Owner
                            </Typography>
                          </Grid>
                          <Grid item xs={6} md={6}>
                            <Typography
                              color="textSecondary"
                              gutterBottom
                              variant="subtitle1"
                            >
                              {courseOwnerEmail}
                            </Typography>
                          </Grid>
                        </Grid>
                      </CardContent>
                    </Card>
                  </Grid>
                </Grid>
              </CardContent>
              <CardActions className={classes.actions}>
                {/* <Button
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
              </Button> */}
              </CardActions>
            </Card>
          </Box>

          <Box mt={3} ml={1} mr={1} mb={1}>
            <Card elevation={5}>
              <CardContent className={classes.content}>
                <PerfectScrollbar>
                  <ReportList data={reportListData} />
                </PerfectScrollbar>
              </CardContent>
              <CardActions className={classes.actions}></CardActions>
            </Card>
          </Box>
        </DialogContentStyle>
      </Dialog>
    </div>
  );
}
