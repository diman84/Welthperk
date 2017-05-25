import React, { Component } from 'react';
import { Modal } from 'react-bootstrap';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import * as modalActions from 'redux/modules/modal';
import ContactForm from './ContactForm';

@connect(
  (state) => {
    return { showModal: state.modal.modalState };
  },
  modalActions
)

export default class ContactModal extends Component {

 static propTypes = {
    showModal: PropTypes.bool.isRequired,
    toggleModal: PropTypes.func.isRequired,
    action: PropTypes.string,
    title: PropTypes.string
  }

  static defaultProps = {
    action: 'GENERIC',
    title: 'Send Request'
  }

  render() {
    const { title, showModal, action } = this.props;
    return (
      <Modal show={showModal} onHide={this.props.toggleModal}>
        <Modal.Header closeButton>
          <Modal.Title>{title}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p>This functionality is not yet available over web.</p>
          <p>Please send us the request with you optional message attached
                and we will get to you as soon as possible.</p>
           <p></p>
          <ContactForm action={action} onSuccess={this.props.toggleModal} />
        </Modal.Body>
        <Modal.Footer>
        </Modal.Footer>
      </Modal>
        );
  }
}
