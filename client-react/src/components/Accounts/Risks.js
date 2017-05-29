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
      ...state.account.settings.riskProfile,
      loading,
      loaded,
      loadError
    };
   })

export default class Risks extends Component {
  static propTypes = {
    profileName: PropTypes.string,
    description: PropTypes.string,
    fee: PropTypes.string,
    loading: PropTypes.bool.isRequired,
    loaded: PropTypes.bool.isRequired,
    loadError: PropTypes.string
  }

static defaultProps = {
    profileName: '',
    description: '',
    fee: '',
    loadError: ''
  }
  render() {
    const {
      profileName,
      description,
      fee,
      loaded,
      loading,
      loadError } = this.props;

    return (
      <ContentBlock>
        <div className="pd-30 btm-content-box">
          <div style={{ marginBottom: '48px' }}>
            <h2>Risk Profile</h2>
            {loaded &&
            <div>
              <p className="h3">{profileName}</p>
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
            <h2>Portfolio Fee</h2>
            {loaded &&
            <p className="h3">{fee}</p>}

            {loading &&
                <ContentLoader height={50} speed={1}>
                 <Rect x={0} y={0} height={20} radius={5} width={200} />
                 <Rect x={0} y={30} height={10} radius={5} width={100} />
                </ContentLoader>}
          </div>

          <div className="view-toggler-box border-top">
            <ModalLink action="CHANGE_ALLOCATION" title="CHANGE ALLOCATION"></ModalLink>
          </div>
        </div>
      </ContentBlock>
    );
  }
}

