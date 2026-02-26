import React from 'react';
import { Box, AppBar, Toolbar, Typography, Drawer, List, ListItem, ListItemText, ListItemIcon, Container, CssBaseline, CircularProgress, Alert } from '@mui/material';
import { useUser } from '../context/UserContext';
import PersonIcon from '@mui/icons-material/Person';
import AssessmentIcon from '@mui/icons-material/Assessment';
import SettingsIcon from '@mui/icons-material/Settings';
import PeopleIcon from '@mui/icons-material/People';
import BarChartIcon from '@mui/icons-material/BarChart';
import PlaylistAddCheckIcon from '@mui/icons-material/PlaylistAddCheck';
import { useNavigate, useLocation } from 'react-router-dom';

const drawerWidth = 240;

const MainLayout = ({ children }) => {
    const { user, isLoading, error } = useUser();
    const navigate = useNavigate();
    const location = useLocation();

    if (isLoading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
                <CircularProgress />
            </Box>
        );
    }

    const menuItems = [
        { text: 'My Surveys', icon: <AssessmentIcon />, path: '/feedback' },
        { text: 'Manage Respondents', icon: <PeopleIcon />, path: '/respondents' },
        { text: 'Survey Approval', icon: <PlaylistAddCheckIcon />, path: '/approval' },
        { text: 'Reporting', icon: <BarChartIcon />, path: '/reporting' },
    ];

    if (user.isSuperAdmin) {
        menuItems.push({ text: 'Employee Mgmt', icon: <PersonIcon />, path: '/add-employee' });
        menuItems.push({ text: 'Change BUHR', icon: <SettingsIcon />, path: '/change-buhr' });
        menuItems.push({ text: 'Change Approver', icon: <SettingsIcon />, path: '/change-approver' });
        menuItems.push({ text: 'Master Data', icon: <SettingsIcon />, path: '/master-data' });
        menuItems.push({ text: 'Bulk Update', icon: <SettingsIcon />, path: '/bulk-update' });
        menuItems.push({ text: 'Send Mail', icon: <SettingsIcon />, path: '/send-mail' });
        menuItems.push({ text: 'Super Admin', icon: <SettingsIcon />, path: '/super-admin' });
    }

    return (
        <Box sx={{ display: 'flex' }}>
            <CssBaseline />
            <AppBar position="fixed" sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}>
                <Toolbar>
                    <Typography variant="h6" noWrap component="div">
                        Feedback360 - {user.userName} ({user.userId})
                    </Typography>
                </Toolbar>
            </AppBar>
            <Drawer
                variant="permanent"
                sx={{
                    width: drawerWidth,
                    flexShrink: 0,
                    [`& .MuiDrawer-paper`]: { width: drawerWidth, boxSizing: 'border-box' },
                }}
            >
                <Toolbar />
                <Box sx={{ overflow: 'auto' }}>
                    <List>
                        {menuItems.map((item) => (
                            <ListItem
                                button
                                key={item.text}
                                onClick={() => navigate(item.path)}
                                selected={location.pathname === item.path}
                                sx={{
                                    '&.Mui-selected': { bgcolor: 'rgba(25, 118, 210, 0.08)' },
                                    '&.Mui-selected:hover': { bgcolor: 'rgba(25, 118, 210, 0.12)' }
                                }}
                            >
                                <ListItemIcon sx={{ color: location.pathname === item.path ? 'primary.main' : 'inherit' }}>
                                    {item.icon}
                                </ListItemIcon>
                                <ListItemText
                                    primary={item.text}
                                    primaryTypographyProps={{
                                        color: location.pathname === item.path ? 'primary.main' : 'inherit',
                                        fontWeight: location.pathname === item.path ? 'bold' : 'normal'
                                    }}
                                />
                            </ListItem>
                        ))}
                    </List>
                </Box>
            </Drawer>
            <Box component="main" sx={{ flexGrow: 1, p: 3 }}>
                <Toolbar />
                {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
                <Container maxWidth="lg">
                    {children}
                </Container>
            </Box>
        </Box>
    );
};

export default MainLayout;
