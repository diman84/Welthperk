import React,{Component} from 'react';
//import {bindActionCreators} from 'redux';
import {connect} from 'react-redux';
import * as modalActions from 'redux/modules/modal';
import { Button } from 'react-bootstrap';
import PropTypes from 'prop-types';


@connect(() => ({ }), modalActions)

export default class ModalComponentButton extends Component{
    static propTypes = {
        toggleModal: PropTypes.func.isRequired,
        title: PropTypes.string.isRequired
    };

    render(){
      const { title } = this.props;
      return (
      <Button bsSize="lg" bsStyle="primary" onClick={this.props.toggleModal}>{title}</Button>
      );
    }
}

/*function mapDispatchToProps(dispatch){
  return bindActionCreators({
    toggleModal
  }, dispatch);
}*/
