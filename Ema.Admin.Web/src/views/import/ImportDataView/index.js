import React from 'react';
import { Container, makeStyles } from '@material-ui/core';
import Page from 'src/components/Page';
import KlcFileUpload from './KlcFileUpload';
// import Password from './Password';

const useStyles = makeStyles((theme) => ({
  root: {
    backgroundColor: theme.palette.background.dark,
    minHeight: '100%',
    paddingBottom: theme.spacing(3),
    paddingTop: theme.spacing(3)
  }
}));

const ImportDataView = () => {
  const classes = useStyles();

  return (
    <Page className={classes.root} title="Import Data">
      <Container maxWidth="xl">
        <KlcFileUpload />
        {/* <Box mt={3}>
          <Password />
        </Box> */}
      </Container>
    </Page>
  );
};

export default ImportDataView;
