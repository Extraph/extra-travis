import { FETCH_USERREGIS_SUCCESS } from '../actions/actionTypes';

export default (state = [], action) => {
  switch (action.type) {
    case FETCH_USERREGIS_SUCCESS:
      return action.payload;
    default:
      return state;
  }
};
