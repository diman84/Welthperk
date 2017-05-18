import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { Col, Row, Grid } from 'react-bootstrap';
import { Link } from 'react-router';
import Helmet from 'react-helmet';
import { connect } from 'react-redux';

@connect(
  state => ({
    online: state.online
  })
)
export default class Home extends Component {

  static propTypes = {
    online: PropTypes.bool.isRequired
  };

  render() {
    // const { online } = this.props;
    return (
      <div>
        <Helmet
          title="Group RSP benefits for modern businesses"
          link={[
            { rel: 'stylesheet', href: '/css/material-kit.css' }
          ]}
        />

        <div
          className="page-header header-filter"
          data-parallax="active"
          style={{ backgroundImage: 'url(https://wealthperk.github.io/assets/img/treehugger.jpg)' }}
        >
          <Grid>
            <Row>
              <Col md={10} mdOffset={1} className="text-center">
                <h1 className="tim-typo" style={{ color: '#ffffff' }}>Your Chief Retirement Officer</h1>
                <h4 style={{ color: '#ffffff' }}>Group RSP benefits for modern businesses</h4>
                <br />
                <br />
                <br />
                <Link to="/GetStarted" className="btn btn-success btn-lg">
                  Request Info
                  <div className="ripple-container" />
                </Link>
              </Col>
            </Row>
          </Grid>
        </div>

        <div className="main main-raised">
          <Grid>
            <div className="features-3">
              <Row>
                <Col md={4}>
                  <div className="phone-container">
                    <br />
                    <br />
                    <br />
                    <img src="https://wealthperk.github.io/assets/img/iphonewp1.png" alt="" />
                  </div>
                </Col>
                <Col md={7}>
                  <h2 className="tim-typo">
                    We make sure your employees get the most out of their Group RSP benefits.
                  </h2>
                  <h4>We empower Canadians to save for retirement in a way that keeps all of their hard-earned
                    dollars in their accounts.</h4>

                  <Col md={6}>
                    <div className="info info-horizontal">
                      <div className="icon icon-success">
                        <i className="material-icons">people</i>
                      </div>
                      <div className="description">
                        <h4>Design your portfolio</h4>
                        <p>
                          Employees who want flexibility can design their own portfolio or collaborate with an advisor.
                        </p>
                      </div>
                    </div>

                    <div className="info info-horizontal">
                      <div className="icon icon-success">
                        <i className="material-icons">donut_small</i>
                      </div>
                      <div className="description">
                        <h4>Low-cost Index Funds</h4>
                        <p>30,000+ fund options from Vanguard, Charles Schwab, BlackRock, Fidelity, etc.</p>
                      </div>
                    </div>

                    <div className="info info-horizontal">
                      <div className="icon icon-success">
                        <i className="material-icons">school</i>
                      </div>
                      <div className="description">
                        <h4 className="tim-typo">Financial Literacy Training</h4>
                        <p> We offer robust resources on retirement savings.</p>
                      </div>
                    </div>
                  </Col>
                  <Col md={6}>
                    <br />
                    <div className="card card-pricing card-raised">
                      <div className="content">
                        <h6 className="category">Employees Pay</h6>
                        <h1 className="tim-title"><small>$</small>0</h1>
                        <h4>No advisory fees</h4>
                        <ul>
                          <li><b>0.05%-0.20%</b> Fund Expense</li>
                          <li><b>$0.0035</b>/share ECN fees</li>
                          <li><b>Custodial</b> fee included</li>
                        </ul>
                      </div>
                    </div>
                  </Col>
                </Col>
              </Row>
            </div>
          </Grid>

          <Grid>
            <div className="features-3">
              <Row>
                <Col md={7}>
                  <h2 className="tim-typo">
                    We act as your extended HR team by automating all of the Group RSP administration.
                  </h2>
                  <h4>
                    We take care of all of the plan setup, ongoing administration,
                    CRA compliance, and employee support to save you time. It’s a great fit
                    for fast growing businesses of any size.
                  </h4>

                  <Col md={6}>
                    <div className="info info-horizontal">
                      <div className="icon icon-success">
                        <i className="material-icons">account_circle</i>
                      </div>
                      <div className="description">
                        <h4>Employee accounts created automatically</h4>
                      </div>
                    </div>

                    <div className="info info-horizontal">
                      <div className="icon icon-success">
                        <i className="material-icons">sync</i>
                      </div>
                      <div className="description">
                        <h4>Employee deductions synced with your payroll provider</h4>
                      </div>
                    </div>

                    <div className="info info-horizontal">
                      <div className="icon icon-success">
                        <i className="material-icons">date_range</i>
                      </div>
                      <div className="description">
                        <h4>Contributions processed automatically every pay period</h4>
                      </div>
                    </div>
                  </Col>
                  <Col md={6}>
                    <br />
                    <div className="card card-pricing card-raised">
                      <div className="content content-success">
                        <h6 className="category">Plan Sponsors Pay</h6>
                        <h1 className="tim-title" style={{ color: '#ffffff' }}><small>$</small>25<small>/mo</small></h1>
                        <h4 style={{ color: '#ffffff' }}>per participant</h4>
                        <ul>
                          <li><b>$500</b> one-time setup</li>
                          <li><b>$100</b> monthly minimum</li>
                          <li><b>0%</b> no AUM fees</li>
                        </ul>
                      </div>
                    </div>
                  </Col>
                </Col>
                <Col md={5}>
                  <div className="phone-container">
                    <br />
                    <br />
                    <img className="img" src="https://wealthperk.github.io/assets/img/iphonewp2.png" alt="" />
                  </div>
                </Col>
              </Row>
            </div>
          </Grid>
        </div>

        <div className="features text-center">
          <Grid>

            <Row>
              <Col md={8} mdOffset={2}>
                <h2 className="tim-typo">Your investments are safe here</h2>
                <h5 className="description">
                  Your accounts stay in your name at a trusted custodian,
                  protected by the best asset coverage in Canada.
                </h5>
              </Col>
            </Row>

            <Row>
              <Col md={4}>
                <div className="info">
                  <div className="icon icon-success">
                    <i className="material-icons">fingerprint</i>
                  </div>
                  <h4>Data is encrypted</h4>
                  <p>Your security and trust are mission critical at Wealthperk.
                    We use bank-level security to protect your data.
                    All our systems back-up multiple times throughout the day
                    to ensure business continuity no matter what happens.</p>
                </div>
              </Col>

              <Col md={4}>
                <div className="info">
                  <div className="icon icon-success">
                    <i className="material-icons">verified_user</i>
                  </div>
                  <h4>Money is insured</h4>
                  <p> Your accounts are insured by the Canadian Insurance Protection
                    Fund (CIPF) up to $1,000,000 against insolvency or bankruptcy.
                    Wealthperk also offers optional insurance coverage for up to $10,000,000.</p>
                </div>
              </Col>

              <Col md={4}>
                <div className="info">
                  <div className="icon icon-success">
                    <i className="material-icons">account_balance</i>
                  </div>
                  <h4>Assets held by custodian</h4>
                  <p>Your assets are held in your name by our trusted custodial broker,
                    who is regulated by IIROC. If we go out of busienss, you could keep
                    your money with custodian, or transfer it to a new advisor or your bank account. </p>
                </div>
              </Col>
            </Row>

          </Grid>
        </div>

        <div className="card card-raised card-carousel">
          <div id="carousel-example-generic" className="carousel slide" data-ride="carousel">
            <div className="carousel slide" data-ride="carousel">

              <div className="carousel-inner">
                <div className="item active">
                  <div
                    className="page-header header-filter"
                    style={{ backgroundImage: 'url(https://wealthperk.github.io/assets/img/founder.png)' }}
                  >
                    <Grid>
                      <Row>
                        <Col md={3} className="text-left">
                          <h2 className="tim-typo" style={{ color: '#ffffff' }}>Dmitri Stepanov</h2>
                          <h4 style={{ color: '#ffffff' }}>Founder &amp; CEO @ Wealthperk</h4>
                          <p>
                            We design easy to use tools that empower individuals to invest like elite institutions.
                          </p>
                          <br />
                        </Col>
                      </Row>
                    </Grid>
                  </div>
                </div>

              </div>
            </div>
          </div>
        </div>

      </div>
    );
  }
}
