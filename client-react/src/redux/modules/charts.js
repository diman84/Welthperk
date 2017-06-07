const FUTURE_YOU_LOADING = 'redux-example/account/FUTURE_YOU_LOADING';
const FUTURE_YOU_LOADED = 'redux-example/account/FUTURE_YOU_LOADED';
const FUTURE_YOU_FAIL = 'redux-example/account/FUTURE_YOU_FAIL';
const defaultError = 'Failed to load data';

const initialState = {
  futureYou: {loading: true}
 };

export default function reducer(state = initialState, action = {}) {
  switch (action.type) {
    case FUTURE_YOU_LOADING:
      return {
        ...state,
        loading: true,
        loaded: false
      };
    case FUTURE_YOU_LOADED:
      return {
        ...state,
        loading: false,
        loaded: true,
        futureYou: {
           ...action.result
        }
      };
    case FUTURE_YOU_FAIL:
      return {
        ...state,
        loading: false,
        loaded: false,
        loadError: action.error.message || defaultError
      };
    default:
      return state;
  }
}

/*
* Actions
* * * * */
export function loadForecast() {
  return {
    types: [FUTURE_YOU_LOADING, FUTURE_YOU_LOADED, FUTURE_YOU_FAIL],
    promise: ({ client }) =>
      client.get('/account/forecast')
            .catch(error => Promise.reject(error))
    };
}
