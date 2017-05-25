import React, { Component } from 'react';
import { reduxForm, Field } from 'redux-form';
import PropTypes from 'prop-types';

@reduxForm({
  form: 'contact'
})

export default class ContactFormInner extends Component {

 static propTypes = {
    handleSubmit: PropTypes.func.isRequired
 }

  renderInput = ({ input, type }) =>
    <div className="form-group row">
        <div className="col-sm-12">
        {type !== 'textarea' &&
         <input {...input} type={type} className="form-control" />
        }
        {type === 'textarea' &&
         <textarea {...input} type={type} className="form-control" />
        }
        </div>
    </div>;

  render() {
    const { handleSubmit } = this.props;

    return (

        <form className="form-horizontal" onSubmit={handleSubmit}>
          <fieldset>
            <Field ref={node => { this.textarea = node; }}
              name="message" type="textarea" component={this.renderInput} />
            <div className="form-group row">
              <div className="col-sm-12">
                <button className="btn btn-success pull-right" type="submit">{' '}Send
                </button>
              </div>
            </div>
          </fieldset>
        </form>

    );
  }
}
