import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import Helmet from 'react-helmet';
import { Col, Row, Grid } from 'react-bootstrap';
import { Value, Contribution, AccountsList, Forecast } from 'components/Portfolio';
import * as accountActions from 'redux/modules/account';
import * as chartActions from 'redux/modules/charts';
import * as authActions from 'redux/modules/auth';

@connect(
  state => ({
    user: state.auth.user
   }),
   {...authActions, ...accountActions, ...chartActions})

export default class Portfolio extends Component {

  static propTypes = {
    user: PropTypes.object.isRequired,
    loadValues: PropTypes.func.isRequired,
    loadSettings: PropTypes.func.isRequired,
    loadForecast: PropTypes.func.isRequired
  }

  componentWillMount() {
    this.props.loadValues();
    this.props.loadSettings();
    this.props.loadForecast();
  }

  render() {
    const { user } = this.props;
    return (
      <Grid className="content-wrapper">
        <h1 className="page-title" style={{fontSize: '24px'}}>Good afternoon, {user.email}!</h1>
        <Helmet title="Portfolio" />
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
