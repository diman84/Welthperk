import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import Helmet from 'react-helmet';
import { Col, Row, Grid } from 'react-bootstrap';
import { Value, Perfomance, Transactions, Risks } from 'components/Portfolio';
import AccountSelector from 'components/Elements/AccountSelector';
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
        <AccountSelector accountName="RRSP Personal" />
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
