import React from 'react';
// import PropTypes from 'prop-types';

const AccountItem = () => (

  <div className="account__item">
    <div className="account__title">
      RRSP Employer<i className="fa fa-arrow-right" />
    </div>
    <div className="account__balance flex-container flex-vertical-center flex-justified">
      <div>
        <div>Balance</div>
        <div className="_val">
          $15,203.51
        </div>
      </div>
      <div className="text-right">
        <div>Earnings</div>
        <div className="_val color-primary">
          $1,203.51
        </div>
      </div>
    </div>
    <div className="account__status">
      <span className="marked-span bg-before-inactive color-gray-light">
        Auto-deposit inactive
      </span>
    </div>
  </div>
);

// AccountItem.propTypes = {
// };

export default AccountItem;
