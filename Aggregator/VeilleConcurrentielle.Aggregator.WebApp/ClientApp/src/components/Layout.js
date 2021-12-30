import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { Footer } from './footer/footer';
import { Header } from './header/header';
import ReactNotification from 'react-notifications-component';
import 'react-notifications-component/dist/theme.css';
import './layout.css';

export class Layout extends Component {
    static displayName = Layout.name;

    render() {
        return (
            <div>
                <ReactNotification />
                <Header />
                <div className="main-app">
                    <div className="main-content">
                        <Container>
                            {this.props.children}
                        </Container>
                    </div>
                    <div className="main-footer">
                        <Footer />
                    </div>
                </div>
            </div>
        );
    }
}
