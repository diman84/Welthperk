import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import * as contactActions from 'redux/modules/contact';
import * as notifActions from 'redux/modules/notifs';
import ContactFormInner from 'components/Elements/ContactForm';

@connect(
  null,
  { ...contactActions, ...notifActions }
)

export default class ContactForm extends Component {
  static propTypes = {
    action: PropTypes.string,
    sendContact: PropTypes.func.isRequired,
    onSuccess: PropTypes.func.isRequired,
    notifSend: PropTypes.func.isRequired,
  }

  static defaultProps = {
    action: 'GENERIC'
  };

  sendMessage = (data) => {
        const {action} = this.props;
        this.props.sendContact({ ...data, action })
        .then(() => {
                this.props.notifSend({
                    message: 'Yout request sent succesfully!',
                    kind: 'success',
                    dismissAfter: 2000
                  });
                  this.props.onSuccess();
            })
        .catch(error => {
                this.props.notifSend({
                  message: error.message,
                  kind: 'danger',
                  dismissAfter: 5000
                });
            });
  }

  renderInput = ({ input, label, type }) =>
    <div className="form-group">
      <label htmlFor={input.name} className="col-sm-2">{label}</label>
      <div className="col-sm-10">
        <input {...input} type={type} className="form-control" />
      </div>
    </div>;

  render() {
    return (
      <ContactFormInner onSubmit={this.sendMessage}>
      </ContactFormInner>
    );
  }
}
