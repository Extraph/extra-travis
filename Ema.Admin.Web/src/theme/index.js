import { createMuiTheme } from '@material-ui/core';
// import shadows from './shadows';
import typography from './typography';

const theme = createMuiTheme({
  palette: {
    background: {
      dark: '#222222',
      paper: '#5A5A5A'
    },
    primary: {
      main: '#FFD400'
    },
    secondary: {
      main: '#5A5A5A'
    },
    success: {
      main: '#319D19'
    },
    error: {
      main: '#D00000'
    },
    type: 'dark'
  },
  // shadows,
  typography
});

export default theme;
