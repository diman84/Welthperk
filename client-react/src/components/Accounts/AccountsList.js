import React, { Component } from 'react';
//import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { ContentBlock } from 'components';
import { AccountItem } from 'components/Accounts';
import ModalLink from 'containers/Modal/ModalLink';

@connect(
  state => ({
    ...state.account.accounts
   }))

export default class AccountsList extends Component {

  render() {
    return (
      <ContentBlock>
        <div className="pd-30 btm-content-box">
          <h2>Your Accounts</h2>

          {[1, 2].map((item, index) => (
            <AccountItem key={`item-${index}`} />
          ))}

          <div className="view-toggler-box border-top">
            <ModalLink action="ADD_ACCOUNT" title="+ ADD ANOTHER ACCOUNT"></ModalLink>
          </div>
        </div>
      </ContentBlock>
    );
  }
}

