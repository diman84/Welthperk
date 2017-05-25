const CONTACT_SENDING = 'redux-example/contact/CONTACT_SENDING';
const CONTACT_SENT  = 'redux-example/notifs/CONTACT_SENT';
const CONTACT_FAIL  = 'redux-example/notifs/CONTACT_FAIL';

const initialState = {};

export default function reducer(state = initialState, action = {}) {
  switch (action.type) {
    case CONTACT_SENDING:
      return {
        ...state,
        sending: true
      };
    case CONTACT_SENT:
      return {
        ...state,
        sending: false
      };
    case CONTACT_FAIL:
      return {
        ...state,
        sendError: action.error
      };
    default:
      return state;
  }
}

export function sendContact(data, validation = true) {
  return {
    types: [CONTACT_SENDING, CONTACT_SENT, CONTACT_FAIL],
    promise: ({ client }) => client.post({
      ...data
    })
      .then(resp => {})
      .catch(error => Promise.reject(error))
  };
}
