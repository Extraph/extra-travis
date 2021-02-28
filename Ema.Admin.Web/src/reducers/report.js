import { FETCH_REPORT_SUCCESS } from '../actions/actionTypes';

export default (state = [], action) => {
  switch (action.type) {
    case FETCH_REPORT_SUCCESS:
      return action.payload;
    default:
      return state;
  }
};
