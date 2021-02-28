import { FETCH_PARTICIPANT_SUCCESS } from '../actions/actionTypes';
// const initState = {
//   checkInNumber: 0,
//   checkOutNumber: 0,
//   data: []
// };
export default (state = [], action) => {
  switch (action.type) {
    case FETCH_PARTICIPANT_SUCCESS:
      return action.payload;
    default:
      return state;
  }
};
