import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { ContentBlock } from 'components';

@connect(
  state => ({
    user: state.auth.user
  }),
  { })
export default class Risks extends Component {
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
          <div style={{ marginBottom: '48px' }}>
            <h2>Risk Profile</h2>
            <p className="h3">Aggressive Growth</p>
            <p>100% Growth Assets (Stocks)</p>
            <p>0% Defensive Assets (Bonds)</p>
          </div>

          <div style={{ marginBottom: '24px' }}>
            <h2>Portfolio Fee</h2>
            <p className="h3">0.20%</p>
          </div>

          <div className="view-toggler-box border-top">
            <a>CHANGE ALLOCATION</a>
          </div>
        </div>
      </ContentBlock>
    );
  }
}

