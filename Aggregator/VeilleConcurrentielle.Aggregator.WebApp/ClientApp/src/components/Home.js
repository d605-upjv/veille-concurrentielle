import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>Veille concurrentielle!</h1>
        <p>Surveille les prix des concurrents et propose des récommendations en conséquences.</p>
      </div>
    );
  }
}
