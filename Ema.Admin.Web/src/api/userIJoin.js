import axios from 'axios';

const instance = axios.create({
  baseURL: process.env.REACT_APP_USER_IJOIN_BASE_URL
});

instance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (err) => {
    return Promise.reject(err);
  }
);

export default instance;
