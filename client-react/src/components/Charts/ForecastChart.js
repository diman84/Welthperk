import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { ResponsiveContainer, AreaChart, Area, YAxis, XAxis, Tooltip, ReferenceLine } from 'recharts';
/*const data = [
  { x: 1, label: 'Today', y: 180, z: 180 },
  { x: 2, label: '', y: 240, z: 240 },
  { x: 3, label: '48 years old', y: 360, z: 360 },
  { x: 4, label: '65 years old', z: 1000 }
];
*/
export default class ForecastChart extends Component {
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

  valueFormatter = (arg) => {
    return arg.toLocaleString ? arg.toLocaleString() : arg;
  }

  render() {
    const {data} = this.props;
    const labels = data.map((d) => (d.label));
    return (
      <ResponsiveContainer width="100%" height={228}>
        <AreaChart data={data}>
          <YAxis
            type="number"
            hide
            domain={[0, 1000]}
            axisLine={false}
            tickSize={0}
            mirror
            tick={this.formatYTicks}
          />
          <XAxis
            dataKey="label"
            hide={false}
            stroke="#d7d7d7"
            strokeWidth={0}
            ticks={labels}
            tick={this.formatXTicks}
          />
          <Tooltip formatter={this.valueFormatter} />
          <Area
            dataKey="y"
            stroke="#FF9C3A"
            strokeWidth={0}
            fill="#FF9C3A"
            fillOpacity={1}
            dot={false}
            type="monotone"
          />
          <Area
            dataKey="z"
            stroke="#FF9C3A"
            strokeWidth={0}
            fill="#FF9C3A"
            fillOpacity={0.6}
            dot={false}
            type="monotone"
          />
          <ReferenceLine x={labels[1]} stroke="#FF9C3A" />
        </AreaChart>
      </ResponsiveContainer>
    );
  }
}
