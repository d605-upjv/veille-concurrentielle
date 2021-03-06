import { Alert, AlertTitle, Card, CardContent, Grid, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Button } from "@mui/material"
import { useEffect, useState } from "react";
import { Loader } from "../loader/loader";
import * as api from "../../services/api";
import { store } from "react-notifications-component";
import { defaultNotification } from "../notifications/notifications";
import './product-item-component.css';
import { getDatetimeToDisplay } from "../../services/utils";
import { ShoppingBasket } from "@mui/icons-material";

const waitTimeBeforeSettingRecoToSeenInSeconds = 5;

const ProductItemComponent = (props) => {
    const [isLoading, setIsLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    const [recommendationChangeMessage, setRecommendationChangeMessage] = useState('');
    const [product, setProduct] = useState(null);
    const productId = props.match.params.id;

    useEffect(() => {
        async function loadProduct() {
            try {
                setIsLoading(true);
                setErrorMessage('');
                const response = await api.getProduct(productId);
                if (response.product) {
                    setProduct(response.product);
                    store.addNotification({
                        ...defaultNotification,
                        type: 'success',
                        message: 'Produit chargé avec succès!',
                    });
                    updateRecommendationToSeenForCurrentProduct();
                }
            } catch (ex) {
                setErrorMessage(`Erreur pendant le chargement du produit: ${ex}`);
                setProduct(null);
            } finally {
                setIsLoading(false);
            }
        }
        loadProduct();
    }, [productId]);

    function updateRecommendationToSeenForCurrentProduct() {
        setTimeout(() => {
            async function setRecommendationAlertsForProductToSync() {
                if (productId) {
                    try {
                        await api.setRecommendationAlertsForProductToSeen(productId);
                    } catch (ex) {
                        store.addNotification({
                            ...defaultNotification,
                            type: 'danger',
                            message: `Erreur pendant la mise à jour des notifications de recommandations: ${ex}`,
                        });
                    }
                }
            }
            setRecommendationAlertsForProductToSync();
        }, waitTimeBeforeSettingRecoToSeenInSeconds * 1000);
    }

    const handleUpdateProductPrice = async (price) => {
        setErrorMessage('');
        setRecommendationChangeMessage('');
        setIsLoading(true);
        try {
            const response = await api.updateMainShopPrice(product.shopProductId, price);
            if (response) {
                store.addNotification({
                    ...defaultNotification,
                    type: 'success',
                    message: 'Le nouveau prix a été soumis à la boutique principale avec succès.',
                });
                setRecommendationChangeMessage('Veuillez mettre à jour le produit à surveiller pour prendre en compte les derniers prix appliqués à la boutique principale!');
            } else {
                throw new Error('La demande a été rejetée par la boutique principale!');
            }
        } catch (ex) {
            store.addNotification({
                ...defaultNotification,
                type: 'danger',
                message: `Erreur pendant la mise à jour du prix auprès de la boutique principale: ${ex}!`,
            });
            setErrorMessage(`Erreur pendant la mise à jour du prix auprès de la boutique principale: ${ex}!`);
        }
        setIsLoading(false);
    }

    return (
        <Paper>
            {isLoading && (
                <Loader />
            )}

            <div className="top-container">

                {!product && (
                    <Alert severity="warning">
                        <AlertTitle>Attention - Erreur 404</AlertTitle>
                        <strong>La page demandée n'existe pas!</strong>
                    </Alert>
                )}

                {errorMessage && (
                    <div className="row">
                        <div className="col">
                            <Alert severity="error">{errorMessage}</Alert>
                        </div>
                    </div>
                )}

                {recommendationChangeMessage && (
                    <div className="row">
                        <div className="col">
                            <Alert severity="warning">
                                <p>
                                    {recommendationChangeMessage}
                                </p>
                                <p>
                                    <a href={`/products/edit/${productId}`}>Mettre à jour</a>
                                </p>
                            </Alert>
                        </div>
                    </div>
                )}

                {product && (
                    <>
                        <div className="row">
                            <div className="col-8">
                                <h3>{product.name}</h3>
                            </div>
                            <div className="col-3">
                                <img src={product.imageUrl} className="product-image" alt={product.name} />
                            </div>
                            <div className="col-1">
                                <a href={`/products/edit/${product.productId}`}>Modifier</a>
                            </div>
                        </div>

                        <Grid container spacing={2}>
                            <Grid item xs={12}>
                                <h5>Infos</h5>
                                <div className="container">
                                    <div className="row">
                                        <div className="col-4 item-info-name">
                                            Nom du produit:
                                        </div>
                                        <div className="col-8 item-info-value">
                                            {product.name}
                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col-4 item-info-name">
                                            Dernière mise à jour:
                                        </div>
                                        <div className="col-8 item-info-value">
                                            {getDatetimeToDisplay(product.updatedAt)}
                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col-4 item-info-name">
                                            Id à la boutique
                                        </div>
                                        <div className="col-8 item-info-value">
                                            {product.shopProductId}
                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col-4 item-info-name">
                                            Lien vers la boutique
                                        </div>
                                        <div className="col-8 item-info-value">
                                            <a href={product.shopProductUrl} target="_blank" rel="noopener noreferrer">{product.shopProductUrl}</a>
                                        </div>
                                    </div>
                                </div>
                            </Grid>

                            <Grid item xs={12}>
                                <h5>Prix</h5>
                                <Grid container>
                                    <Grid item xs={4}>
                                        {product.minPrice && (
                                            <Card>
                                                <CardContent>
                                                    <Stack direction="column">
                                                        <div className="price-title">Le concurrent <strong>le moins cher</strong></div>
                                                        <div className="price-value min">
                                                            {product.minPrice} €
                                                        </div>
                                                        <Grid container>
                                                            <Grid item xs={6}>
                                                                <div className="price-competitor">
                                                                    Vendu par <span className="sold-by-competitor-name">{product.minPriceCompetitorName}</span><br />
                                                                    <img src={product.minPriceCompetitorLogoUrl} alt={product.minPriceCompetitorName} />
                                                                </div>
                                                            </Grid>
                                                            <Grid item xs={6}>
                                                                <div className="price-quantity">
                                                                    Quantité disponible: {product.minPriceQuantity}
                                                                </div>
                                                            </Grid>
                                                        </Grid>
                                                    </Stack>
                                                </CardContent>
                                            </Card>
                                        )}
                                    </Grid>

                                    <Grid item xs={4}>
                                        <Card>
                                            <CardContent>
                                                <Stack direction="column">
                                                    <div className="price-title">Prix actuel</div>
                                                    <div className="price-value">
                                                        {product.price} €
                                                    </div>
                                                    <Grid container>
                                                        <Grid item xs={6}>
                                                            <div className="price-my-shop-logo">
                                                                <ShoppingBasket />
                                                            </div>
                                                        </Grid>
                                                        <Grid item xs={6}>
                                                            <div className="price-quantity">
                                                                Quantité disponible: {product.quantity}
                                                            </div>
                                                        </Grid>
                                                    </Grid>
                                                </Stack>
                                            </CardContent>
                                        </Card>
                                    </Grid>

                                    <Grid item xs={4}>
                                        {product.maxPrice && (
                                            <Card>
                                                <CardContent>
                                                    <Stack direction="column">
                                                        <div className="price-title">Le concurrent <strong>le plus cher</strong></div>
                                                        <div className="price-value max">
                                                            {product.maxPrice} €
                                                        </div>
                                                        <Grid container>
                                                            <Grid item xs={6}>
                                                                <div className="price-competitor">
                                                                    Vendu par <span className="sold-by-competitor-name">{product.maxPriceCompetitorName}</span><br />
                                                                    <img src={product.maxPriceCompetitorLogoUrl} alt={product.maxPriceCompetitorName} />
                                                                </div>
                                                            </Grid>
                                                            <Grid item xs={6}>
                                                                <div className="price-quantity">
                                                                    Quantité disponible: {product.maxPriceQuantity}
                                                                </div>
                                                            </Grid>
                                                        </Grid>
                                                    </Stack>
                                                </CardContent>
                                            </Card>
                                        )}

                                    </Grid>
                                </Grid>
                            </Grid>

                            <Grid item xs={12}>
                                <h5>Recommandations</h5>
                                {product.recommendations.length > 0 && (
                                    <TableContainer>
                                        <Table stickyHeader>
                                            <TableHead>
                                                <TableRow>
                                                    <TableCell>Stratégie</TableCell>
                                                    <TableCell>Prix actuel</TableCell>
                                                    <TableCell>Prix récommendé</TableCell>
                                                    <TableCell>Date</TableCell>
                                                    <TableCell></TableCell>
                                                </TableRow>
                                            </TableHead>
                                            <TableBody>
                                                {product.recommendations.map(recommendation => (
                                                    <TableRow key={`reco-${recommendation.strategyId}`} hover>
                                                        <TableCell>
                                                            <span className="recommendation-strategy">{recommendation.strategyName}</span>
                                                        </TableCell>
                                                        <TableCell>{recommendation.currentPrice} €</TableCell>
                                                        <TableCell>
                                                            <span className="recommendation-price">{recommendation.price} €</span>
                                                        </TableCell>
                                                        <TableCell>{getDatetimeToDisplay(recommendation.createdAt)}</TableCell>
                                                        <TableCell>
                                                            <Button
                                                                variant="contained"
                                                                onClick={async () => { await handleUpdateProductPrice(recommendation.price); }}
                                                            >
                                                                Appliquer
                                                            </Button>
                                                        </TableCell>
                                                    </TableRow>
                                                ))}
                                            </TableBody>
                                        </Table>
                                    </TableContainer>
                                )}

                                {product.recommendations.length === 0 && (
                                    <Alert severity="info">Il n'y a pas encore de recommandations à proposer pour le moment.</Alert>
                                )}
                            </Grid>

                            <Grid item xs={12}>
                                <h5>Historique des prix des concurrents</h5>
                                {product.lastPrices.length > 0 && (
                                    <TableContainer>
                                        <Table stickyHeader>
                                            <TableHead>
                                                <TableRow>
                                                    <TableCell colSpan={2}>Boutique</TableCell>
                                                    <TableCell>Prix</TableCell>
                                                    <TableCell>Quantité</TableCell>
                                                    <TableCell>Date</TableCell>
                                                </TableRow>
                                            </TableHead>
                                            <TableBody>
                                                {product.lastPrices.map((lastPrice, index) => (
                                                    <TableRow key={`lastPrice-${index}`} hover>
                                                        <TableCell>
                                                            <img src={lastPrice.competitorLogUrl} alt={lastPrice.competitorName} className="last-price-logo" />
                                                        </TableCell>
                                                        <TableCell>{lastPrice.competitorName}</TableCell>
                                                        <TableCell>
                                                            {lastPrice.price === product.minPrice && (
                                                                <span className="last-price-min">{lastPrice.price} €</span>
                                                            )}
                                                            {lastPrice.price === product.maxPrice && lastPrice.price !== product.minPrice && (
                                                                <span className="last-price-max">{lastPrice.price} €</span>
                                                            )}
                                                            {lastPrice.price !== product.minPrice && lastPrice.price !== product.maxPrice && (
                                                                <span>{lastPrice.price} €</span>
                                                            )}
                                                        </TableCell>
                                                        <TableCell>{lastPrice.quantity}</TableCell>
                                                        <TableCell>{getDatetimeToDisplay(lastPrice.createdAt)}</TableCell>
                                                    </TableRow>
                                                ))}
                                            </TableBody>
                                        </Table>
                                    </TableContainer>
                                )}

                                {product.lastPrices.length === 0 && (
                                    <Alert severity="info">Il n'y a pas encore d'historique de prix des concurrents à afficher.</Alert>
                                )}
                            </Grid>
                        </Grid>
                    </>
                )}

            </div>

        </Paper>
    )
}

export { ProductItemComponent }