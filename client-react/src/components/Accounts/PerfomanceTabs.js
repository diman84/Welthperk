import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Tabs from 'react-bootstrap/lib/Tabs';
import Tab from 'react-bootstrap/lib/Tab';

@connect(
  state => ({
    user: state.auth.user
  }),
  { })
export default class PerfomanceTabs extends Component {
  static propTypes = {
    user: PropTypes.object
  };

  static defaultProps = {
    user: null
  };

  state = {
    key: 1
  };

  handleSelect = (key) => {
    this.setState({ key });
  };

  render() {
    return (
      <Tabs activeKey={this.state.key} justified onSelect={this.handleSelect} id="perfomanceTabs">
        <Tab eventKey={1} title="1 DAY">
          <div className="perfomance__tabs--content">
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-success">
                  VCN - Canadian All Cap Index
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-warning">
                  VCN - Canadian All Cap Index
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-info">
                  VCN - Canadian All Cap Index
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-danger">
                  VCN - Canadian All Cap Index
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-purple">
                  VCN - Canadian All Cap Index
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-success">
                  VCN - Canadian All Cap Index
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
          </div>
        </Tab>
        <Tab eventKey={2} title="1 MONTH">
          <div className="perfomance__tabs--content">
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-success">
                  VXC - Global Stocks
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-warning">
                  VXC - Global Stocks
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-info">
                  VXC - Global Stocks
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-danger">
                  VXC - Global Stocks
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-purple">
                  VXC - Global Stocks
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-success">
                  VXC - Global Stocks
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
          </div>
        </Tab>
        <Tab eventKey={3} title="1 YEAR">
          <div className="perfomance__tabs--content">
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-success">
                  VAB - Canadian Bonds
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-warning">
                  VAB - Canadian Bonds
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-info">
                  VAB - Canadian Bonds
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-danger">
                  VAB - Canadian Bonds
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-purple">
                  VAB - Canadian Bonds
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-success">
                  VAB - Canadian Bonds
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
          </div>
        </Tab>
        <Tab eventKey={4} title="MAX">
          <div className="perfomance__tabs--content">
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-success">
                  VCN - Canadian All Cap Index
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-warning">
                  VCN - Canadian All Cap Index
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-info">
                  VCN - Canadian All Cap Index
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-danger">
                  VCN - Canadian All Cap Index
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-purple">
                  VCN - Canadian All Cap Index
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
            <div className="data-flex-row">
              <div>
                <span className="marked-span bg-before-success">
                  VCN - Canadian All Cap Index
                </span>
              </div>
              <div>
                + 10.23%
              </div>
            </div>
          </div>
        </Tab>
      </Tabs>
    );
  }
}

