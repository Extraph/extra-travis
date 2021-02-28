import * as actionTypes from './actionTypes';

export const selectEditFrom = (editFrom) => {
  return {
    type: actionTypes.EDITFROM_SELECTED,
    payload: editFrom
  };
};

export const selectSession = (session) => {
  return {
    type: actionTypes.SESSION_SELECTED,
    payload: session
  };
};

export const selectSessionUser = (sessionUser) => {
  return {
    type: actionTypes.SESSIONUSER_SELECTED,
    payload: sessionUser
  };
};

export const selectAddDay = (day) => {
  return {
    type: actionTypes.ADDDAY_SELECTED,
    payload: day
  };
};

export const selectUserAssign = (userAssign) => {
  return {
    type: actionTypes.USERASSIGN_SELECTED,
    payload: userAssign
  };
};
