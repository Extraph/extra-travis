import React from 'react';
import PropTypes from 'prop-types';
import { useNavigate } from 'react-router-dom';
import clsx from 'clsx';
import {
  Card,
  CardContent,
  CardMedia,
  CardActions,
  Divider,
  Typography,
  makeStyles,
  IconButton,
  Grid
} from '@material-ui/core';
import {
  GroupAdd,
  QueryBuilder,
  AccessTime,
  Event,
  // Timelapse,
  LocationOn
} from '@material-ui/icons';
import moment from 'moment';
import { selectSession } from 'src/actions/selected';
import { useDispatch } from 'react-redux';
import {
  fetchParticipant,
  fetchUserRegistration
} from 'src/actions/participant';

const useStyles = makeStyles((theme) => ({
  root: {
    display: 'flex',
    flexDirection: 'column',
    minWidth: 210,
    maxWidth: 210
  },
  statsIcon: {
    marginRight: theme.spacing(1)
  },
  media: {
    height: 300,
    width: 210
  },
  expand: {
    transform: 'rotate(0deg)',
    marginLeft: 'auto',
    transition: theme.transitions.create('transform', {
      duration: theme.transitions.duration.shortest
    })
  },
  expandOpen: {
    transform: 'rotate(180deg)'
  },
  actions: {
    justifyContent: 'flex-end'
  },
  content: {
    padding: theme.spacing(1)
  },
  icon: {
    fontSize: theme.spacing(2.5),
    margin: 3,
    color: '#F76F00'
  },
  iconAction: {
    color: theme.palette.background.dark
  },
  headline: {
    fontSize: 15,
    fontWeight: 400
  },
  footnote: {
    fontSize: 13,
    fontWeight: 400
  },
  caption1: {
    fontSize: 12,
    fontWeight: 300
  }
}));

const SessionCard = ({ className, cardData, ...rest }) => {
  const classes = useStyles();
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const handleAddClick = () => {
    dispatch(selectSession(cardData));
    dispatch(fetchParticipant({ sessionId: cardData.sessionId, userId: '' }));
    dispatch(
      fetchUserRegistration({ sessionId: cardData.sessionId, userId: '' })
    );
    navigate('/app/participant', { replace: true });
  };

  const handleCreditHourClick = () => {
    dispatch(selectSession(cardData));
    navigate('/app/credithour', { replace: true });
  };

  const { startDateTime, endDateTime } = cardData;

  const eventText =
    moment(startDateTime).format('DD/MM/YYYY') ===
    moment(endDateTime).format('DD/MM/YYYY')
      ? moment(startDateTime).format('DD/MM/YYYY')
      : `${moment(startDateTime).format('DD/MM/YYYY')} - ${moment(
          endDateTime
        ).format('DD/MM/YYYY')}`;

  return (
    <Card className={clsx(classes.root, className)} {...rest}>
      {/* <CardActionArea> */}
      <CardMedia
        className={classes.media}
        //image={'/static/images/products/ezgif-3-52499cde7dbe.png'}
        image={
          '/static/images/products/26035E25-E0B6-47A3-A617-EF53159D6559.PNG'
        }
        title=""
      />
      <CardContent className={classes.content}>
        <Typography className={classes.headline} color="textSecondary">
          {`${cardData.courseName} (${cardData.courseId})`}
        </Typography>
        <Typography className={classes.footnote} color="textSecondary">
          {`${cardData.sessionName} (${cardData.sessionId})`}
        </Typography>
        <Grid
          container
          direction="row"
          justify="flex-start"
          alignItems="center"
        >
          <Event color="primary" className={classes.icon} />
          <Typography className={classes.caption1} color="textPrimary">
            {eventText}
          </Typography>
        </Grid>

        <Grid
          container
          direction="row"
          justify="flex-start"
          alignItems="center"
        >
          <AccessTime color="primary" className={classes.icon} />
          <Typography
            className={classes.caption1}
            color="textPrimary"
          >{`${moment(cardData.startDateTime).format('hh:mmA')} - ${moment(
            cardData.endDateTime
          ).format('hh:mmA')} (${
            cardData.courseCreditHoursInit
          } hrs)`}</Typography>
        </Grid>

        <Grid
          container
          direction="row"
          justify="flex-start"
          alignItems="center"
        >
          <LocationOn color="primary" className={classes.icon} />
          <Typography className={classes.caption1} color="textPrimary">
            {cardData.venue}
          </Typography>
        </Grid>
      </CardContent>
      <Divider />
      <CardActions className={classes.actions} disableSpacing>
        <IconButton onClick={handleCreditHourClick}>
          <QueryBuilder className={classes.iconAction} />
        </IconButton>
        <IconButton onClick={handleAddClick}>
          <GroupAdd className={classes.iconAction} />
        </IconButton>
      </CardActions>
    </Card>
  );
};

SessionCard.propTypes = {
  className: PropTypes.string,
  cardData: PropTypes.object.isRequired
};

export default SessionCard;
