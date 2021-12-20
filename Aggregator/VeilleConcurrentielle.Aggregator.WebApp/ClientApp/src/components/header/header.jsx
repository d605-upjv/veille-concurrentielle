import * as React from "react";
import {
    AppBar,
    Box,
    IconButton,
    Toolbar,
    Typography
} from "@mui/material";
import { Menu } from "@mui/icons-material";

const Header = () => {
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
        </Box>
    )
}

export { Header };