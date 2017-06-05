import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import Helmet from 'react-helmet';
import { Col, Row, Grid } from 'react-bootstrap';
import { Value, Perfomance, Transactions, Risks } from 'components/Accounts';
import AccountSelector from 'components/Elements/AccountSelector';
import * as authActions from 'redux/modules/auth';
import * as accountActions from 'redux/modules/account';

@connect(
  state => ({ user: state.auth.user }),
   {...authActions, ...accountActions})

export default class Account extends Component {
  static propTypes = {
    user: PropTypes.object.isRequired,
    loadValues: PropTypes.func.isRequired,
    loadSettings: PropTypes.func.isRequired,
    params: PropTypes.object.isRequired
  }

  componentWillMount() {
    if (this.props.params.id) {
      this.props.loadAccountValue(this.props.params.id);
      this.props.loadSettings(this.props.params.id);
    }
  }

  render() {
    const { user } = this.props;
    return (user &&
      <Grid className="content-wrapper">
        <AccountSelector />
        <Helmet title="Account" />

        <Row>
          <Col md={8}>
            <Value />
          </Col>
          <Col md={4}>
            <Perfomance />
          </Col>
        </Row>
        <Row>
          <Col md={8}>
            <Transactions />
          </Col>
          <Col md={4}>
            <Risks />
          </Col>
        </Row>

      </Grid>
    );
  }
}
