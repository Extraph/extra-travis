import { FETCH_SEVENDAY_SUCCESS } from '../actions/actionTypes';

export default (state = [], action) => {
  switch (action.type) {
    case FETCH_SEVENDAY_SUCCESS:
      return action.payload;
    default:
      return state;
  }
};
