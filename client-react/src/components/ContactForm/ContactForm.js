import React, { Component } from 'react';
import { reduxForm, Field, propTypes } from 'redux-form';
import PropTypes from 'prop-types';

@reduxForm({
  form: 'contact'
})
export default class ContactForm extends Component {
  static propTypes = {
    ...propTypes,
    notifSend: PropTypes.func.isRequired,
    sendContact: PropTypes.func.isRequired,
  }

  sendMessage = data =>
    this.props.sendContact(data)
        .then(() => {
                this.props.notifSend({
                  message: 'Your request has been sent',
                  kind: 'success',
                  dismissAfter: 5000
                });
            })
        .catch(error => {
                this.props.notifSend({
                  message: error.message,
                  kind: 'danger',
                  dismissAfter: 5000
                });
            });

  renderInput = ({ input, label, type, meta: { touched, error } }) =>
    <div className={`form-group ${error && touched ? 'has-error' : ''}`}>
      <label htmlFor={input.name} className="col-sm-2">{label}</label>
      <div className="col-sm-10">
        <input {...input} type={type} className="form-control" />
        {error && touched && <span className="glyphicon glyphicon-remove form-control-feedback"></span>}
        {error && touched && <div className="text-danger"><strong>{error}</strong></div>}
      </div>
    </div>;

  render() {
    const { action, error } = this.props;

    return (
      <form className="form-horizontal" onSubmit={this.sendMessage}>
        <Field name="username" type="text" component={this.renderInput} label="Message" />
        <Field name="action" type="hidden" component={this.renderInput} value={action} />
        {error && <p className="text-danger"><strong>{error}</strong></p>}
        <button className="btn btn-success pull-right" type="submit">
          <i className="fa" />{' '}Send
        </button>
      </form>
    );
  }
}
