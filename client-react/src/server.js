import Express from 'express';
import React from 'react';
import ReactDOM from 'react-dom/server';
import config from './config';
import favicon from 'serve-favicon';
import compression from 'compression';
import httpProxy from 'http-proxy';
import path from 'path';
import ApiClient from './helpers/ApiClient';
import Html from './helpers/Html';
import PrettyError from 'pretty-error';
import http from 'http';

//import { match } from 'react-router';
//import { syncHistoryWithStore } from 'react-router-redux';
//import { ReduxAsyncConnect, loadOnServer } from 'redux-async-connect';
//import createHistory from 'react-router/lib/createMemoryHistory';
//import {Provider} from 'react-redux';
//import getRoutes from './routes';

const targetUrl = 'http://' + config.apiHost + ':' + config.apiPort;
const pretty = new PrettyError();
const app = new Express();
const server = new http.Server(app);
const proxy = httpProxy.createProxyServer({
  target: targetUrl,
  ws: true
});

app.use(compression());
app.use(favicon(path.join(__dirname, '..', 'static', 'favicon.ico')));

app.use(Express.static(path.join(__dirname, '..', 'static')));

// Proxy to API server
app.use('/api', (req, resp) => {
  //proxy.web(req, res, {target: targetUrl});  
  http.get(targetUrl, (res) => {
    const { statusCode } = res;
    const contentType = res.headers['content-type'];

    let error;
    let raw;
    if (statusCode !== 200) {
      error = new Error(`Request Failed.\n` +
                        `Status Code: ${statusCode}`);
    } else if (!/^application\/json/.test(contentType)) {
      error = new Error(`Invalid content-type.\n` +
                        `Expected application/json but received ${contentType}`);
    }
    if (error) {
      console.error(error.message);
      // consume response data to free up memory
      res.resume();
      raw = error.message;
    }

    res.setEncoding('utf8');
    let rawData = '';
    res.on('data', (chunk) => { rawData += chunk; });
    res.on('end', () => {
      try {
        //const parsedData = JSON.parse(rawData);
        raw = rawData;
      } catch (e) {
        console.error(e.message);
        raw = e.message;
      }
      resp.send(raw);
    });
  }).on('error', (e) => {
    console.error(`Got error: ${e.message}`);
    resp.send(e.message);
  }); 
});

app.use('/ws', (req, res) => {
  proxy.web(req, res, {target: targetUrl + '/ws'});
});

server.on('upgrade', (req, socket, head) => {
  proxy.ws(req, socket, head);
});

// added the error handling to avoid https://github.com/nodejitsu/node-http-proxy/issues/527
proxy.on('error', (error, req, res) => {
  let json;
  if (error.code !== 'ECONNRESET') {
    console.error('proxy error', error);
  }
  if (!res.headersSent) {
    res.writeHead(500, {'content-type': 'application/json'});
  }

  json = {error: 'proxy_error', reason: error.message};
  res.end(JSON.stringify(json));
});

app.use((req, res) => {
  if (__DEVELOPMENT__) {
    // Do not cache webpack stats: the script file would change since
    // hot module replacement is enabled in the development env
    webpackIsomorphicTools.refresh();
  }
  const client = new ApiClient(req);
  //const memoryHistory = createHistory(req.originalUrl);
  //const store = createStore(memoryHistory, client);
  //const history = syncHistoryWithStore(memoryHistory, store);

  function hydrateOnClient() {
    res.send('<!doctype html>\n' +
      //ReactDOM.renderToString(<Html assets={webpackIsomorphicTools.assets()} store={store}/>));
      //ReactDOM.renderToString(<Html assets={webpackIsomorphicTools.assets()}><div>Hydrated on client</div></Html>));
      '<html><title>Client react</title>' +
      '<body><h1>Client react (' + targetUrl +')</h1></body></html>'
    );
  }

  if (__DISABLE_SSR__) {
    hydrateOnClient();
    return;
  }
});

if (config.port) {
  server.listen(config.port, (err) => {
    if (err) {
      console.error(err);
    }
    console.info('----\n==> ✅  %s is running, talking to API server on %s.', config.app.title, config.apiPort);
    console.info('==> 💻  Open http://%s:%s in a browser to view the app.', config.host, config.port);
  });
} else {
  console.error('==>     ERROR: No PORT environment variable has been specified');
}
