import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import { ContentBlock } from 'components';
import { Row, Col } from 'react-bootstrap';
import { WithTooltip } from 'components/Elements';
import ModalButton from 'containers/Modal/ModalButton';
import { portfolioValue } from 'constants/staticText';
import ContentLoader, { Rect } from 'react-content-loader';


@connect(
  state => {
    const { loading, loaded, loadError } = state.account.value;
    return {
      ...state.account.value.total,
      loading,
      loaded,
      loadError
    };
   })

export default class Value extends Component {
static propTypes = {
    retirementSavings: PropTypes.string,
    returns: PropTypes.string,
    totalEarnings: PropTypes.string,
    feeSavings: PropTypes.string,
    freeTrades: PropTypes.string,
    dividents: PropTypes.string,
    loading: PropTypes.bool.isRequired,
    loaded: PropTypes.bool.isRequired,
    loadError: PropTypes.string
  }

static defaultProps = {
    retirementSavings: '',
    returns: '',
    totalEarnings: '',
    feeSavings: '',
    freeTrades: '',
    dividents: '',
    loadError: ''
  }

  render() {
    const {
      retirementSavings,
      returns,
      totalEarnings,
      feeSavings,
      freeTrades,
      dividents,
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
                  RETIREMENT SAVINGS
                  <WithTooltip id="tt1" tooltip={portfolioValue}>
                    <span className="info-icon" />
                  </WithTooltip>
                </div>
                {loaded &&
                <div className="value__header--value">
                  {retirementSavings}
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
                  TOTAL EARNINGS
                  <WithTooltip id="tt2" tooltip={portfolioValue} >
                    <span className="info-icon" />
                  </WithTooltip>
                </div>
                {loaded &&
                <div className="value__header--value">
                  {totalEarnings}
                </div>}
                {loading &&
                <div>
                  <ContentLoader height={50} speed={1}>
                     <Rect x={80} y={10} height={20} radius={5} width={200} />
                     <Rect x={80} y={40} height={10} radius={5} width={100} />
                  </ContentLoader>
                  </div>}
              </Col>
              <Col md={4} className="value__header--box" style={loaded ? {} : {'min-height': 0 }}>
                <div className="value__header--title">
                  RETURN
                  <WithTooltip id="tt3" tooltip={portfolioValue} >
                    <span className="info-icon" />
                  </WithTooltip>
                </div>
                {loaded &&
                <div className="value__header--value">
                 {returns}
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

              {!loading && !loaded && loadError &&
              <Row>
                <Col md={12} className="value__header--value">
                  <div className="value__header--error">
                    {loadError}
                  </div>
                </Col>
              </Row>}
          </div>

          <div className="pd-30">

            {loaded &&
            <div className="value__features flex-container flex-vertical-center">
              <div className="value__features--box flex-container flex-vertical-center">
                <div className="_val">{feeSavings}</div>
                <div className="_text">saving on fees</div>
              </div>
              <div className="value__features--box flex-container flex-vertical-center">
                <div className="_val">{freeTrades}</div>
                <div className="_text">free trades made</div>
              </div>
              <div className="value__features--box flex-container flex-vertical-center">
                <div className="_val">{dividents}</div>
                <div className="_text">reinvested devidends</div>
              </div>
            </div>}

            <div style={loaded ? { marginTop: '48px' } : { marginTop: '0' }}>
              <ModalButton action="ROLLOVER" title="Rollover your old RRSP"></ModalButton>
            </div>
          </div>
        </div>
      </ContentBlock>
    );
  }
}

