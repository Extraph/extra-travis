import { FETCH_TODAY_SUCCESS } from '../actions/actionTypes';

export default (state = [], action) => {
  switch (action.type) {
    case FETCH_TODAY_SUCCESS:
      return action.payload;
    default:
      return state;
  }
};
