import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { ContentBlock } from 'components';
import { AccountItem } from 'components/Accounts';
import ModalLink from 'containers/Modal/ModalLink';
import ContentLoader, { Rect } from 'react-content-loader';

@connect(
  state => {
    const { loading, loaded, loadError, accounts } = state.account.value;
    return {
      accounts,
      loading,
      loaded,
      loadError
    };
   })

export default class AccountsList extends Component {
static propTypes = {
    accounts: PropTypes.array,
    loading: PropTypes.bool.isRequired,
    loaded: PropTypes.bool.isRequired,
    loadError: PropTypes.string
  }

  static defaultProps = {
    accounts: [],
    loadError: ''
  }

  render() {
    const {
      accounts,
      loaded,
      loading,
      loadError
    } = this.props;
    return (
      <ContentBlock>
        <div className="pd-30 btm-content-box">
          <h2>Your Accounts</h2>
          {!loading && !loaded && loadError &&
              <p className="value__header--error">{loadError}</p>}

          {loading &&
                <ContentLoader height={100} speed={1}>
                 <Rect x={0} y={0} height={30} radius={5} width={300} />
                 <Rect x={0} y={40} height={20} radius={5} width={200} />
                 <Rect x={0} y={70} height={10} radius={5} width={100} />
                </ContentLoader>}

          {loaded && accounts.map((item) => (
            <AccountItem key={item.id} item={item} />
          ))}

          <div className="view-toggler-box border-top">
            <ModalLink action="ADD_ACCOUNT" title="+ ADD ANOTHER ACCOUNT"></ModalLink>
          </div>
        </div>
      </ContentBlock>
    );
  }
}

