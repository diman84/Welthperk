require('babel-polyfill');

const environment = {
  development: {
    isProduction: false
  },
  production: {
    isProduction: true
  }
}[process.env.NODE_ENV || 'development'];

module.exports = Object.assign({
  host: process.env.HOST || 'localhost',
  port: process.env.PORT,
  apiHost: process.env.APIHOST || 'localhost',
  apiPort: process.env.APIPORT,
  app: {
    title: 'Wealthperk',
    description: 'Wealthperk.',
    head: {
      titleTemplate: '%s | Wealthperk',
      meta: [
        { name: 'description', content: 'Wealthperk.' },
        { charset: 'utf-8' },
        { property: 'og:site_name', content: 'Wealthperk' },
        { property: 'og:locale', content: 'en_US' },
        { property: 'og:title', content: 'Wealthperk' },
        { property: 'og:description', content: 'Wealthperk.' },
      ]
    }
  }
}, environment);
