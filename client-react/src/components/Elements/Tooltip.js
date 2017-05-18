import React, { Component } from 'react';
import PropTypes from 'prop-types';
import OverlayTrigger from 'react-bootstrap/lib/OverlayTrigger';
import Tooltip from 'react-bootstrap/lib/Tooltip';

export default class WithTooltip extends Component {
  static propTypes = {
    children: PropTypes.node.isRequired,
    id: PropTypes.string.isRequired,
    tooltip: PropTypes.string.isRequired
  };

  render() {
    const tooltip = <Tooltip id={this.props.id}>{this.props.tooltip}</Tooltip>;
    return (
      <OverlayTrigger
        overlay={tooltip} placement="bottom"
        delayShow={300} delayHide={150}
      >
        {this.props.children}
      </OverlayTrigger>
    );
  }
}
