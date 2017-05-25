import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { ContentBlock } from 'components';
import ModalLink from 'containers/Modal/ModalLink';

@connect(
  state => ({
    user: state.auth.user
  }),
  { })
export default class Contribution extends Component {
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
            <h2>Contribution</h2>
            <p className="h3">$250.10</p>
            <p>3% of your pay</p>
            <p>1% employer match</p>
          </div>

          <div style={{ marginBottom: '24px' }}>
            <h2>Frequency</h2>
            <p className="h3">Biweekly</p>
          </div>

          <div className="view-toggler-box border-top">
            <ModalLink action="CHANGE_CONTRIBUTION" title="CHANGE CONTRIBUTION"></ModalLink>
          </div>
        </div>
      </ContentBlock>
    );
  }
}

