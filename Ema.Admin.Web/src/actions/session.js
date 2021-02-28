import * as actionTypes from './actionTypes';
import adminIJoin from '../api/adminIJoin';

export const fetchSessions = (SessionId) => async (dispatch) => {
  try {
    dispatch({ type: actionTypes.BEGIN_SESSIONS_API_CALL });
    const response = await adminIJoin.post(`/Sessions`, {
      SessionId
    });
    dispatch({
      type: actionTypes.FETCH_SESSIONS_SUCCESS,
      payload: response.data
    });
  } catch (e) {
    dispatch({ type: actionTypes.SESSIONS_API_CALL_ERROR, error: e });
    throw e;
  }
};

export const updateSession = (sessionItem) => {
  return {
    type: actionTypes.SESSIONS_UPDATE,
    payload: sessionItem
  };
};

export const fetchToday = (term) => async (dispatch) => {
  try {
    dispatch({ type: actionTypes.BEGIN_TODAY_API_CALL });
    const response = await adminIJoin.post(`/Sessions/ToDayClass`, term);
    dispatch({
      type: actionTypes.FETCH_TODAY_SUCCESS,
      payload: response.data
    });
  } catch (e) {
    dispatch({ type: actionTypes.TODAY_API_CALL_ERROR, error: e });
    throw e;
  }
};

export const fetchNextSevenDay = (term) => async (dispatch) => {
  try {
    dispatch({ type: actionTypes.BEGIN_SEVENDAY_API_CALL });
    const response = await adminIJoin.post(`/Sessions/SevenDayClass`, {
      CourseId: term
    });
    dispatch({
      type: actionTypes.FETCH_SEVENDAY_SUCCESS,
      payload: response.data
    });
  } catch (e) {
    dispatch({ type: actionTypes.SEVENDAY_API_CALL_ERROR, error: e });
    throw e;
  }
};

export const fetchNextSixDayDashs = (term) => async (dispatch) => {
  try {
    dispatch({ type: actionTypes.BEGIN_DAYDASH_API_CALL });
    const response = await adminIJoin.post(`/Sessions/NextSixDayDashs`, {
      CourseId: term
    });
    dispatch({
      type: actionTypes.FETCH_DAYDASH_SUCCESS,
      payload: response.data
    });
  } catch (e) {
    dispatch({ type: actionTypes.DAYDASH_API_CALL_ERROR, error: e });
    throw e;
  }
};

export const fetchReportSessions = (SessionId) => async (dispatch) => {
  try {
    dispatch({ type: actionTypes.BEGIN_REPORT_API_CALL });
    const response = await adminIJoin.post(`/Sessions/ReportSessions`, {
      SessionId
    });
    dispatch({
      type: actionTypes.FETCH_REPORT_SUCCESS,
      payload: response.data
    });
  } catch (e) {
    dispatch({ type: actionTypes.REPORT_API_CALL_ERROR, error: e });
    throw e;
  }
};
