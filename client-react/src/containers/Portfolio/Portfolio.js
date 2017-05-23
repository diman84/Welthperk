import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import Helmet from 'react-helmet';
import { Col, Row, Grid } from 'react-bootstrap';
import { Value, Perfomance, Transactions, Risks } from 'components/Portfolio';
import * as authActions from 'redux/modules/auth';

@connect(
  state => ({ user: state.auth.user }),
  authActions)

export default class Portfolio extends Component {
  static propTypes = {
    user: PropTypes.object.isRequired
  }
  render() {
    const { user } = this.props;
    return (user &&
      <Grid className="content-wrapper">
        <h1 className="text-center page-title">Hi, {user.email}, welcome to your investments!</h1>
        <Helmet title="Portfolio" />

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
