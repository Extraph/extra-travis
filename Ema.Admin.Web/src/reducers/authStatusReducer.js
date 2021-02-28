import * as types from '../actions/actionTypes';

export default (state = { loading: 0, errmsg: '' }, action) => {
  switch (action.type) {
    case types.AUTH_BEGIN:
      return { loading: 1, errmsg: '' };
    case types.AUTH_ERROR:
      return { loading: 0, errmsg: action.payload };
    case types.AUTH_USER:
      return { loading: 0, errmsg: '' };
    default:
      return state;
  }
};
