import React,{Component} from 'react';
import {connect} from 'react-redux';
import * as modalActions from 'redux/modules/modal';
import { Button } from 'react-bootstrap';
import PropTypes from 'prop-types';


@connect(() => ({ }), modalActions)

export default class ModalComponentButton extends Component{
    static propTypes = {
        toggleModal: PropTypes.func.isRequired,
        title: PropTypes.string.isRequired,
        action: PropTypes.string.isRequired
    };

    showModal = () => {
      const { title, action, toggleModal } = this.props;
      toggleModal({action, title});
    }

    render(){
      const { title } = this.props;
      return (
    <Button bsSize="lg" bsStyle="primary"
      onClick={this.showModal}>{title}</Button>
      );
    }
}
