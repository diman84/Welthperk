import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { ContentBlock } from 'components';
import { Row, Col } from 'react-bootstrap';
import Button from 'react-bootstrap/lib/Button';
import { WithTooltip } from 'components/Elements';
import { portfolioValue } from 'constants/staticText';

@connect(
  state => ({
    user: state.auth.user
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
                  RETIREMENT SAVINGS
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
                  TOTAL EARNINGS
                  <WithTooltip id="tt2" tooltip={portfolioValue} >
                    <span className="info-icon" />
                  </WithTooltip>
                </div>
                <div className="value__header--value">
                  + $5,912.12
                </div>
              </Col>
              <Col md={4} className="value__header--box">
                <div className="value__header--title">
                  RETURN
                  <WithTooltip id="tt3" tooltip={portfolioValue} >
                    <span className="info-icon" />
                  </WithTooltip>
                </div>
                <div className="value__header--value">
                  +14.1%
                </div>
              </Col>
            </Row>
          </div>

          <div className="pd-30">

            <div className="value__features flex-container flex-vertical-center">
              <div className="value__features--box flex-container flex-vertical-center">
                <div className="_val">$509</div>
                <div className="_text">saving on fees</div>
              </div>
              <div className="value__features--box flex-container flex-vertical-center">
                <div className="_val">629</div>
                <div className="_text">free trades made</div>
              </div>
              <div className="value__features--box flex-container flex-vertical-center">
                <div className="_val">$643</div>
                <div className="_text">reinvested devidends</div>
              </div>
            </div>

            <div style={{ marginTop: '48px' }}>
              <Button bsSize="lg" bsStyle="primary">Rollover your old RRSP</Button>
            </div>
          </div>
        </div>
      </ContentBlock>
    );
  }
}

