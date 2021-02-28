import * as types from '../actions/actionTypes';

export default (state = { loading: 0, errmsg: '' }, action) => {
  switch (action.type) {
    case types.BEGIN_TODAY_API_CALL:
      return { loading: state.loading + 1, errmsg: '' };
    case types.TODAY_API_CALL_ERROR:
      return { loading: state.loading - 1, errmsg: action.error };
    case types.FETCH_TODAY_SUCCESS:
      return { loading: state.loading - 1, errmsg: '' };
    default:
      return state;
  }
};
