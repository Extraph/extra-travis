import React from 'react';
import {
  Box,
  Card,
  CardContent,
  CardActions,
  Grid,
  Paper,
  Typography,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  Chip
} from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';
import QRCode from 'react-qr-code';
import { AccessTime, Event, LocationOn } from '@material-ui/icons';
import moment from 'moment';

const styles = (theme) => ({
  content: {
    padding: 0
  },
  actions: {
    justifyContent: 'flex-end',
    padding: 0
  },
  boxContent: {
    width: 760,
    padding: 0
  },
  boxQr: {
    height: 1120,
    padding: theme.spacing(5),
    paddingTop: theme.spacing(35),
    margin: theme.spacing(0)
  },
  paperQr: {
    color: theme.palette.text.secondary,
    backgroundColor: theme.palette.text.primary
  },
  typographyFont: {
    color: theme.palette.common.black
  }
});

const qrInOut = ['Check-In', 'Check-Out'];

class QrCodeView extends React.PureComponent {
  render() {
    const { qrData, classes } = this.props;

    return (
      <Card elevation={0}>
        <CardContent className={classes.content}>
          {qrData.segmentsQr.map((qrDay) => (
            <React.Fragment key={qrDay.startDateTime}>
              {qrInOut.map((inout) => (
                <Box
                  className={classes.boxQr}
                  key={inout}
                  bgcolor="text.primary"
                >
                  <Grid
                    container
                    direction="column"
                    justify="space-between"
                    alignItems="center"
                    spacing={3}
                  >
                    <Grid item>
                      <Typography
                        className={classes.typographyFont}
                        align="center"
                        color="inherit"
                        variant="h4"
                      >
                        {`${qrDay.courseName} (${qrDay.courseId})`}
                      </Typography>
                      <Typography
                        className={classes.typographyFont}
                        align="center"
                        color="inherit"
                        variant="h4"
                      >
                        {`${qrDay.sessionName} (${qrDay.sessionId})`}
                      </Typography>
                      {qrDay.segmentName && (
                        <Typography
                          className={classes.typographyFont}
                          align="center"
                          color="inherit"
                          variant="h4"
                        >
                          {`${qrDay.segmentName}`}
                        </Typography>
                      )}
                    </Grid>
                    <Grid item>
                      <Paper className={classes.paperQr}>
                        <QRCode
                          size={300}
                          value={`${inout}:${qrDay.sessionId}:${moment(
                            qrDay.startDateTime
                          ).format('YYYYMMDD')}:${moment(
                            inout === 'Check-In'
                              ? qrDay.startDateTime
                              : qrDay.endDateTime
                          ).format('HHmm')}`}
                        />
                      </Paper>
                    </Grid>
                    {/* <Chip
                      label={`${inout}:${qrDay.sessionId}:${moment(
                        qrDay.startDateTime
                      ).format('YYYYMMDD')}:${moment(
                        inout === 'Check-In'
                          ? qrDay.startDateTime
                          : qrDay.endDateTime
                      ).format('HHmm')}`}
                      color="primary"
                    /> */}
                    <Chip label={inout} color="primary" />
                    <List dense>
                      <ListItem>
                        <ListItemIcon className={classes.typographyFont}>
                          <Event />
                        </ListItemIcon>
                        <ListItemText
                          className={classes.typographyFont}
                          primary={moment(qrDay.startDateTime).format(
                            'DD/MM/YYYY'
                          )}
                        />
                      </ListItem>
                      <ListItem>
                        <ListItemIcon className={classes.typographyFont}>
                          <AccessTime />
                        </ListItemIcon>
                        <ListItemText
                          className={classes.typographyFont}
                          primary={`${moment(qrDay.startDateTime).format(
                            'hh:mm A'
                          )} - ${moment(qrDay.endDateTime).format('hh:mm A')}`}
                        />
                      </ListItem>
                      <ListItem>
                        <ListItemIcon className={classes.typographyFont}>
                          <LocationOn />
                        </ListItemIcon>
                        <ListItemText
                          className={classes.typographyFont}
                          primary={qrDay.venue}
                        />
                      </ListItem>
                    </List>
                  </Grid>
                </Box>
              ))}
            </React.Fragment>
          ))}
        </CardContent>
        <CardActions className={classes.actions}></CardActions>
      </Card>
    );
  }
}

export default withStyles(styles)(QrCodeView);
