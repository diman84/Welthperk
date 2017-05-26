import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import Helmet from 'react-helmet';
import { Col, Row, Grid } from 'react-bootstrap';
import { Value, Risks, Contribution, AccountsList, Forecast } from 'components/Accounts';
import * as accountActions from 'redux/modules/account';

@connect(
  state => ({
    user: state.auth.user
   }),
   accountActions)

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
        <h1 className="text-center page-title">Good afternoon, {user.email}!</h1>
        <Helmet title="Accounts" />
        <Row>
          <Col md={8}>
            <Value />
            <Row>
              <Col md={6}>
                <Contribution />
              </Col>
              <Col md={6}>
                <Risks />
              </Col>
              <Col md={12}>
                <Forecast />
              </Col>
            </Row>
          </Col>
          <Col md={4}>
            <AccountsList />
          </Col>
        </Row>

      </Grid>
    );
  }
}
