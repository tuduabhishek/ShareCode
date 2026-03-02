import React, { useState } from 'react';
import { Box, Typography, Paper, Grid, TextField, Button, Alert, CircularProgress } from '@mui/material';
import { useUser } from '../context/UserContext';
import axiosAPI from '../variables/axiosAPI';

const ChangeApproverPage = () => {
    const { user } = useUser();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);
    const [data, setData] = useState({ assesPno: '', newPno: '' });

    const handleExecuteChange = async () => {
        if (!data.assesPno || !data.newPno) {
            setError("Please enter both Assessee PNo and New Approver PNo");
            return;
        }
        setLoading(true);
        setError(null);
        setSuccess(null);
        try {
            await axiosAPI.post('/admin/update-approver', null, {
                params: {
                    pno: data.assesPno,
                    appPno: data.newPno,
                    year: user.fiscalYear,
                    cycle: user.cycle
                }
            });
            setSuccess("Approver updated successfully");
            setData({ assesPno: '', newPno: '' });
        } catch (err) {
            setError("Update failed. Please check the PNo codes.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <Box sx={{ py: 4 }}>
            <Typography variant="h4" gutterBottom color="primary">Change Approver</Typography>
            <Typography variant="subtitle1" gutterBottom color="textSecondary">
                Change the Reporting Manager (Approver) for a specific employee in the current cycle.
            </Typography>

            <Paper sx={{ p: 4, mt: 3, maxWidth: 600 }}>
                {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
                {success && <Alert severity="success" sx={{ mb: 2 }}>{success}</Alert>}

                <Grid container spacing={3}>
                    <Grid item xs={12}>
                        <TextField
                            label="Assessee PNo"
                            fullWidth
                            value={data.assesPno}
                            onChange={(e) => setData({ ...data, assesPno: e.target.value })}
                            disabled={loading}
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <TextField
                            label="New Approver PNo"
                            fullWidth
                            value={data.newPno}
                            onChange={(e) => setData({ ...data, newPno: e.target.value })}
                            disabled={loading}
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <Button
                            variant="contained"
                            fullWidth
                            size="large"
                            onClick={handleExecuteChange}
                            disabled={loading}
                        >
                            {loading ? <CircularProgress size={24} color="inherit" /> : "Update Approver"}
                        </Button>
                    </Grid>
                </Grid>
            </Paper>
        </Box>
    );
};

export default ChangeApproverPage;
