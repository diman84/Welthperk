import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { ContentBlock } from 'components';
// import { Row, Col } from 'react-bootstrap';
import PerfomanceTabs from './PerfomanceTabs';

@connect(
  state => ({
    user: state.auth.user
  }),
  { })
export default class Perfomance extends Component {
  static propTypes = {
    user: PropTypes.object
  };

  static defaultProps = {
    user: null
  };

  render() {
    return (
      <ContentBlock>
        <div>
          <div className="perfomance__header pd-30">
            <h2>Perfomance</h2>
            <p>MONEY-WEIGHTED RETURN</p>
            <div className="perfomance__value">
              + 10.9%
            </div>
          </div>
          <div className="perfomance__tabs">
            <PerfomanceTabs />
          </div>
        </div>
      </ContentBlock>
    );
  }
}

