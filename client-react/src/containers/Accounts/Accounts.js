import React, { Component } from 'react';
import Helmet from 'react-helmet';
import { Col, Row, Grid } from 'react-bootstrap';
import { Value, Risks, Contribution, AccountsList, Forecast } from 'components/Accounts';

export default class Accounts extends Component {

  state = {
  };

  render() {
    return (
      <Grid className="content-wrapper">
        <h1 className="text-center page-title">Good afternoon, Dmitri!</h1>
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
