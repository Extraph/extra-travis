import * as actionTypes from './actionTypes';
import adminIJoin from '../api/adminIJoin';
import userIJoin from '../api/userIJoin';

export const fetchParticipant = (term) => async (dispatch) => {
  try {
    dispatch({ type: actionTypes.BEGIN_PARTICIPANT_API_CALL });
    const response = await adminIJoin.post('/Participant', term);
    dispatch({
      type: actionTypes.FETCH_PARTICIPANT_SUCCESS,
      payload: response.data
    });
  } catch (e) {
    dispatch({ type: actionTypes.PARTICIPANT_API_CALL_ERROR, error: e });
    throw e;
  }
};

export const fetchUserRegistration = (term) => async (dispatch) => {
  try {
    dispatch({ type: actionTypes.BEGIN_USERREGIS_API_CALL });
    const response = await userIJoin.post(`/UserRegistration`, term);
    dispatch({
      type: actionTypes.FETCH_USERREGIS_SUCCESS,
      payload: response.data
    });
  } catch (e) {
    dispatch({ type: actionTypes.USERREGIS_API_CALL_ERROR, error: e });
    throw e;
  }
};
