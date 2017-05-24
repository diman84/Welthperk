import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { ContentBlock } from 'components';
import { ForecastChart } from 'components/Charts';

@connect(
  state => ({
    user: state.auth.user
  }),
  { })
export default class Forecast extends Component {
  static propTypes = {
    user: PropTypes.object
  };

  static defaultProps = {
    user: null
  };

  render() {
    return (
      <ContentBlock>
        <div className="pd-30">
          <div style={{ marginBottom: '24px' }}>
            <h2>Future You</h2>
            <p>With the current contribution you are projected to have
              roughly <strong>$1,019,101</strong> by the age of <strong>65</strong></p>
          </div>

          <div style={{ marginBottom: '8px' }}>
            <ForecastChart data={[1, 2, 3]} />
          </div>

          <div className="account__balance flex-container flex-vertical-bottom flex-justified">
            <div>
              <div className="_val">
                $51,823.33
              </div>
              <div>Balance</div>
            </div>
            <div className="text-right">
              <a>How is this calculated?</a>
            </div>
          </div>

        </div>
      </ContentBlock>
    );
  }
}

