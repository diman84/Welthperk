import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { ContentBlock } from 'components';
import { Row, Col } from 'react-bootstrap';
import { WithTooltip } from 'components/Elements';
import { portfolioValue } from 'constants/staticText';
import { ValueChart } from 'components/Charts';
import ContentLoader, { Rect } from 'react-content-loader';

@connect(
  state => ({
    ...state.account.current
  }))
export default class Value extends Component {
  static propTypes = {
    balance: PropTypes.string,
    earnings: PropTypes.string,
    feeSavings: PropTypes.string,
    loading: PropTypes.bool.isRequired,
    loaded: PropTypes.bool.isRequired,
    loadError: PropTypes.string
  };

  static defaultProps = {
    balance: '',
    earnings: '',
    feeSavings: '',
    loadError: ''
  };

  render() {
    const {
      balance,
      earnings,
      feeSavings,
      loaded,
      loading,
      loadError
    } = this.props;
    return (
      <ContentBlock>
        <div>
          <div className="value__header">
            <Row>
              <Col md={4} className="value__header--box">
                <div className="value__header--title">
                  ACCOUNT VALUE
                  <WithTooltip id="tt1" tooltip={portfolioValue} >
                    <span className="info-icon" />
                  </WithTooltip>
                </div>
                 {loaded &&
                <div className="value__header--value">
                  {balance}
                </div>}
                {loading &&
                <div>
                  <ContentLoader height={50} speed={1}>
                     <Rect x={50} y={10} height={20} radius={5} width={200} />
                     <Rect x={50} y={40} height={10} radius={5} width={100} />
                  </ContentLoader>
                  </div>}
              </Col>
              <Col md={4} className="value__header--box">
                <div className="value__header--title">
                  EARNINGS
                  <WithTooltip id="tt2" tooltip={portfolioValue} >
                    <span className="info-icon" />
                  </WithTooltip>
                </div>
                {loaded &&
                <div className="value__header--value">
                  {earnings}
                </div>}
                {loading &&
                <div>
                  <ContentLoader height={50} speed={1}>
                     <Rect x={80} y={10} height={20} radius={5} width={200} />
                     <Rect x={80} y={40} height={10} radius={5} width={100} />
                  </ContentLoader>
                  </div>}
              </Col>
              <Col md={4} className="value__header--box">
                <div className="value__header--title">
                  SAVED ON FEES
                  <WithTooltip id="tt3" tooltip={portfolioValue} >
                    <span className="info-icon" />
                  </WithTooltip>
                </div>
                {loaded &&
                <div className="value__header--value">
                  {feeSavings}
                </div>}
                {loading &&
                <div>
                  <ContentLoader height={50} speed={1}>
                     <Rect x={100} y={10} height={20} radius={5} width={200} />
                     <Rect x={100} y={40} height={10} radius={5} width={100} />
                  </ContentLoader>
                  </div>}
              </Col>
            </Row>
          </div>
          {!loading && !loaded && loadError &&
              <Row>
                <Col md={12} className="value__header--value">
                  <div className="value__header--error">
                    {loadError}
                  </div>
                </Col>
              </Row>}
          <div className="value__content pd-30">
            <h2>Overall Chart</h2>
            <ValueChart data={[1, 2, 3]} />
          </div>
        </div>
      </ContentBlock>
    );
  }
}

