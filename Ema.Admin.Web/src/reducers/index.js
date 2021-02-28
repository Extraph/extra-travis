import { combineReducers } from 'redux';
import authReducer from './authReducer';
import authStatusReducer from './authStatusReducer';
import session from './session';
import sessionStatus from './sessionStatus';
import participant from './participant';
import participantStatus from './participantStatus';
import userRegis from './userRegis';
import userRegisStatus from './userRegisStatus';
import selectedReducer from './selectedReducer';
import today from './today';
import todayStatus from './todayStatus';
import sevenday from './sevenday';
import sevendayStatus from './sevendayStatus';
import daydash from './daydash';
import daydashStatus from './daydashStatus';
import report from './report';
import reportStatus from './reportStatus';

export default combineReducers({
  auth: authReducer,
  authStatus: authStatusReducer,
  session,
  sessionStatus,
  participant,
  participantStatus,
  userRegis,
  userRegisStatus,
  selected: selectedReducer,
  today,
  todayStatus,
  sevenday,
  sevendayStatus,
  daydash,
  daydashStatus,
  report,
  reportStatus
});
