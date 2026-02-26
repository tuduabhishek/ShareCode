import React, { useState } from 'react';
import { Box, Paper, TextField, Button, Typography, Container, Alert } from '@mui/material';
import { useUser } from '../context/UserContext';
import { useNavigate } from 'react-router-dom';

const LoginPage = () => {
    const [userId, setUserId] = useState('');
    const { login, error } = useUser();
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        if (userId) {
            const success = await login(userId);
            if (success) {
                navigate('/feedback'); // Redirect to feedback as default or landing page
            }
        }
    };

    return (
        <Container maxWidth="xs">
            <Box sx={{ mt: 8, display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
                <Paper elevation={3} sx={{ p: 4, width: '100%' }}>
                    <Typography component="h1" variant="h5" align="center" gutterBottom>
                        Feedback 360 Login
                    </Typography>
                    <Box component="form" onSubmit={handleLogin} sx={{ mt: 1 }}>
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            id="userId"
                            label="User ID / PNo"
                            name="userId"
                            autoComplete="username"
                            autoFocus
                            value={userId}
                            onChange={(e) => setUserId(e.target.value)}
                        />
                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            sx={{ mt: 3, mb: 2 }}
                        >
                            Log In
                        </Button>
                        {error && <Alert severity="error">{error}</Alert>}
                    </Box>
                </Paper>
            </Box>
        </Container>
    );
};

export default LoginPage;
