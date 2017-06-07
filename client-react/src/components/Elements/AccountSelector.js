import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import NavDropdown from 'react-bootstrap/lib/NavDropdown';
import Nav from 'react-bootstrap/lib/Nav';
import MenuItem from 'react-bootstrap/lib/MenuItem';
import { Link } from 'react-router';

@connect(
  state => ({
    currentAccount: state.account.current,
    user: state.auth.user
  }))

export default class AccountSelector extends Component {
  static propTypes = {
    currentAccount: PropTypes.object,
    user: PropTypes.object
  };

  static defaultProps = {
    currentAccount: null,
    user: null
  };

  render() {
    const { currentAccount, user } = this.props;
    const visibleAccounts = user && user.accounts
                ? user.accounts.filter((item) => { return !currentAccount || item.id !== currentAccount.id; })
                : [];
    return (
      <div className="account__selector clearfix">
          <div className="account__selector--link">
          <Link to="/portfolio">
            <i className="fa fa-arrow-left"></i>
          </Link>
           {currentAccount ? currentAccount.name : 'Select account'}
          </div>

        <div>
          {visibleAccounts.length > 0 &&
          <Nav className="account__selector--nav">
            <NavDropdown title="" id="accountList">
              {visibleAccounts.map((item) => (
                <MenuItem>
                 <Link to={'/accounts/' + item.id}>
                  {item.name}
                  </Link>
                </MenuItem>
              ))}
            </NavDropdown>
        </Nav>}
        </div>
      </div>
    );
  }
}
