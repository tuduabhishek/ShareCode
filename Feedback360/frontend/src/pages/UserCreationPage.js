import React, { useState, useEffect, useCallback } from 'react';
import { Box, Typography, Paper, Grid, TextField, Button, MenuItem, Autocomplete, CircularProgress, Alert, RadioGroup, FormControlLabel, Radio, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, IconButton } from '@mui/material';
import { Edit as EditIcon, Save as SaveIcon } from '@mui/icons-material';
import { useUser } from '../context/UserContext';
import axiosAPI from '../variables/axiosAPI';

const UserCreationPage = () => {
    const { user } = useUser();
    const [mode, setMode] = useState('I'); // 'I' for Internal, 'E' for External
    const [adminUsers, setAdminUsers] = useState([]);
    const [searchQuery, setSearchQuery] = useState('');
    const [searchResults, setSearchResults] = useState([]);
    const [extUser, setExtUser] = useState({ userId: '', userName: '', email: '', status: 'A', remarks: 'ADM' });
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchAdminUsers = useCallback(async () => {
        try {
            setIsLoading(true);
            const response = await axiosAPI.get(`/admin/users?mode=${mode}`);
            setAdminUsers(response.data);
        } catch (err) {
            setError('Failed to fetch admin users');
        } finally {
            setIsLoading(false);
        }
    }, [mode]);

    useEffect(() => {
        fetchAdminUsers();
    }, [fetchAdminUsers]);

    useEffect(() => {
        const delayDebounceFn = setTimeout(() => {
            if (searchQuery.length > 2) {
                axiosAPI.get(`/admin/search-employees?prefix=${searchQuery}`).then((res) => setSearchResults(res.data));
            }
        }, 300);
        return () => clearTimeout(delayDebounceFn);
    }, [searchQuery]);

    const handleSearchSelect = async (event, value) => {
        if (!value) return;
        const perno = value.split('(')[1].replace(')', '');
        try {
            setIsLoading(true);
            await axiosAPI.post(`/admin/manage-user?action=I`, {
                userId: perno,
                userName: value.split('(')[0],
                mode: 'I',
                status: 'A',
                remarks: 'ADM'
            });
            fetchAdminUsers();
        } catch (err) {
            setError('Failed to add internal user');
        } finally {
            setIsLoading(false);
        }
    };

    const handleAddExtUser = async () => {
        try {
            setIsLoading(true);
            await axiosAPI.post(`/admin/manage-user?action=I`, { ...extUser, mode: 'E' });
            setExtUser({ userId: '', userName: '', email: '', status: 'A', remarks: 'ADM' });
            fetchAdminUsers();
        } catch (err) {
            setError('Failed to add external user');
        } finally {
            setIsLoading(false);
        }
    };

    const handleStatusUpdate = async (adminUser, newStatus) => {
        try {
            await axiosAPI.post(`/admin/manage-user?action=U`, { ...adminUser, status: newStatus });
            fetchAdminUsers();
        } catch (err) {
            setError('Failed to update user status');
        }
    };

    return (
        <Box sx={{ py: 4 }}>
            <Typography variant="h4" gutterBottom color="primary">Assign Admin Role</Typography>

            <Paper sx={{ p: 2, mb: 3 }}>
                <RadioGroup row value={mode} onChange={(e) => setMode(e.target.value)}>
                    <FormControlLabel value="I" control={<Radio />} label="Internal User" />
                    <FormControlLabel value="E" control={<Radio />} label="External User" />
                </RadioGroup>
            </Paper>

            {mode === 'I' ? (
                <Paper sx={{ p: 2, mb: 3 }}>
                    <Autocomplete
                        options={searchResults}
                        onInputChange={(e, val) => setSearchQuery(val)}
                        onChange={handleSearchSelect}
                        renderInput={(params) => <TextField {...params} label="Search Employee to Assign Role" fullWidth />}
                    />
                </Paper>
            ) : (
                <Paper sx={{ p: 2, mb: 3 }}>
                    <Grid container spacing={2} alignItems="center">
                        <Grid item xs={2}><TextField label="User ID" fullWidth value={extUser.userId} onChange={(e) => setExtUser({ ...extUser, userId: e.target.value })} /></Grid>
                        <Grid item xs={3}><TextField label="Name" fullWidth value={extUser.userName} onChange={(e) => setExtUser({ ...extUser, userName: e.target.value })} /></Grid>
                        <Grid item xs={3}><TextField label="Email" fullWidth value={extUser.email} onChange={(e) => setExtUser({ ...extUser, email: e.target.value })} /></Grid>
                        <Grid item xs={2}>
                            <TextField select label="Status" fullWidth value={extUser.status} onChange={(e) => setExtUser({ ...extUser, status: e.target.value })}>
                                <MenuItem value="A">Active</MenuItem>
                                <MenuItem value="D">De-Active</MenuItem>
                            </TextField>
                        </Grid>
                        <Grid item xs={2}><Button variant="contained" fullWidth onClick={handleAddExtUser}>Add Admin</Button></Grid>
                    </Grid>
                </Paper>
            )}

            <TableContainer component={Paper}>
                <Table>
                    <TableHead sx={{ backgroundColor: '#f5f5f5' }}>
                        <TableRow>
                            <TableCell>User ID</TableCell>
                            <TableCell>Name</TableCell>
                            <TableCell>Email</TableCell>
                            <TableCell>Department</TableCell>
                            <TableCell>Status</TableCell>
                            <TableCell>Action</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {adminUsers.map((u) => (
                            <TableRow key={u.UserId}>
                                <TableCell>{u.UserId}</TableCell>
                                <TableCell>{u.UserName}</TableCell>
                                <TableCell>{u.Email}</TableCell>
                                <TableCell>{u.Department}</TableCell>
                                <TableCell>
                                    <TextField select size="small" value={u.Status} onChange={(e) => handleStatusUpdate(u, e.target.value)}>
                                        <MenuItem value="A">Active</MenuItem>
                                        <MenuItem value="D">De-Active</MenuItem>
                                    </TextField>
                                </TableCell>
                                <TableCell>
                                    <IconButton size="small"><EditIcon /></IconButton>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>

            {error && <Alert severity="error" sx={{ mt: 2 }}>{error}</Alert>}
        </Box>
    );
};

export default UserCreationPage;
