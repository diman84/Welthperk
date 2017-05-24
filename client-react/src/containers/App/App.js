import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { IndexLink } from 'react-router';
import { LinkContainer } from 'react-router-bootstrap';
import Navbar from 'react-bootstrap/lib/Navbar';
import Nav from 'react-bootstrap/lib/Nav';
import NavItem from 'react-bootstrap/lib/NavItem';
import Alert from 'react-bootstrap/lib/Alert';
import Helmet from 'react-helmet';
import { isLoaded as isAuthLoaded, load as loadAuth, logout } from 'redux/modules/auth';
import { Notifs, Footer } from 'components';
import { push } from 'react-router-redux';
import config from 'config';
import { asyncConnect } from 'redux-connect';

@asyncConnect([{
  promise: ({ store: { dispatch, getState } }) => {
    const promises = [];

    if (!isAuthLoaded(getState())) {
      promises.push(dispatch(loadAuth()));
    }
    return Promise.all(promises);
  }
}])
@connect(
  state => ({
    notifs: state.notifs,
    user: state.auth.user
  }),
  { logout, pushState: push })
export default class App extends Component {
  static propTypes = {
    children: PropTypes.object.isRequired,
    router: PropTypes.object.isRequired,
    user: PropTypes.object,
    notifs: PropTypes.object.isRequired,
    logout: PropTypes.func.isRequired,
    pushState: PropTypes.func.isRequired
  };

  static defaultProps = {
    user: null
  };

  static contextTypes = {
    store: PropTypes.object.isRequired
  };

  state = {
    offsetY: 0
  };

  componentDidMount() {
    window.addEventListener('scroll', this.runOnScroll);
  }

  componentWillReceiveProps(nextProps) {
    if (!this.props.user && nextProps.user) {
      // login
      const redirect = this.props.router.location.query && this.props.router.location.query.redirect;
      this.props.pushState(redirect || '/portfolio');
    } else if (this.props.user && !nextProps.user) {
      // logout
      this.props.pushState('/');
    }
  }

  componentWillUnmount() {
    window.removeEventListener('scroll', this.runOnScroll);
  }

  runOnScroll = () => {
    const pageYOffset = window.scrollY;
    this.setState({ offsetY: pageYOffset });
  };

  handleLogout = event => {
    event.preventDefault();
    this.props.logout();
  };

  render() {
    const { user, notifs, children } = this.props;
    const { offsetY } = this.state;
    const pathname = this.props.router.location.pathname;
    let navClass = '';
    let wrapperStyle = { paddingTop: '0px' };
    if (pathname === '/') {
      navClass = offsetY < 30 ? 'navbar-transparent' : '';
    } else {
      navClass = 'navbar-black';
      wrapperStyle = { paddingTop: '80px' };
    }
    const styles = require('./App.scss');

    return (
      <div className={styles.app} style={wrapperStyle}>
        <Helmet
          {...config.app.head}
        />
        <Navbar fixedTop className={navClass}>
          <Navbar.Header>
            <Navbar.Brand>
              <IndexLink to="/">
                <span style={{ fontWeight: 500 }}>{config.app.title}</span>
                {pathname === '/' && <i className="material-icons icon icon-success">favorite</i>}
              </IndexLink>
            </Navbar.Brand>
            <Navbar.Toggle />
          </Navbar.Header>

          <Navbar.Collapse>
            <Nav navbar pullRight>
              {user && <LinkContainer to="/accounts">
                <NavItem>ACCOUNTS</NavItem>
              </LinkContainer>}
              {user && <LinkContainer to="/portfolio">
                <NavItem>PORTFOLIO</NavItem>
              </LinkContainer>}
              {!user && <LinkContainer to="/accounts">
                <NavItem>HELP</NavItem>
              </LinkContainer>}

              {!user && <LinkContainer to="/login">
                <NavItem>LOG IN</NavItem>
              </LinkContainer>}
              {user && <LinkContainer to="/logout">
                <NavItem eventKey={7} onClick={this.handleLogout}>LOG OUT</NavItem>
              </LinkContainer>}
            </Nav>
          </Navbar.Collapse>
        </Navbar>

        <div className={styles.appContent}>
          {notifs.global && <div className="container">
            <Notifs
              className={styles.notifs}
              namespace="global"
              NotifComponent={props => <Alert bsStyle={props.kind}>{props.message}</Alert>}
            />
          </div>}

          {children}
        </div>

        <Footer />

      </div>
    );
  }
}
