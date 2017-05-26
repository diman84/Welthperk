const VALUE_LOADING = 'redux-example/account/VALUE_LOADING';
const VALUE_LOADED = 'redux-example/account/VALUE_LOADED';
const VALUE_FAIL = 'redux-example/account/VALUE_FAIL';

const initialState = { value: {loading: true} };

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
          loadError: action.error.message | 'Values won`t load',
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
