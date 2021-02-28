import * as types from '../actions/actionTypes';

const INITIAL_STATE = {
  editFrom: '',
  session: {},
  sessionUser: {},
  addDay: 0,
  userAssign: {}
};

export default function (state = INITIAL_STATE, action) {
  switch (action.type) {
    case types.EDITFROM_SELECTED:
      return { ...state, editFrom: action.payload };
    case types.SESSION_SELECTED:
      return { ...state, session: action.payload };
    case types.SESSIONUSER_SELECTED:
      return { ...state, sessionUser: action.payload };
    case types.ADDDAY_SELECTED:
      return { ...state, addDay: action.payload };
    case types.USERASSIGN_SELECTED:
      return { ...state, userAssign: action.payload };
    default:
      return state;
  }
}
