import React, { Component } from 'react';
import Modal from 'react-bootstrap/lib/Modal';
import Button from 'react-bootstrap/lib/Button';
import PropTypes from 'prop-types';
import ContactForm from 'components/ContactForm/ContactForm';
import { connect } from 'react-redux';

@connect(
  (state) => ({ showModal: state.showModal })
)

export default class ContactModal extends Component {

 static propTypes = {
    showModal: PropTypes.bool,
    title: PropTypes.string,
    action: PropTypes.string
  };

  static defaultProps = {
    showModal: false,
    title: 'Send request',
    action: 'GENERIC'
  };

  close() {
    this.setState({ showModal: false });
  }

  render() {
    const { action, title, showModal } = this.props;
    return (
      <Modal show={showModal} onHide={this.close}>
        <Modal.Header closeButton>
          <Modal.Title>{title}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <h4>{title}</h4>
          <p>This functionality is not yet available over web.</p>
          <p>Please send us the request with you optional message attached
                and we will get to you as soon as possible.</p>
          <ContactForm action={action} />
        </Modal.Body>
        <Modal.Footer>
          <Button onClick={this.close}>Close</Button>
        </Modal.Footer>
      </Modal>
        );
  }
}
