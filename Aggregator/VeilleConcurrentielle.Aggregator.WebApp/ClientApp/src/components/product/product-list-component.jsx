import {
    Paper,
    Table,
    TableContainer,
    TableHead,
    TableRow,
    TableCell,
    TableBody,
    Alert,
    TableFooter
} from "@mui/material";
import React, { useEffect, useState } from "react";
import { Loader } from "../loader/loader";
import * as api from "../../services/api";
import { store } from 'react-notifications-component';
import { defaultNotification } from "../notifications/notifications";

const ProductListComponent = () => {
    const [isLoading, setIsLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    const [products, setProducts] = useState([]);
    const [competitorColumns, setCompetitorColumns] = useState([]);

    useEffect(() => {
        async function loadProducts() {
            try {
                setIsLoading(true);
                setErrorMessage('');
                const response = await api.getProducts();
                if (response.products && response.products.length > 0) {
                    setProducts(response.products);
                    computeCompetitorColumns(response.products[0]);
                    store.addNotification({
                        ...defaultNotification,
                        type: 'success',
                        message: 'Chargement des produits terminé!',
                    });
                } else {
                    store.addNotification({
                        ...defaultNotification,
                        type: 'warning',
                        message: "Aucun produit à observer n'a été configuré pour le moment!",
                    });
                }
            } catch (ex) {
                setErrorMessage(`Erreur pendant le chargement des produits: ${ex}`);
                setProducts([]);
                setCompetitorColumns([]);
            } finally {
                setIsLoading(false);
            }
        };

        loadProducts();
    }, []);

    const computeCompetitorColumns = (product) => {
        const columNames = product.lastPricesOfAlCompetitors.map(price => price.competitorName);
        setCompetitorColumns(columNames);
    };

    return (
        <Paper>
            {isLoading && (
                <Loader />
            )}

            <div className="container">
                <div className="row">
                    <div className="col">
                        <h1>Produits à observer</h1>
                    </div>
                </div>

                {errorMessage && (
                    <div className="row">
                        <div className="col">
                            <Alert severity="error">{errorMessage}</Alert>
                        </div>
                    </div>
                )}

                <div className="row">
                    <div className="col">
                        <TableContainer>
                            <Table stickyHeader>
                                <TableHead>
                                    <TableRow>
                                        <TableCell></TableCell>
                                        <TableCell>Titre</TableCell>
                                        <TableCell>Prix</TableCell>
                                        <TableCell>Le moins cher</TableCell>
                                        <TableCell>Le plus cher</TableCell>
                                        {competitorColumns.map(competitorColumn => (
                                            <TableCell key={competitorColumn}>{competitorColumn}</TableCell>
                                        ))}
                                        <TableCell></TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {products.map(product => (
                                        <React.Fragment key={product.productId}>
                                            <TableRow hover>
                                                <TableCell></TableCell>
                                                <TableCell>{product.name}</TableCell>
                                                <TableCell>{product.price} €</TableCell>
                                                <TableCell>
                                                    {product.minPrice && (
                                                        <>
                                                            {product.minPrice} €
                                                        </>
                                                    )}
                                                </TableCell>
                                                <TableCell>
                                                    {product.maxPrice && (
                                                        <>
                                                            {product.maxPrice} €
                                                        </>
                                                    )}
                                                </TableCell>
                                                {product.lastPricesOfAlCompetitors.map(price => (
                                                    <TableCell key={`${product.productId}-${price.competitorId}`} >
                                                        {
                                                            price.price > 0 && (
                                                                <span>{price.price} €</span>
                                                            )
                                                        }
                                                        {
                                                            price.price === 0 && (
                                                                <span>-</span>
                                                            )
                                                        }
                                                    </TableCell>
                                                ))}
                                                <TableCell style={{ display: 'flex', flexDirection: 'row', gap: '0.5rem' }}>
                                                    <a href={`/products/${product.productId}`} target="_blank" rel="noopener noreferrer">Voir</a>
                                                    <a href={`/products/edit/${product.productId}`} target="_blank" rel="noopener noreferrer">Modifier</a>
                                                </TableCell>
                                            </TableRow>
                                        </React.Fragment>
                                    ))}
                                </TableBody>
                                <TableFooter>
                                    <TableRow>
                                        <TableRow>
                                            <a href='/products/new' target="_blank" rel="noopener noreferrer">Ajouter</a>
                                        </TableRow>
                                    </TableRow>
                                </TableFooter>
                            </Table>
                        </TableContainer>
                    </div>
                </div>
            </div>
        </Paper >
    )
}

export { ProductListComponent }