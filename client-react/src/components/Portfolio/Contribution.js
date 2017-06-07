import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { ContentBlock } from 'components';
import ModalLink from 'containers/Modal/ModalLink';
import ContentLoader, { Rect } from 'react-content-loader';

@connect(
  state => {
    const { loading, loaded, loadError } = state.account.settings;
    return {
      ...state.account.settings.contribution,
      loading,
      loaded,
      loadError
    };
   })

export default class Contribution extends Component {
  static propTypes = {
    contribution: PropTypes.string,
    description: PropTypes.string,
    frequency: PropTypes.string,
    loading: PropTypes.bool.isRequired,
    loaded: PropTypes.bool.isRequired,
    loadError: PropTypes.string
  }

  static defaultProps = {
    contribution: '',
    description: '',
    frequency: '',
    loadError: ''
  }
  render() {
  const {
      contribution,
      description,
      frequency,
      loaded,
      loading,
      loadError } = this.props;

    return (
      <ContentBlock>
        <div className="pd-30 btm-content-box">
          <div style={{ marginBottom: '48px' }}>
            <h2>Contribution</h2>
             {loaded &&
            <div>
              <p className="h3">{contribution}</p>
              <div dangerouslySetInnerHTML={{__html: description}}></div>
            </div>}
             {loading &&
                <ContentLoader height={50} speed={1}>
                 <Rect x={0} y={0} height={20} radius={5} width={200} />
                 <Rect x={0} y={30} height={10} radius={5} width={100} />
                </ContentLoader>}
            {!loading && !loaded && loadError &&
              <p className="value__header--error">{loadError}</p>}
          </div>

          <div style={{ marginBottom: '24px' }}>
            <h2>Frequency</h2>
           {loaded &&
            <p className="h3">{frequency}</p>}

            {loading &&
                <ContentLoader height={50} speed={1}>
                 <Rect x={0} y={0} height={20} radius={5} width={200} />
                 <Rect x={0} y={30} height={10} radius={5} width={100} />
                </ContentLoader>}
          </div>

          <div className="view-toggler-box border-top">
            <ModalLink action="CHANGE_CONTRIBUTION" title="CHANGE CONTRIBUTION"></ModalLink>
          </div>
        </div>
      </ContentBlock>
    );
  }
}

