import { Search } from "@mui/icons-material";
import { Alert, Button, IconButton, Input, InputAdornment, InputLabel, Paper } from "@mui/material";
import { useEffect, useState } from "react";
import { Loader } from "../loader/loader";
import * as api from "../../services/api";
import { store } from "react-notifications-component";
import { defaultNotification } from "../notifications/notifications";
import './product-edit-item-component.css';
import Select from "react-select";

export const ProductEditItemComponent = (props) => {
    const [product, setProduct] = useState(null);
    const [mainShopProduct, setMainShopProduct] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    const [successMessage, setSuccessMessage] = useState('');
    const productId = props.match.params.id;
    const isEditMode = (productId !== undefined) && (productId !== '');
    const [productUrl, setProductUrl] = useState('');
    const [strategies, setStrategies] = useState([]);
    const [selectedStrategies, setSelectedStrategies] = useState({});

    const handleSubmit = async () => {
        if (!product || !mainShopProduct) {
            setErrorMessage('Veuillez renseigner un url du produit et cliquer sur le bouton "Rechercher" (loupe)');
            return;
        }
        if (selectedStrategies.length === 0) {
            setErrorMessage('Veuillez choisir au moins une stratégie de récommendations.');
            return;
        }
        setIsLoading(true);
        setErrorMessage('');
        setSuccessMessage('');
        try {
            const productToSubmit = {};
            productToSubmit.productId = productId;
            productToSubmit.productName = mainShopProduct.name;
            productToSubmit.price = mainShopProduct.price;
            productToSubmit.quantity = mainShopProduct.quantity;
            productToSubmit.isActive = true;
            productToSubmit.imageUrl = mainShopProduct.imageUrl;
            productToSubmit.shopProductId = mainShopProduct.shopProductId;
            productToSubmit.shopProductUrl = mainShopProduct.shopProductUrl;
            productToSubmit.strategies = selectedStrategies.map(s => {
                return {
                    id: s.value
                };
            });
            productToSubmit.competitorConfigs = [];
            product.competitorConfigs.forEach(c => {
                if (c.productUrl) {
                    productToSubmit.competitorConfigs.push({
                        competitorId: c.competitorId,
                        holder: {
                            items: [
                                {
                                    key: 'ProductPageUrl',
                                    value: c.productUrl
                                }
                            ]
                        }
                    });
                }
            });
            const response = await api.upsertProduct(productToSubmit);
            if (response && response.eventId) {
                setSuccessMessage(`La demande d'engistrement a été soumise avec succès avec le numéro: ${response.eventId}`);
                store.addNotification({
                    ...defaultNotification,
                    type: 'success',
                    message: `La demande d'engistrement a été soumise avec succès avec le numéro: ${response.eventId}`,
                });
            } else {
                throw new Error(`Le serveur a rejeté la demande: ${response}`);
            }
        } catch (ex) {
            setErrorMessage(`Erreur pendant l'enregistrement de la demande: ${ex}`);
            store.addNotification({
                ...defaultNotification,
                type: 'danger',
                message: `Erreur pendant l'enregistrement de la demande: ${ex}`,
            });
        }
        setIsLoading(false);
    }

    const handleSearchMainProduct = async () => {
        if (productUrl) {
            await loadMainShopProduct(productUrl);
        } else {
            store.addNotification({
                ...defaultNotification,
                type: 'danger',
                message: "L'url du produit est obligatoire!",
            });
        }
    }

    const handleProductUrlChange = (event) => {
        setProductUrl(event.target.value);
    }

    const handleStrategiesChange = (selectedStrategies_) => {
        setSelectedStrategies(selectedStrategies_);
    }

    const handleCompetitorConfigChange = (competitorId) => (event) => {
        const items = [...product.competitorConfigs];
        const index = items.findIndex(item => item.competitorId === competitorId);
        items[index].productUrl = event.target.value;
        setProduct({ ...product, competitorConfigs: items });
    }

    useEffect(() => {
        if (isEditMode) {
            loadProductToEdit(productId);
        } else {
            loadProductToAdd();
        }
    }, [productId]);

    const resetProduct = () => {
        setProduct(null);
        setMainShopProduct(null);
        setProductUrl(null);
        setStrategies([]);
        setSelectedStrategies([]);
    }

    const loadProductToEdit = async (productId) => {
        setIsLoading(true);
        setErrorMessage('');
        resetProduct();
        setSuccessMessage('');
        try {
            const response = await api.GetProductToEdit(productId);
            if (response) {
                setProduct(response);
                loadStrategies(response);
                setMainShopProduct(response.mainShopProduct);
                if (response.mainShopProduct) {
                    setProductUrl(response.mainShopProduct.shopProductUrl);
                }
            } else {
                store.addNotification({
                    ...defaultNotification,
                    type: 'danger',
                    message: "Impossible de charger le produit! Veuillez vérifier sa configuration.",
                });
                throw new Error('La configuration du produit est invalide');
            }
        } catch (ex) {
            setErrorMessage(`Erreur pendant le chargement du produit: ${ex}`);
            setProduct(null);
        }
        setIsLoading(false);
    }

    const loadProductToAdd = async () => {
        setIsLoading(true);
        setErrorMessage('');
        resetProduct();
        setSuccessMessage('');
        try {
            const response = await api.GetProductToAdd();
            if (response) {
                setProduct(response);
                loadStrategies(response);
            } else {
                store.addNotification({
                    ...defaultNotification,
                    type: 'danger',
                    message: "Impossible de charger certaines données requises par cette page!",
                });
                throw new Error('Impossible de charger certaines données requises par cette page');
            }
        } catch (ex) {
            setErrorMessage(`Erreur pendant le chargement: ${ex}`);
            setProduct(null);
        }
        setIsLoading(false);
    }

    const loadStrategies = (product_) => {
        const allItems = product_.allStrategies.map(strategy => {
            return {
                value: strategy.strategyId,
                label: strategy.strategyName
            };
        });
        setStrategies(allItems);
        const selectedItems = product_.selectedStrategies.map(strategy => {
            return {
                value: strategy.strategyId,
                label: strategy.strategyName
            };
        });
        setSelectedStrategies(selectedItems);
    }

    const loadMainShopProduct = async (productUrl_) => {
        setIsLoading(true);
        setErrorMessage('');
        setMainShopProduct(null);
        try {
            const response = await api.getMainShopProduct(productUrl_);
            if (response && response.product) {
                setMainShopProduct(response.product);
                store.addNotification({
                    ...defaultNotification,
                    type: 'success',
                    message: 'Produit chargé avec succès!',
                });
            } else {
                throw new Error('Url non valide');
            }
        } catch (ex) {
            setErrorMessage(`Erreur pendant le chargement du produit: ${ex}`);
            setMainShopProduct(null);
        }
        setIsLoading(false);
    }

    return (
        <Paper>
            {isLoading && (
                <Loader />
            )}

            <div className="top-container">
                {errorMessage && (
                    <div className="row">
                        <div className="col">
                            <Alert severity="error">{errorMessage}</Alert>
                        </div>
                    </div>
                )}

                {successMessage && (
                    <div className="row">
                        <div className="col">
                            <Alert severity="success">{successMessage}</Alert>
                        </div>
                    </div>
                )}

                <div className="row">
                    <div className="col-8">
                        <h3>
                            {(isEditMode && mainShopProduct) && (
                                <span>{mainShopProduct.name}</span>
                            )}
                            {(isEditMode && !mainShopProduct) && (
                                <span>Veuillez renseigner l'url du produit!</span>
                            )}
                            {!isEditMode && (
                                <span>Ajouter un nouveau produit</span>
                            )}
                        </h3>
                    </div>
                    <div className="col-3">
                        {mainShopProduct && (
                            <img src={mainShopProduct.imageUrl} className="product-image" alt={mainShopProduct.name} />
                        )}
                    </div>
                    <div className="col-1">
                        {isEditMode && (
                            <div className="col-1">
                                <a href={`/products/${productId}`}>Voir</a>
                            </div>
                        )}
                    </div>
                </div>

                <div className="row">
                    <div className="col-4">
                        <InputLabel htmlFor="productUrl">Url du produit</InputLabel>
                    </div>
                    <div className="col-8">
                        <Input id="productUrl"
                            fullWidth
                            value={productUrl}
                            onChange={handleProductUrlChange}
                            placeholder="https://main-shop.d605-shops.fr/..."
                            required
                            endAdornment={
                                <InputAdornment position="end">
                                    <IconButton
                                        aria-label="Charger le produit"
                                        edge="end"
                                        onClick={handleSearchMainProduct}
                                    >
                                        <Search />
                                    </IconButton>
                                </InputAdornment>
                            }
                        />
                    </div>
                </div>

                {mainShopProduct && (
                    <>
                        <div className="row">
                            <div className="col-4">
                                <InputLabel htmlFor="name">Nom</InputLabel>
                            </div>
                            <div className="col-8">
                                <Input id="name"
                                    fullWidth
                                    value={mainShopProduct.name}
                                    readOnly
                                />
                            </div>
                        </div>

                        <div className="row">
                            <div className="col-4">
                                <InputLabel htmlFor="name">Id (à la boutique principale)</InputLabel>
                            </div>
                            <div className="col-8">
                                <Input id="productId"
                                    fullWidth
                                    value={mainShopProduct.shopProductId}
                                    readOnly
                                />
                            </div>
                        </div>

                        <div className="row">
                            <div className="col-4">
                                <InputLabel htmlFor="price">Prix</InputLabel>
                            </div>
                            <div className="col-8">
                                <Input id="price"
                                    fullWidth
                                    value={`${mainShopProduct.price} €`}
                                    readOnly
                                />
                            </div>
                        </div>

                        <div className="row">
                            <div className="col-4">
                                <InputLabel htmlFor="quantity">Quantité</InputLabel>
                            </div>
                            <div className="col-8">
                                <Input id="quantity"
                                    fullWidth
                                    value={mainShopProduct.quantity}
                                    readOnly
                                />
                            </div>
                        </div>

                        {product && (
                            <>
                                <div className="row">
                                    <div className="col-4">
                                        <InputLabel for="strategies">Stratégies de récommendations</InputLabel>
                                    </div>
                                    <div className="col-8">
                                        <Select id="strategies"
                                            options={strategies}
                                            value={selectedStrategies}
                                            onChange={handleStrategiesChange}
                                            isMulti={true}
                                            placeholder='Choisir les stratégies à appliquer'
                                        />
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col-4">
                                        <InputLabel>Concurrents</InputLabel>
                                    </div>
                                    <div className="col-8">
                                        <div className="container">
                                            {product.competitorConfigs.map((competitorConfig, index) => (
                                                <div className="row" key={`competitor-key-${competitorConfig.competitorId}`}>
                                                    <div className="col-1">
                                                        <img className="competitor-logo-url" src={competitorConfig.logoUrl} alt={competitorConfig.competitorName} />
                                                    </div>
                                                    <div className="col-2">
                                                        <InputLabel for={`competitor-${index}`}>{competitorConfig.competitorName}</InputLabel>
                                                    </div>
                                                    <div className="col-8">
                                                        <Input id={`competitor-${index}`}
                                                            fullWidth
                                                            value={competitorConfig.productUrl}
                                                            onChange={handleCompetitorConfigChange(competitorConfig.competitorId)}
                                                            placeholder={`Url du produit dans la boutique ${competitorConfig.competitorName}`}
                                                        />
                                                    </div>
                                                </div>
                                            ))}
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <Button
                                            variant="contained"
                                            onClick={handleSubmit}
                                            className="float-end"
                                        >
                                            Enregistrer
                                        </Button>
                                    </div>
                                </div>
                            </>
                        )}

                    </>
                )}
            </div>
        </Paper>
    )
}