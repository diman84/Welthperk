import React, { Component } from 'react';
import PropTypes from 'prop-types';

export default class ContentBlock extends Component {
  static propTypes = {
    children: PropTypes.node.isRequired,
    blockClass: PropTypes.string
  };

  static defaultProps = {
    blockClass: ''
  };

  render() {
    const blockClass = this.props.blockClass;
    return (
      <div className={'content-block ' + blockClass}>
        {this.props.children}
      </div>
    );
  }
}
