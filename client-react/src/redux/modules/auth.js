import { SubmissionError } from 'redux-form';
import cookie from 'js-cookie';
//const jwtDecode = require('jwt-decode');
const LOAD = 'redux-example/auth/LOAD';
const LOAD_SUCCESS = 'redux-example/auth/LOAD_SUCCESS';
const LOAD_FAIL = 'redux-example/auth/LOAD_FAIL';
const LOGIN = 'redux-example/auth/LOGIN';
const LOGIN_SUCCESS = 'redux-example/auth/LOGIN_SUCCESS';
const LOGIN_FAIL = 'redux-example/auth/LOGIN_FAIL';
const REGISTER = 'redux-example/auth/REGISTER';
const REGISTER_SUCCESS = 'redux-example/auth/REGISTER_SUCCESS';
const REGISTER_FAIL = 'redux-example/auth/REGISTER_FAIL';
const LOGOUT = 'redux-example/auth/LOGOUT';
const LOGOUT_SUCCESS = 'redux-example/auth/LOGOUT_SUCCESS';
const LOGOUT_FAIL = 'redux-example/auth/LOGOUT_FAIL';

const initialState = {
  loaded: false
};

export default function reducer(state = initialState, action = {}) {
  switch (action.type) {
    case LOAD:
      return {
        ...state,
        loading: true
      };
    case LOAD_SUCCESS:
      return {
        ...state,
        loading: false,
        loaded: true,
        accessToken: action.result.accessToken,
        user: action.result.user
      };
    case LOAD_FAIL:
      return {
        ...state,
        loading: false,
        loaded: false,
        error: action.error
      };
    case LOGIN:
      return {
        ...state,
        loggingIn: true
      };
    case LOGIN_SUCCESS:
      return {
        ...state,
        loggingIn: false,
        accessToken: action.result.access_token,
        user: action.result.user
      };
    case LOGIN_FAIL:
      return {
        ...state,
        loggingIn: false,
        loginError: action.error
      };
    case REGISTER:
      return {
        ...state,
        registeringIn: true
      };
    case REGISTER_SUCCESS:
      return {
        ...state,
        registeringIn: false
      };
    case REGISTER_FAIL:
      return {
        ...state,
        registeringIn: false,
        registerError: action.error
      };
    case LOGOUT:
      return {
        ...state,
        loggingOut: true
      };
    case LOGOUT_SUCCESS:
      return {
        ...state,
        loggingOut: false,
        accessToken: null,
        user: null
      };
    case LOGOUT_FAIL:
      return {
        ...state,
        loggingOut: false,
        logoutError: action.error
      };
    default:
      return state;
  }
}

const catchValidation = error => {
  if (error.message) {
    if (error.message === 'Validation failed' && error.data) {
      throw new SubmissionError(error.data);
    }
    throw new SubmissionError({ _error: error.message });
  }
  return Promise.reject(error);
};

function setToken({ client, restApp }) {
  return response => {
    const { access_token, refresh_token } = response;

    // set manually the JWT for both instances of feathers/client
    restApp.set('accessToken', access_token);
    restApp.set('refreshToken', refresh_token);
    restApp.passport.setJWT(access_token);
    client.setJwtToken(access_token);

    return response;
  };
}

function setCookie({ restApp }) {
  return response => {
    const options = response.expires_in ? { expires: response.expires_in / (60 * 60 * 24) } : undefined;
    cookie.set('feathers-jwt', restApp.get('accessToken'), options);
    cookie.set('refreshToken', restApp.get('refreshToken'), options);
    return response;
  };
}

/*
* Actions
* * * * */

export function isLoaded(globalState) {
  return globalState.auth && globalState.auth.loaded;
}

export function load() {
  return {
    types: [LOAD, LOAD_SUCCESS, LOAD_FAIL],
    promise: ({ client, restApp }) => restApp.authenticate({
      grant_type: 'refresh_token',
      strategy: 'local',
      refresh_token: restApp.get('refreshToken') || cookie.get('refreshToken')
    }).then(setToken({ client, restApp }))
      .then(setCookie({ restApp }))
      .then(response => {
        const { access_token } = response;
        return restApp.passport.verifyJWT(access_token);
      })
      .then(payload => {
        restApp.set('user', { email: payload.name });
        return payload;
      })
      .then(() => {
        return {
            access_token: restApp.get('accessToken'),
            user: restApp.get('user')
          };
      })
      .catch(error => Promise.reject(error))
  };
}

export function register(data) {
  return {
    types: [REGISTER, REGISTER_SUCCESS, REGISTER_FAIL],
    promise: ({ restApp }) => restApp.service('users').create(data).catch(catchValidation)
  };
}

export function login(strategy, data, validation = true) {
  //const socketId = socket.io.engine.id;
  return {
    types: [LOGIN, LOGIN_SUCCESS, LOGIN_FAIL],
    promise: ({ client, restApp }) => restApp.authenticate({
      ...data,
      strategy,
      grant_type: 'password'
    //  socketId
    })
      .then(setToken({ client, restApp }))
      .then(setCookie({ restApp }))
      /*.then(response => {
        const { access_token } = response;
        console.log('Authenticated!', response);
        return restApp.passport.verifyJWT(access_token);
      })
      .then(payload => {
        console.log('JWT Payload', payload);
        //return restApp.service('users').get(payload.id_token);
        return jwtDecode(payload.id_token);
      })*/
      .then(response => {
        //console.log('JWT Payload', payload);
        //return restApp.service('users').get(response.id_token);
        const { access_token } = response;
        return restApp.passport.verifyJWT(access_token);
      })
      .then(payload => {
        //restApp.set('user', response.user);
        //return response;
        restApp.set('user', { email: payload.name });
        return payload;
      })
      .then(() => {
        return {
            access_token: restApp.get('accessToken'),
            user: restApp.get('user')
          };
      })
      .catch(validation ? catchValidation : error => Promise.reject(error))
  };
}

export function logout() {
  return {
    types: [LOGOUT, LOGOUT_SUCCESS, LOGOUT_FAIL],
    promise: ({ client, restApp }) => restApp.logout()
      .then(() => setToken({ client, restApp })({ access_token: null, refresh_token: null }))
      .then(setCookie({ restApp }))
  };
}
