import React, { Component } from 'react';
import PropTypes from 'prop-types';
import NavDropdown from 'react-bootstrap/lib/NavDropdown';
import Nav from 'react-bootstrap/lib/Nav';
import MenuItem from 'react-bootstrap/lib/MenuItem';

export default class AccountSelector extends Component {
  static propTypes = {
    accountName: PropTypes.string.isRequired
  };

  handleLinkClick = () => {
    console.log('Clicked');
  }

  render() {
    const { accountName } = this.props;
    return (
      <div className="account__selector clearfix">
        <div className="account__selector--link" onClick={this.handleLinkClick}>
          <i className="fa fa-arrow-left" /> {accountName}
        </div>
        <div>
          <Nav className="account__selector--nav">
            <NavDropdown title="" id="accountList">
              <MenuItem eventKey={1}>RRSP Employer</MenuItem>
              <MenuItem eventKey={2}>RRSP Personal</MenuItem>
            </NavDropdown>
        </Nav>
        </div>
      </div>
    );
  }
}
