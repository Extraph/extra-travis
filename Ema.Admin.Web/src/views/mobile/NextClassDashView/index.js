import React, { useEffect, useState } from 'react';
import {
  Container,
  Grid,
  makeStyles,
  Typography,
  Card,
  CardActionArea,
  CardActions,
  CardContent,
  Divider
} from '@material-ui/core';
import Page from 'src/components/Page';
import Spinner from 'src/components/Spinner';
import { useDispatch, useSelector } from 'react-redux';
import { fetchNextSixDayDashs } from 'src/actions/session';
import AlertMessage from 'src/components/AlertMessage';
import moment from 'moment';
import { selectAddDay } from 'src/actions/selected';
import { useNavigate } from 'react-router-dom';

const useStyles = makeStyles((theme) => ({
  root: {
    backgroundColor: theme.palette.background.dark,
    minHeight: '100%',
    paddingBottom: theme.spacing(3),
    paddingTop: theme.spacing(3),
    paddingLeft: theme.spacing(1),
    paddingRight: theme.spacing(1)
  },
  paper: {
    padding: theme.spacing(2),
    textAlign: 'center',
    color: theme.palette.text.secondary,
    minHeight: 130,
    display: 'flex',
    justifyContent: 'center',
    flexDirection: 'column'
  },
  actions: {
    justifyContent: 'flex-end'
  }
}));

const NextClassDash = (props) => {
  const classes = useStyles();
  const [openSnackbar, setOpenSnackbar] = useState(false);
  const dispatch = useDispatch();
  const daydash = useSelector((state) => state.daydash);

  const navigate = useNavigate();

  const { loading, errmsg } = useSelector((state) => state.daydashStatus);

  const handleSnackbarClose = () => {
    setOpenSnackbar(false);
  };

  const handleGoTodayAddDay = (day) => {
    console.log(day);
    dispatch(selectAddDay(day));

    navigate('/app/today');
  };

  useEffect(() => {
    // dispatch(selectEditFrom('next'));
    dispatch(fetchNextSixDayDashs(''));
    setOpenSnackbar(true);
  }, [dispatch]);

  console.log(daydash);

  return (
    <Page className={classes.root} title="Next Classes Dashboard">
      <Container maxWidth={false}>
        {loading ? (
          <Spinner />
        ) : (
          <Grid container spacing={3}>
            {daydash.map((data, index) => (
              <Grid key={index} item lg={3} sm={6} xl={3} xs={6}>
                <Card>
                  <CardActionArea
                    onClick={() => handleGoTodayAddDay(data.addDay)}
                  >
                    <CardContent className={classes.paper}>
                      <Typography color="textSecondary" variant="h1">
                        {data.sessionCount}
                      </Typography>
                    </CardContent>
                  </CardActionArea>
                  <Divider />
                  <CardActions className={classes.actions}>
                    <Typography color="textSecondary" variant="h4">
                      {moment(data.dateTime).format('D MMMM')}
                    </Typography>
                  </CardActions>
                </Card>
              </Grid>
            ))}
          </Grid>
        )}
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

export default NextClassDash;
