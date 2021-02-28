import React, { useEffect, useState } from 'react';
import {
  Box,
  Container,
  GridList,
  GridListTile,
  makeStyles,
  Typography
} from '@material-ui/core';
import Page from 'src/components/Page';
// import TodayCard from './TodayCard';
import Spinner from 'src/components/Spinner';
import Toolbar from './Toolbar';
import { useDispatch, useSelector } from 'react-redux';
import { fetchToday } from 'src/actions/session';
import AlertMessage from 'src/components/AlertMessage';
import SessionCard from '../Card/SessionCard';
import { selectEditFrom } from 'src/actions/selected';
import moment from 'moment';

const useStyles = makeStyles((theme) => ({
  root: {
    backgroundColor: theme.palette.background.dark,
    minHeight: '100%',
    paddingBottom: theme.spacing(3),
    paddingTop: theme.spacing(3)
  },
  rootGridList: {
    display: 'flex',
    flexWrap: 'wrap',
    justifyContent: 'flex-start',
    overflow: 'hidden',
    backgroundColor: theme.palette.background.dark
  },
  gridList: {
    flexWrap: 'nowrap',
    transform: 'translateZ(0)'
  },
  productCard: {
    height: '100%'
  }
}));

const TodayClass = (props) => {
  const classes = useStyles();
  const [openSnackbar, setOpenSnackbar] = useState(false);
  const dispatch = useDispatch();
  const today = useSelector((state) => state.today);

  const { loading, errmsg } = useSelector((state) => state.todayStatus);

  const addDay = useSelector((state) => state.selected.addDay);

  const onSearchSubmit = (term) => {
    console.log(addDay);
    dispatch(
      fetchToday({
        CourseId: term,
        AddDay: addDay
      })
    );
    setOpenSnackbar(true);
  };
  const handleSnackbarClose = () => {
    setOpenSnackbar(false);
  };

  useEffect(() => {
    dispatch(selectEditFrom('today'));
    console.log(addDay);
    dispatch(
      fetchToday({
        CourseId: '',
        AddDay: addDay
      })
    );
    setOpenSnackbar(true);
  }, [addDay, dispatch]);

  console.log(today);

  return (
    <Page className={classes.root} title="Today Class">
      <Container maxWidth={false}>
        <Toolbar onSearchSubmit={onSearchSubmit} />
        <Box mt={3}>
          <Typography color="textSecondary" variant="h4">
            {moment().add(addDay, 'days').format('D MMMM yyyy')}
          </Typography>
        </Box>
        <Box mt={3}>
          {loading ? (
            <Spinner />
          ) : (
            <div className={classes.rootGridList}>
              <GridList
                className={classes.gridList}
                cellHeight={'auto'}
                cols={today.length === 1 ? 1 : 1.5}
                spacing={1}
              >
                {today.map((data, index) => (
                  <GridListTile key={index}>
                    <SessionCard
                      className={classes.productCard}
                      cardData={data}
                    />
                  </GridListTile>
                ))}
              </GridList>
            </div>
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

export default TodayClass;
