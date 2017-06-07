import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { ContentBlock } from 'components';
import { ForecastChart } from 'components/Charts';
import ContentLoader, { Rect } from 'react-content-loader';

@connect(
  state => {
    const { loading, loaded, loadError } = state.charts;
    return {
      ...state.charts.futureYou,
      loading,
      loaded,
      loadError
    };
   })
export default class Forecast extends Component {
  static propTypes = {
    currentAmount: PropTypes.string.isRequired,
    currentAge: PropTypes.string.isRequired,
    byAmount: PropTypes.string.isRequired,
    byAge: PropTypes.string.isRequired,
    forecast: PropTypes.array.isRequired,
    forRetirement: PropTypes.bool,
    loading: PropTypes.bool.isRequired,
    loaded: PropTypes.bool.isRequired,
    loadError: PropTypes.string
  }

  static defaultProps = {
      loadError: '',
      forRetirement: true
    };

  render() {
    const { currentAmount, currentAge, byAmount, byAge, forecast,
       loading, loaded, loadError, forRetirement } = this.props;
    return (
      <ContentBlock>
        <div className="pd-30">
          <div style={{ marginBottom: '24px' }}>
            <h2>Future You</h2>
            {loaded &&
            <p>With the current contribution you are projected to have
              roughly <strong>{byAmount}</strong>
              {forRetirement &&
               <span> by the age of <strong>{byAge}</strong></span>}
               {!forRetirement &&
               <span> after <strong>{byAge}</strong></span>}
               </p>}
            {loading &&
              <ContentLoader height={50} speed={1}>
                 <Rect x={0} y={0} height={20} radius={5} width={400} />
                </ContentLoader>}
          </div>

          <div style={{ marginBottom: '8px' }}>
            {loaded && <ForecastChart data={forecast} />}
          </div>
           {loaded &&
            <div className="account__balance flex-container flex-vertical-bottom flex-justified">
              <div>
                <div>Portfolio balance at {forRetirement && <strong>{currentAge}</strong>}
                {forRetirement ? '' : currentAge}</div>
                <div className="_val">
                  {currentAmount}
                </div>
              </div>
              <div className="text-right">
                <a>How is this calculated?</a>
              </div>
            </div>}
          {!loading && !loaded && loadError &&
          <div>{loadError}</div>}
        </div>
      </ContentBlock>
    );
  }
}

