import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { ContentBlock } from 'components';
import { AccountItem } from 'components/Accounts';

@connect(
  state => ({
    user: state.auth.user
  }),
  { })
export default class AccountsList extends Component {
  static propTypes = {
    user: PropTypes.object
  };

  static defaultProps = {
    user: null
  };

  render() {
    return (
      <ContentBlock>
        <div className="pd-30 btm-content-box">
          <h2>Your Accounts</h2>

          {[1, 2].map((item, index) => (
            <AccountItem key={`item-${index}`} />
          ))}

          <div className="view-toggler-box border-top">
            <a>+ ADD ANOTHER ACCOUNT</a>
          </div>
        </div>
      </ContentBlock>
    );
  }
}

