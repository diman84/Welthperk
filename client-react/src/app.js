import React, { Component } from 'react';
import PropTypes from 'prop-types';
import feathers from 'feathers/client';
import hooks from 'feathers-hooks';
import rest from 'feathers-rest/client';
import authentication from 'feathers-authentication-client';
import superagent from 'superagent';
import config from './config';

const storage = __SERVER__ ? require('localstorage-memory') : require('localforage');

const host = clientUrl => (__SERVER__ ? `http://${config.apiHost}:${config.apiPort}` : clientUrl);

const configureApp = transport => feathers()
  .configure(transport)
  .configure(hooks())
  .configure(authentication({ storage, path: '/auth/login' }));

const customizeAuthRequest = () =>
   hook => {
    const { strategy, ...data } = hook.data;
    hook.data = data;
    hook.params.headers = { 'Content-Type': 'application/x-www-form-urlencoded', ...hook.params.headers };
    return Promise.resolve(hook);
  };

export function createApp(req) {
  if (req === 'rest') {
      const feathersApp = configureApp(rest(host('/api')).superagent(superagent));
      feathersApp.service('auth/login').hooks({
        before: {
              create: [customizeAuthRequest()]
            }
          });
      return feathersApp;
  }
//if (__SERVER__ && req) {
    const app = configureApp(rest(host('/api')).superagent(superagent, {
      headers: {
        Cookie: req.get('cookie'),
        authorization: req.header('authorization')
      }
    }));

    const accessToken = req.header('authorization') || (req.cookies && req.cookies['feathers-jwt']);
    app.set('accessToken', accessToken);

    return app;
  }

export function withApp(WrappedComponent) {
  class WithAppComponent extends Component {
    static contextTypes = {
      app: PropTypes.object.isRequired,
      restApp: PropTypes.object.isRequired,
    }

    render() {
      const { restApp } = this.context;
      return <WrappedComponent {...this.props} restApp={restApp} />;
    }
  }

  return WithAppComponent;
}
