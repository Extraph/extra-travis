import { AUTH_USER } from '../actions/actionTypes';

const INITIAL_STATE = {
  authenticated: ''
};

const role = [
  { roleId: 1, roleName: 'Super Admin' },
  { roleId: 2, roleName: 'Admin' },
  { roleId: 3, roleName: 'Report Admin' },
  { roleId: 4, roleName: 'Instructor' }
];

export default function (state = INITIAL_STATE, action) {
  switch (action.type) {
    case AUTH_USER:
      if (action.payload.roleId)
        return {
          ...state,
          authenticated: {
            ...action.payload,
            roleName: role.find((f) => f.roleId === action.payload.roleId)
              .roleName
          }
        };
      else return { ...state, authenticated: action.payload };
    default:
      return state;
  }
}
