import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { ContentBlock } from 'components';
import { Row, Col } from 'react-bootstrap';
import { WithTooltip } from 'components/Elements';
import { portfolioValue } from 'constants/staticText';
import { ValueChart } from 'components/Charts';

@connect(
  state => ({
    user: state.auth.user,
    current: state.account.current
  }),
  { })
export default class Value extends Component {
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
          <div className="value__header">
            <Row>
              <Col md={4} className="value__header--box">
                <div className="value__header--title">
                  PORTFOLIO VALUE
                  <WithTooltip id="tt1" tooltip={portfolioValue} >
                    <span className="info-icon" />
                  </WithTooltip>
                </div>
                <div className="value__header--value">
                  $105,912.12
                </div>
              </Col>
              <Col md={4} className="value__header--box">
                <div className="value__header--title">
                  EARNINGS
                  <WithTooltip id="tt2" tooltip={portfolioValue} >
                    <span className="info-icon" />
                  </WithTooltip>
                </div>
                <div className="value__header--value">
                  $105,912.12
                </div>
              </Col>
              <Col md={4} className="value__header--box">
                <div className="value__header--title">
                  SAVED ON FEES
                  <WithTooltip id="tt3" tooltip={portfolioValue} >
                    <span className="info-icon" />
                  </WithTooltip>
                </div>
                <div className="value__header--value">
                  $105,912.12
                </div>
              </Col>
            </Row>
          </div>

          <div className="value__content pd-30">
            <h2>Overall Chart</h2>
            <ValueChart data={[1, 2, 3]} />
          </div>
        </div>
      </ContentBlock>
    );
  }
}

