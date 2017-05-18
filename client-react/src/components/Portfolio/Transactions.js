import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { ContentBlock } from 'components';

@connect(
  state => ({
    user: state.auth.user
  }),
  { })
export default class Transactions extends Component {
  static propTypes = {
    user: PropTypes.object
  };

  static defaultProps = {
    user: null
  };

  state = {
    filters: false
  };

  render() {
    const { filters } = this.state;
    return (
      <ContentBlock>
        <div className="pd-30 btm-content-box">
          <div className="transactions__header">
            <h2>Transactions</h2>
            <p>Feb 12,2017</p>
            <div
              className={'filter-icon' + (filters ? ' _open' : '')}
              onClick={() => this.setState({ filters: !filters })}
            />
          </div>

          <div className={'transactions__filters' + (filters ? ' _open' : '')}>
            <div className="transactions__filters--inner">
              <span className="slim-btn active">Bought</span>
              <span className="slim-btn">Received</span>
            </div>
          </div>

          <div className="transactions__content">
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-danger">
                  VAB - Canadian Bonds
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-danger">
                  VAB - Canadian Bonds
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-danger">
                  VAB - Canadian Bonds
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
          </div>
          <div className="view-toggler-box border-top">
            <a>VIEW MORE</a>
          </div>
        </div>
      </ContentBlock>
    );
  }
}

