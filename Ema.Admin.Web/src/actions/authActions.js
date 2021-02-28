import axios from 'axios';
import { AUTH_BEGIN, AUTH_USER, AUTH_ERROR } from './actionTypes';

export const signin = (loginTerm, callback) => async (dispatch) => {
  try {
    dispatch({ type: AUTH_BEGIN });

    const response = await axios.post(
      `${process.env.REACT_APP_ADMIN_IJOIN_BASE_URL}/authen/authenticate`,
      loginTerm
    );

    if (response.data.isauthen) {
      const { token } = response.data.response;

      dispatch({ type: AUTH_USER, payload: response.data.response });

      console.log('JWT Token From Server : ' + token);

      localStorage.setItem('token', token);
      callback();
    } else dispatch({ type: AUTH_ERROR, payload: response.data.message });
  } catch (e) {
    dispatch({ type: AUTH_ERROR, payload: e.message });
  }
};

export const signout = () => {
  localStorage.removeItem('token');

  return {
    type: AUTH_USER,
    payload: ''
  };
};
