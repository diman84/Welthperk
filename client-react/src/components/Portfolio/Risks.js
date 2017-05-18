import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { ContentBlock } from 'components';

@connect(
  state => ({
    user: state.auth.user
  }),
  { })
export default class Transactions extends Component {
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
          <div className="risks__header">
            <h2>Risk Profile</h2>
            <p>Aggressive Growth</p>
          </div>

          <div className="risks__content">
            <h2>Portfolio Fee</h2>
            <p>0.20%</p>
          </div>

          <div className="view-toggler-box border-top">
            <a>CHANGE ALLOCATION</a>
          </div>
        </div>
      </ContentBlock>
    );
  }
}

