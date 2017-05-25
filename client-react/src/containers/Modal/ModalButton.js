import React,{Component} from 'react';
import {bindActionCreators} from 'redux';
import {connect} from 'react-redux';
import {toggleModal} from 'redux/modules/modal';
import { Button } from 'react-bootstrap';
import PropTypes from 'prop-types';

class ModalComponentButton extends Component{
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

function mapDispatchToProps(dispatch){
  return bindActionCreators({
    toggleModal
  }, dispatch);
}

export default connect(null, mapDispatchToProps)(ModalComponentButton);
