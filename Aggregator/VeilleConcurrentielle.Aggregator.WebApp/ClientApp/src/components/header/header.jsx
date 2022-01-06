import React, { useState } from 'react';
import {
    AppBar,
    Badge,
    Box,
    IconButton,
    Tab,
    Tabs,
    Toolbar,
    Typography,
    Tooltip,
    Menu,
    MenuItem,
    CircularProgress,
    Alert,
    Divider
} from "@mui/material";
import { Notifications } from "@mui/icons-material";
import { useEffect } from 'react';
import * as api from "../../services/api";
import { store } from 'react-notifications-component';
import { defaultNotification } from '../notifications/notifications';
import { useHistory, useLocation } from 'react-router-dom';

function LinkTab(props) {
    return (
        <Tab
            component="a"
            {...props}
        />
    );
}

const refreshTimeInSeconds = 10;

const Header = () => {
    const history = useHistory();
    const location = useLocation();
    const [menuIndex, setMenuIndex] = useState(0);
    const [alertCount, setAlertCount] = useState(0);
    const [isLoading, setIsLoading] = useState(false);
    const [anchorEl, setAnchorEl] = useState(null);
    const openAlertCount = Boolean(anchorEl);
    const [alertCountByProduct, setAlertCountByProduct] = useState([]);

    const alertCountOnClick = async (event) => {
        setAnchorEl(event.currentTarget);
        setAlertCountByProduct([]);
        setIsLoading(true);
        try {
            const response = await api.getRecommendationAlertsCountByProduct();
            if (response.items) {
                setAlertCountByProduct(response.items);
            }
        } catch (error) {
            store.addNotification({
                ...defaultNotification,
                type: 'danger',
                message: `Erreur lors de la récupération des récommendations: ${error}!`,
            });
            setAlertCountByProduct([]);
        } finally {
            setIsLoading(false);
        }
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleProductAlertClick = (productId) => {
        history.push(`/products/${productId}`);
    }

    useEffect(() => {
        refreshAlertCount();
        const currentPath = location.pathname;
        if (currentPath && currentPath.includes("products")) {
            setMenuIndex(1);
        }
    });

    async function refreshAlertCount() {
        setTimeout(() => {
            api.getRecommendationAlertsCount()
                .then(response => {
                    setAlertCount(response.count);
                })
                .catch(error => {
                    store.addNotification({
                        ...defaultNotification,
                        type: 'danger',
                        message: `Erreur lors de la récupération du nombre total des récommendations: ${error}!`,
                    });
                }).finally(() => {
                    refreshAlertCount();
                })
        }, refreshTimeInSeconds * 1000);
    };

    const handleMenuIndexChange = (event, newValue) => {
        setMenuIndex(newValue);
    };

    return (
        <React.Fragment>
            <Box sx={{ flexGrow: 1 }}>
                <AppBar position="static">
                    <Toolbar variant="dense">
                        <IconButton edge="start" color="inherit" aria-label="menu" sx={{ mr: 2 }}>
                            <Menu />
                        </IconButton>
                        <Typography variant="h6" color="inherit" component="div">
                            Veille Concurrentielle
                        </Typography>
                        <Box sx={{ flexGrow: 1 }} />
                        <Tooltip title={`${alertCount} récommendations`}>
                            <IconButton
                                size="large"
                                aria-label={`Afficher les ${alertCount} récommendations`}
                                color="inherit"
                                onClick={alertCountOnClick}
                            >
                                <Badge badgeContent={alertCount} color="error">
                                    <Notifications />
                                </Badge>
                            </IconButton>
                        </Tooltip>
                    </Toolbar>
                </AppBar>
                <Tabs value={menuIndex} onChange={handleMenuIndexChange} aria-label="Menu">
                    <LinkTab label="Acceuil" href="/" />
                    <LinkTab label="Produits" href="/products" />
                </Tabs>
            </Box>
            <Menu
                anchorEl={anchorEl}
                id="alert-menu"
                open={openAlertCount}
                onClose={handleClose}
                onClick={handleClose}
                PaperProps={{
                    elevation: 0,
                    sx: {
                        overflow: 'visible',
                        filter: 'drop-shadow(0px 2px 8px rgba(0,0,0,0.32))',
                        mt: 1.5,
                        '& .MuiAvatar-root': {
                            width: 32,
                            height: 32,
                            ml: -0.5,
                            mr: 1,
                        },
                        '&:before': {
                            content: '""',
                            display: 'block',
                            position: 'absolute',
                            top: 0,
                            right: 14,
                            width: 10,
                            height: 10,
                            bgcolor: 'background.paper',
                            transform: 'translateY(-50%) rotate(45deg)',
                            zIndex: 0,
                        },
                    },
                }}
                transformOrigin={{ horizontal: 'right', vertical: 'top' }}
                anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
            >
                {isLoading && (
                    <MenuItem>
                        <CircularProgress />
                    </MenuItem>
                )}

                {alertCountByProduct.length === 0 && (
                    <Alert severity="warning">Il n'y a pas encore de récommendations pour le moment!</Alert>
                )}

                {alertCountByProduct.map((alertCountByProduct, index) => (
                    <React.Fragment key={`alert-${alertCountByProduct.productName}`}>
                        {index > 0 && (
                            <Divider />
                        )}
                        <MenuItem onClick={() => handleProductAlertClick(alertCountByProduct.productId)}>
                            <Badge badgeContent={alertCountByProduct.count} color="error">
                                {alertCountByProduct.productName}
                            </Badge>
                        </MenuItem>
                    </React.Fragment>
                ))}
            </Menu>
        </React.Fragment>
    )
}

export { Header };