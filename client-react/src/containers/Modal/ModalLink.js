import React,{Component} from 'react';
import {connect} from 'react-redux';
import * as modalActions from 'redux/modules/modal';
import PropTypes from 'prop-types';


@connect(() => ({ }), modalActions)

export default class ModalComponentButton extends Component{
    static propTypes = {
        toggleModal: PropTypes.func.isRequired,
        title: PropTypes.string.isRequired,
        action: PropTypes.string.isRequired
    };

    showModal = (event) => {
      const { title, action, toggleModal } = this.props;
      event.preventDefault();
      toggleModal({action, title});
    }

    render(){
      const { title } = this.props;
      return (
    <a onClick={this.showModal}>{title}</a>
      );
    }
}
