import { FETCH_DAYDASH_SUCCESS } from '../actions/actionTypes';

export default (state = [], action) => {
  switch (action.type) {
    case FETCH_DAYDASH_SUCCESS:
      return action.payload;
    default:
      return state;
  }
};
