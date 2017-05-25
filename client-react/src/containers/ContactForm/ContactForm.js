import React, { Component } from 'react';
import { reduxForm, Field, propTypes } from 'redux-form';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import { Button } from 'react-bootstrap';
import * as contactActions from 'redux/modules/contact';
import * as notifActions from 'redux/modules/notifs';

@reduxForm({
  form: 'contact'
})

@connect(
  (state) => { return { ...state }; },
  { ...contactActions, ...notifActions }
)

export default class ContactForm extends Component {
  static propTypes = {
    ...propTypes,
    title: PropTypes.string,
    action: PropTypes.string,
    sendContact: PropTypes.func.isRequired,
    onSuccess: PropTypes.func.isRequired,
    notifSend: PropTypes.func.isRequired,
  }

  static defaultProps = {
    title: 'Send request',
    action: 'GENERIC'
  };

  static textarea;

  sendMessage = data =>
    this.props.sendContact(data)
        .then(() => {
                this.textarea.value = '';
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
    const { action, error, } = this.props;

    return (
      <form className="form-horizontal" onSubmit={this.sendMessage}>
        <Field ref={node => { this.textarea = node; }}
          name="username" type="text" component={this.renderInput} label="Message" />
        <Field name="action" type="hidden" component={this.renderInput} value={action} />
        {error && <p className="text-danger"><strong>{error}</strong></p>}
        <div className="form-group">
          <Button className="btn btn-success pull-right" type="submit">
          <i className="fa" />{' '}Send
          </Button>
        </div>
      </form>
    );
  }
}
