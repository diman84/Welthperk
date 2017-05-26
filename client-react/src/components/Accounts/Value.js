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
  state => ({
    ...state.account.value
   }))

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
              </Col>
            </Row>

            {loading &&
              <Row>
                <Col md={12} className="value__header--value">
                  <ContentLoader height={50} speed={1} primaryColor={'#63B014'}
                    style={{width: '100%'}} secondaryColor={'#6bbd17'}>
                    <Rect x={10} y={0} height={50} width={380} />
                  </ContentLoader>
                </Col>
              </Row>}

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

