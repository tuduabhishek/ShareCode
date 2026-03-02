import React, { useState, useEffect, useCallback, useRef } from 'react';
import { Box, Typography, Paper, Grid, Button, CircularProgress, Alert, Divider } from '@mui/material';
import { TabulatorFull as Tabulator } from 'tabulator-tables';
import 'tabulator-tables/dist/css/tabulator_semanticui.min.css';
import { useUser } from '../context/UserContext';
import axiosAPI from '../variables/axiosAPI';

const SuperAdminPage = () => {
    const { user } = useUser();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const adminRef = useRef(null);

    const loadAdmins = useCallback(async () => {
        setLoading(true);
        try {
            const res = await axiosAPI.get('/admin/super-admins');
            new Tabulator(adminRef.current, {
                data: res.data,
                layout: "fitColumns",
                columns: [
                    { title: "User ID (PNo)", field: "UserId" },
                    { title: "Group ID", field: "GroupId" },
                    {
                        title: "Action",
                        formatter: "buttonCross",
                        cellClick: (e, cell) => handleRemoveAdmin(cell.getData()),
                        headerSort: false,
                        width: 80
                    }
                ]
            });
        } catch (err) { setError("Failed to load super admins"); }
        finally { setLoading(false); }
    }, []);

    useEffect(() => { loadAdmins(); }, [loadAdmins]);

    const handleAddAdmin = async () => {
        const uid = prompt("Enter User ID (PNo) for new Super Admin:");
        if (uid) {
            try {
                await axiosAPI.post('/admin/add-super-admin', null, { params: { userId: uid, me: user.userId } });
                loadAdmins();
            } catch (err) { alert("Failed to add admin"); }
        }
    };

    const handleRemoveAdmin = async (adm) => {
        if (window.confirm("Remove super admin access for " + adm.UserId + "?")) {
            try {
                await axiosAPI.delete('/admin/remove-super-admin', { params: { userId: adm.UserId } });
                loadAdmins();
            } catch (err) { alert("Failed to remove admin"); }
        }
    };

    return (
        <Box sx={{ py: 4 }}>
            <Typography variant="h4" gutterBottom color="primary">Super Admin Roles</Typography>
            <Typography variant="subtitle1" gutterBottom color="textSecondary">
                Manage individuals with administrative privileges across the Feedback360 platform.
            </Typography>

            <Paper sx={{ p: 3, mt: 3 }}>
                {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
                <Box sx={{ mb: 2, display: 'flex', justifyContent: 'flex-end' }}>
                    <Button variant="contained" onClick={handleAddAdmin}>Add Super Admin</Button>
                </Box>
                <div ref={adminRef} style={{ height: '500px' }}></div>
            </Paper>

            {loading && <Box sx={{ display: 'flex', justifyContent: 'center', p: 3 }}><CircularProgress /></Box>}
        </Box>
    );
};

export default SuperAdminPage;
