import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { Link } from 'react-router';

export default class AccountItem extends Component {
  static propTypes = {
    item: PropTypes.object.isRequired
  };

  render(){
    const {balance, earnings, name, earningsSign, id} = this.props.item;
    function signColorClass(sign) {
        if (sign > 0){
          return 'color-primary';
        }
        if (sign < 0){
          return 'color-danger';
        }
        return '';
    }
    return (
    <div className="account__item">
    <Link to={'/accounts/' + id}>
      <div className="account__title">
        {name}<i className="fa fa-arrow-right" />
      </div>
    </Link>
    <div className="account__balance flex-container flex-vertical-center flex-justified">
      <div>
        <div>Balance</div>
        <div className="_val">
          {balance}
        </div>
      </div>
      <div className="text-right">
        <div>Earnings</div>
        <div className={'_val ' + signColorClass(earningsSign)}>
          {earnings}
        </div>
      </div>
    </div>
  </div>);
  }
}
