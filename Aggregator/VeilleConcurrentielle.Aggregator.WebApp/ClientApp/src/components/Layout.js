import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { Footer } from './footer/footer';
import { Header } from './header/header';
import { NavMenu } from './NavMenu';

export class Layout extends Component {
    static displayName = Layout.name;

    render() {
        return (
            <div>
                <Header />
                <NavMenu />
                <Container>
                    {this.props.children}
                </Container>
                <Footer />
            </div>
        );
    }
}