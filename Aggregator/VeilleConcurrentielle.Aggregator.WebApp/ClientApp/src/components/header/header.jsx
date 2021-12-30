import { useState } from 'react';
import {
    AppBar,
    Box,
    IconButton,
    Tab,
    Tabs,
    Toolbar,
    Typography
} from "@mui/material";
import { Menu } from "@mui/icons-material";

function LinkTab(props) {
    return (
        <Tab
            component="a"
            {...props}
        />
    );
}

const Header = () => {
    const [menuIndex, setMenuIndex] = useState(0);

    const handleMenuIndexChange = (event, newValue) => {
        setMenuIndex(newValue);
    };

    return (
        <Box sx={{ flexGrow: 1 }}>
            <AppBar position="static">
                <Toolbar variant="dense">
                    <IconButton edge="start" color="inherit" aria-label="menu" sx={{ mr: 2 }}>
                        <Menu />
                    </IconButton>
                    <Typography variant="h6" color="inherit" component="div">
                        Veille Concurrentielle
                    </Typography>
                </Toolbar>
            </AppBar>
            <Tabs value={menuIndex} onChange={handleMenuIndexChange} aria-label="Menu">
                <LinkTab label="Produits" href="/products" />
            </Tabs>
        </Box>
    )
}

export { Header };