import {
  FETCH_SESSIONS_SUCCESS,
  SESSIONS_UPDATE
} from '../actions/actionTypes';

export default (state = [], action) => {
  switch (action.type) {
    case FETCH_SESSIONS_SUCCESS:
      return action.payload;
    case SESSIONS_UPDATE:
      return state.map((session) =>
        session.sessionId === action.payload.sessionId
          ? action.payload
          : session
      );
    default:
      return state;
  }
};
