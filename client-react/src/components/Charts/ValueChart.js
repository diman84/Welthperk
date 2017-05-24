import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { ResponsiveContainer, LineChart, Line, YAxis, XAxis, Tooltip, CartesianGrid } from 'recharts';

const data = [
  { x: 0, y: 80, saved: 2400 },
  { x: '1W', y: 110, saved: 2210 },
  { x: '2W', y: 800, saved: 2290 },
  { x: '3W', y: 480, saved: 2000 },
  { x: '2M', y: 720, saved: 2181 },
  { x: '4M', y: 560, saved: 2500 },
  { x: 'Y', y: 920, saved: 2100 },
];

export default class ValueChart extends Component {
  static propTypes = {
    data: PropTypes.array.isRequired
  };

  state = {
  };

  formatXTicks = (args) => {
    const { x, y, payload } = args;

    return (
      <g transform={`translate(${x},${y})`}>
        <text x={0} y={0} dy={16} fill="#707070" textAnchor="middle" fontSize={14} transform="translate(0,1)">
          {payload.value}
        </text>
      </g>
    );
  };

  formatYTicks = (args) => {
    const { x, y, payload } = args;

    return (
      <g transform={`translate(${x},${y})`}>
        <text x={0} y={0} dy={16} fill="#707070" textAnchor="start" fontSize={12} transform="translate(0,-1)">
          ${payload.value}
        </text>
      </g>
    );
  };

  render() {
    return (
      <ResponsiveContainer width="100%" height={228}>
        <LineChart data={data}>
          <YAxis
            type="number"
            domain={[0, 1000]}
            axisLine={false}
            tickSize={0}
            ticks={[250, 500, 750, 1000]}
            mirror
            tick={this.formatYTicks}
          />
          <XAxis
            dataKey="x"
            hide={false}
            stroke="#d7d7d7"
            strokeWidth={0.5}
            tick={this.formatXTicks}
          />
          <CartesianGrid vertical={false} strokeWidth={0.5} />
          <Tooltip />
          <Line
            dataKey="y"
            stroke="#FF9C3A"
            strokeWidth={3}
            dot={false}
            type="linear"
          />
        </LineChart>
      </ResponsiveContainer>
    );
  }
}
