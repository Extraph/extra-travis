import React, { useEffect, useState } from 'react';
import {
  Box,
  Container,
  GridList,
  GridListTile,
  makeStyles
} from '@material-ui/core';
import Page from 'src/components/Page';
// import NextCard from './NextCard';
import Spinner from 'src/components/Spinner';
import Toolbar from './Toolbar';
import { useDispatch, useSelector } from 'react-redux';
import { fetchNextSevenDay } from 'src/actions/session';
import AlertMessage from 'src/components/AlertMessage';
import SessionCard from '../Card/SessionCard';
import { selectEditFrom } from 'src/actions/selected';

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

const NextClass = (props) => {
  const classes = useStyles();
  const [openSnackbar, setOpenSnackbar] = useState(false);
  const dispatch = useDispatch();
  const sevenday = useSelector((state) => state.sevenday);

  const { loading, errmsg } = useSelector((state) => state.sevendayStatus);

  const onSearchSubmit = (term) => {
    // console.log(term);
    dispatch(fetchNextSevenDay(term));
    setOpenSnackbar(true);
  };
  const handleSnackbarClose = () => {
    setOpenSnackbar(false);
  };

  useEffect(() => {
    dispatch(selectEditFrom('next'));
    dispatch(fetchNextSevenDay(''));
    setOpenSnackbar(true);
  }, [dispatch]);

  console.log(sevenday);

  return (
    <Page className={classes.root} title="Next Classes">
      <Container maxWidth={false}>
        <Toolbar onSearchSubmit={onSearchSubmit} />
        <Box mt={3}>
          {loading ? (
            <Spinner />
          ) : (
            <div className={classes.rootGridList}>
              <GridList
                className={classes.gridList}
                cellHeight={'auto'}
                cols={sevenday.length === 1 ? 1 : 1.5}
                spacing={1}
              >
                {sevenday.map((data, index) => (
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

export default NextClass;
