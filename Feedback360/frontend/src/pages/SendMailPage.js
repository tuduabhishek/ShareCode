import React, { useState, useEffect } from 'react';
import { Box, Typography, Paper, Grid, TextField, Button, MenuItem, Alert, CircularProgress, Divider } from '@mui/material';
import { useUser } from '../context/UserContext';
import axiosAPI from '../variables/axiosAPI';

const SendMailPage = () => {
    const { user } = useUser();
    const [loading, setLoading] = useState(false);
    const [msg, setMsg] = useState({ text: '', type: 'info' });
    const [filters, setFilters] = useState({ year: user.fiscalYear, cycle: user.cycle });
    const [commType, setCommType] = useState('remtoresp');
    const [commMetric, setCommMetric] = useState(0);
    const [endDate, setEndDate] = useState('');

    const fetchCommMetric = async () => {
        try {
            const res = await axiosAPI.get(`/admin/communication-metrics?year=${filters.year}&cycle=${filters.cycle}&type=${commType}`);
            setCommMetric(res.data[0]?.Count || 0);
        } catch (err) { console.error(err); }
    };

    useEffect(() => { fetchCommMetric(); }, [commType, filters.year, filters.cycle]);

    const handleTriggerComm = async () => {
        if (!endDate) return setMsg({ text: "Please select an end date", type: 'error' });
        if (window.confirm(`Trigger reminders to ${commMetric} users?`)) {
            setLoading(true);
            try {
                await axiosAPI.post('/admin/trigger-reminders', null, {
                    params: {
                        year: filters.year, cycle: filters.cycle, type: commType, endDate, me: user.userId
                    }
                });
                setMsg({ text: "Reminders triggered successfully", type: 'success' });
            } catch (err) {
                setMsg({ text: "Failed to trigger reminders", type: 'error' });
            } finally {
                setLoading(false);
            }
        }
    };

    return (
        <Box sx={{ py: 4 }}>
            <Typography variant="h4" gutterBottom color="primary">Bulk Communications</Typography>
            <Typography variant="subtitle1" gutterBottom color="textSecondary">
                Manage and trigger bulk email communications for the 360 feedback cycle.
            </Typography>

            <Paper sx={{ p: 4, mt: 3 }}>
                {msg.text && <Alert severity={msg.type} sx={{ mb: 3 }}>{msg.text}</Alert>}

                <Grid container spacing={3} alignItems="center">
                    <Grid item xs={12} md={3}>
                        <TextField label="Year" fullWidth value={filters.year} onChange={(e) => setFilters({ ...filters, year: e.target.value })} />
                    </Grid>
                    <Grid item xs={12} md={3}>
                        <TextField label="Cycle" fullWidth value={filters.cycle} onChange={(e) => setFilters({ ...filters, cycle: e.target.value })} />
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <TextField select label="Mail Type" fullWidth value={commType} onChange={(e) => setCommType(e.target.value)}>
                            <MenuItem value="remtoresp">Reminder to Respondents (Pending Feedback)</MenuItem>
                            <MenuItem value="rtil2fin">Reminder to Assessees (Selection Pending)</MenuItem>
                            <MenuItem value="rtil1app">Reminder to Managers (Approval Pending)</MenuItem>
                        </TextField>
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <TextField
                            label="Target End Date for Cycle"
                            type="date"
                            fullWidth
                            InputLabelProps={{ shrink: true }}
                            value={endDate}
                            onChange={(e) => setEndDate(e.target.value)}
                        />
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <Paper sx={{ p: 2, bgcolor: '#f5f5f5', textAlign: 'center' }}>
                            <Typography variant="h6">Target Count: <strong>{commMetric}</strong></Typography>
                            <Button
                                variant="contained"
                                color="secondary"
                                fullWidth
                                sx={{ mt: 1 }}
                                onClick={handleTriggerComm}
                                disabled={commMetric === 0 || loading}
                            >
                                {loading ? <CircularProgress size={24} color="inherit" /> : "Trigger Bulk Mails"}
                            </Button>
                        </Paper>
                    </Grid>
                </Grid>
            </Paper>
        </Box>
    );
};

export default SendMailPage;
