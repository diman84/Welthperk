import React, { Component } from 'react';
import Helmet from 'react-helmet';
import { Col, Row, Grid } from 'react-bootstrap';
import { Value, Perfomance, Transactions, Risks } from 'components/Portfolio';

export default class Portfolio extends Component {

  state = {
  };

  render() {
    return (
      <Grid className="content-wrapper">
        <h1 className="text-center page-title">Hi, Dmitri, welcome to your investments!</h1>
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
