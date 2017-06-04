import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import Helmet from 'react-helmet';
import { Col, Row, Grid } from 'react-bootstrap';
import { Value, Contribution, AccountsList, Forecast } from 'components/Accounts';
import * as accountActions from 'redux/modules/account';
import * as authActions from 'redux/modules/auth';

@connect(
  state => ({
    user: state.auth.user
   }),
   {...authActions, ...accountActions})

export default class Accounts extends Component {

  static propTypes = {
    user: PropTypes.object.isRequired,
    loadValues: PropTypes.func.isRequired,
    loadSettings: PropTypes.func.isRequired
  }

  componentWillMount() {
    this.props.loadValues();
    this.props.loadSettings();
  }

  render() {
    const { user } = this.props;
    return (
      <Grid className="content-wrapper">
        <h1 className="page-title" style={{fontSize: '24px'}}>Good afternoon, {user.email}!</h1>
        <Helmet title="Accounts" />
        <Row>
          <Col md={8}>
            <Value />
          </Col>
          <Col md={4}>
            <AccountsList />
          </Col>
        </Row>
        <Row>
          <Col md={8}>
            <Forecast />
          </Col>
          <Col md={4}>
            <Contribution />
          </Col>
        </Row>

      </Grid>
    );
  }
}
