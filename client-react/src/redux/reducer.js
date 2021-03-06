// import multireducer from 'multireducer';
import { routerReducer } from 'react-router-redux';
import { reducer as reduxAsyncConnect } from 'redux-connect';
import { reducer as form } from 'redux-form';
import auth from './modules/auth';
import notifs from './modules/notifs';
import modal from './modules/modal';
import account from './modules/account';
import charts from './modules/charts';
// import widgets from './modules/widgets';
// import survey from './modules/survey';
// import chat from './modules/chat';

export default function createReducers(asyncReducers) {
  return {
    routing: routerReducer,
    reduxAsyncConnect,
    online: (v = true) => v,
    form,
    notifs,
    auth,
    modal,
    account,
    charts,
    // widgets,
    // survey,
    // chat,
    ...asyncReducers
  };
}
