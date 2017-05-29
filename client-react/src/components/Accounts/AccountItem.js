import React, { Component } from 'react';
import PropTypes from 'prop-types';

export default class AccountItem extends Component {
  static propTypes = {
    item: PropTypes.object.isRequired
  }

  render(){
    const {balance, earnings, name, autodeposit} = this.props.item;
    return (
    <div className="account__item">
    <div className="account__title">
      {name}<i className="fa fa-arrow-right" />
    </div>
    <div className="account__balance flex-container flex-vertical-center flex-justified">
      <div>
        <div>Balance</div>
        <div className="_val">
          {balance}
        </div>
      </div>
      <div className="text-right">
        <div>Earnings</div>
        <div className="_val color-primary">
          {earnings}
        </div>
      </div>
    </div>
    <div className="account__status">
      {!autodeposit &&
        <span className="marked-span bg-before-inactive color-gray-light">
          Auto-deposit inactive
        </span>}
        {autodeposit &&
        <span className="marked-span bg-before-active color-secondary">
          Auto-deposit active
        </span>}
    </div>
  </div>);
  }
}
