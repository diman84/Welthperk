const VALUE_LOADING = 'redux-example/account/VALUE_LOADING';
const VALUE_LOADED = 'redux-example/account/VALUE_LOADED';
const VALUE_FAIL = 'redux-example/account/VALUE_FAIL';
const SETTINGS_LOADING = 'redux-example/account/SETTINGS_LOADING';
const SETTINGS_LOADED = 'redux-example/account/SETTINGS_LOADED';
const SETTINGS_FAIL = 'redux-example/account/SETTINGS_FAIL';
const defaultError = 'Failed to load data';

const initialState = {
  value: {loading: true},
  settings: {
    loading: true,
    risksProfile: {},
    contribution: {}
  }
 };

export default function reducer(state = initialState, action = {}) {
  switch (action.type) {
    case VALUE_LOADING:
      return {
        ...state,
        value: {
          loading: true,
          loaded: false
        }
      };
    case VALUE_LOADED:
      return {
        ...state,
        value: {
           ...action.result,
          loading: false,
          loaded: true
        }
      };
    case VALUE_FAIL:
      return {
        ...state,
        value: {
          loading: false,
          loaded: false,
          loadError: action.error.message || defaultError
        }
      };
    case SETTINGS_LOADING:
      return {
        ...state,
        settings: {
          loading: true,
          loaded: false
        }
      };
    case SETTINGS_LOADED:
      return {
        ...state,
        settings: {
           ...action.result,
          loading: false,
          loaded: true
        }
      };
    case SETTINGS_FAIL:
      return {
        ...state,
        settings: {
          loading: false,
          loaded: false,
          loadError: action.error.message || defaultError
        }
      };
    default:
      return state;
  }
}

/*
* Actions
* * * * */
export function loadValues() {
  return {
    types: [VALUE_LOADING, VALUE_LOADED, VALUE_FAIL],
    promise: ({ client }) =>
      client.get('/account/values')
            .catch(error => Promise.reject(error))
    };
}

export function loadSettings() {
  return {
    types: [SETTINGS_LOADING, SETTINGS_LOADED, SETTINGS_FAIL],
    promise: ({ client }) =>
      client.get('/account/settings')
            .catch(error => Promise.reject(error))
    };
}
