import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { ContentBlock } from 'components';
import { Row, Col, Button, Modal } from 'react-bootstrap';
import { WithTooltip } from 'components/Elements';
//import ContactModal from 'containers/ContactForm/ContactModal';
import ModalButton from 'containers/Modal/ModalButton';
import { portfolioValue } from 'constants/staticText';
import { bindActionCreators } from 'redux';
import { toggleModal } from 'redux/modules/modal';

@connect(
    state => {
      return {
        modalStatus: state ? state.modalState : false
      };
    },
    dispatch => {
        return bindActionCreators({
        toggleModal
      }, dispatch);
    })

export default class Value extends Component {

  static propTypes = {
    modalStatus: PropTypes.bool,
    toggleModal: PropTypes.func.isRequired
  };

  static defaultProps = {
    modalStatus: false
  }

  constructor(props) {
    super(props);
    this.closeModal = this.closeModal.bind(this);
  }

  closeModal() {
    this.props.toggleModal();
  }

  /*static defaultProps = {
    showModal: false
  };*/

  render() {
    const { modalStatus } = this.props;
    //const modal = <ContactModal action="ROLLOVER" title="Rollover your old RRSP"></ContactModal>;
    return (
      <ContentBlock>
         <Modal show={modalStatus} onHide={this.closeModal}>
        <Modal.Header closeButton>
          <Modal.Title>Rollover your old RRSP</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <h4>Rollover your old RRSP</h4>
          <p>This functionality is not yet available over web.</p>
          <p>Please send us the request with you optional message attached
                and we will get to you as soon as possible.</p>
        </Modal.Body>
        <Modal.Footer>
          <Button onClick={this.closeModal}>Close</Button>
        </Modal.Footer>
      </Modal>
        <div>
          <div className="value__header">
            <Row>
              <Col md={4} className="value__header--box">
                <div className="value__header--title">
                  RETIREMENT SAVINGS
                  <WithTooltip id="tt1" tooltip={portfolioValue}>
                    <span className="info-icon" />
                  </WithTooltip>
                </div>
                <div className="value__header--value">
                  $105,912.12
                </div>
              </Col>
              <Col md={4} className="value__header--box">
                <div className="value__header--title">
                  TOTAL EARNINGS
                  <WithTooltip id="tt2" tooltip={portfolioValue} >
                    <span className="info-icon" />
                  </WithTooltip>
                </div>
                <div className="value__header--value">
                  + $5,912.12
                </div>
              </Col>
              <Col md={4} className="value__header--box">
                <div className="value__header--title">
                  RETURN
                  <WithTooltip id="tt3" tooltip={portfolioValue} >
                    <span className="info-icon" />
                  </WithTooltip>
                </div>
                <div className="value__header--value">
                  +14.1%
                </div>
              </Col>
            </Row>
          </div>

          <div className="pd-30">

            <div className="value__features flex-container flex-vertical-center">
              <div className="value__features--box flex-container flex-vertical-center">
                <div className="_val">$509</div>
                <div className="_text">saving on fees</div>
              </div>
              <div className="value__features--box flex-container flex-vertical-center">
                <div className="_val">629</div>
                <div className="_text">free trades made</div>
              </div>
              <div className="value__features--box flex-container flex-vertical-center">
                <div className="_val">$643</div>
                <div className="_text">reinvested devidends</div>
              </div>
            </div>

            <div style={{ marginTop: '48px' }}>
              <ModalButton title="Rollover your old RRSP"></ModalButton>
            </div>
          </div>
        </div>
      </ContentBlock>
    );
  }
}

