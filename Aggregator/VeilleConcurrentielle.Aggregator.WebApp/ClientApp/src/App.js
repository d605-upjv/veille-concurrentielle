import React, { Component } from 'react';
import {
    Route,
    BrowserRouter as Router,
    Switch
} from 'react-router-dom';
import { Layout } from './components/layout';
import { Home } from './components/home';
import { ProductListComponent } from './components/product/product-list-component';
import { ProductItemComponent } from './components/product/product-item-component';

import './custom.css'
import { ProductEditItemComponent } from './components/product/product-edit-item-component';

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Router>
                <Layout>
                    <Switch>
                        <Route exact path='/' component={Home} />
                        <Route exact path='/products' component={ProductListComponent} />
                        <Route exact path='/products/edit/:id' component={ProductEditItemComponent} />
                        <Route exact  path='/products/new' component={ProductEditItemComponent} />
                        <Route exact path='/products/:id' component={ProductItemComponent} />
                    </Switch>
                </Layout>
            </Router>
        );
    }
}
