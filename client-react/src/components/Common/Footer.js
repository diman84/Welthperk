import React from 'react';
import { Grid, Col, Row } from 'react-bootstrap';

const PublicFooter = () => (
  <div className="publicpage__footer">
    <Grid>
      <Row>
        <Col md={6} mdOffset={3}>
          <div className="publicpage__footer--container">
            <div className="publicpage__footer--socials text-center">
              <a
                href="https://www.linkedin.com/company/Wealthperk"
                className="_in"
                target="_blank"
                rel="noopener noreferrer" />
              <a href="https://twitter.com/Wealthperk" className="_tw" target="_blank" rel="noopener noreferrer" />
              <a href="https://www.facebook.com/Wealthperk" className="_fb" target="_blank" rel="noopener noreferrer" />
              <a href="mailto:info@wealthperk.com" className="_email" />
            </div>
            <div className="publicpage__footer--text">
              <div className="text-center">
                Investment-related content is provided for general information
                purposes by Wealfhperk and is not intended to be construed
                as investment advice.
              </div>
            </div>
            <div className="publicpage__footer--copy text-center">
              (c) 2016-2017 WEALTHPERK, INC. ALL RIGHTS RESERVED
            </div>
          </div>
        </Col>
      </Row>
    </Grid>
  </div>
);

export default PublicFooter;
