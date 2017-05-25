import React, { Component } from 'react';
import { Modal, Button } from 'react-bootstrap';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import ContactForm from './ContactForm';

@connect(
  (state) => { return { showModal: state.showModal || false }; }
)

export default class ContactModal extends Component {

 static propTypes = {
    //showModal: PropTypes.bool.isRequired,
    action: PropTypes.string.isRequired,
    title: PropTypes.string.isRequired
  };

  show() {
    this.setState({ showModal: true });
  }

  close() {
    this.setState({ showModal: false });
  }

  messageSent() {
    this.close();
  }

  render() {
    const { action, title } = this.props;
    return (
      <Modal show={this.state.showModal} onHide={this.close}>
        <Modal.Header closeButton>
          <Modal.Title>{title}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <h4>{title}</h4>
          <p>This functionality is not yet available over web.</p>
          <p>Please send us the request with you optional message attached
                and we will get to you as soon as possible.</p>
          <ContactForm action={action} onSuccess={this.messageSent} />
        </Modal.Body>
        <Modal.Footer>
          <Button onClick={this.close}>Close</Button>
        </Modal.Footer>
      </Modal>
        );
  }
}
