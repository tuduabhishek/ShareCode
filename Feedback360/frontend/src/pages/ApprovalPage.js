import React, { useState, useEffect, useCallback } from 'react';
import { Box, Typography, Paper, Grid, Button, Divider, Alert } from '@mui/material';
import { useUser } from '../context/UserContext';
import axiosAPI from '../variables/axiosAPI';
import { ReactTabulator } from 'react-tabulator';

const ApprovalPage = () => {
    const { user } = useUser();
    const [pendingLists, setPendingLists] = useState([]);
    const [selectedAssessee, setSelectedAssessee] = useState(null);
    const [respondents, setRespondents] = useState([]);

    const fetchPending = useCallback(async () => {
        try {
            const res = await axiosAPI.get('/respondent/assessees', {
                params: {
                    adminId: user.userId,
                    isSuperAdmin: user.isSuperAdmin,
                    fy: user.fiscalYear,
                    cycle: user.cycle
                }
            });
            // Filter only submitted but not approved
            setPendingLists(res.data.filter(a => a.SubmitStatus === 'SU' && !a.AppStatus));
        } catch (err) {
            console.error(err);
        }
    }, [user]);

    useEffect(() => { if (user.userId) fetchPending(); }, [user, fetchPending]);

    const viewDetails = async (row) => {
        setSelectedAssessee(row);
        const res = await axiosAPI.get('/respondent/list', {
            params: { assesPno: row.Perno, fy: user.fiscalYear, cycle: user.cycle }
        });
        setRespondents(res.data);
    };

    const handleAction = async (action) => {
        if (!selectedAssessee) return;
        try {
            await axiosAPI.post(`/respondent/${action}`, null, {
                params: { assesPno: selectedAssessee.Perno, fy: user.fiscalYear, cycle: user.cycle, me: user.userId }
            });
            alert(`List ${action}ed`);
            setSelectedAssessee(null);
            fetchPending();
        } catch (err) {
            alert("Action failed");
        }
    };

    const mainColumns = [
        { title: "Perno", field: "Perno", width: 100 },
        { title: "Name", field: "Name", width: 250 },
        { title: "Level", field: "Level", width: 80 },
        {
            title: "Action",
            formatter: () => '<button style="background: #1976d2; color: white; border: none; padding: 4px 8px; border-radius: 4px; cursor: pointer;">Review</button>',
            cellClick: (e, cell) => viewDetails(cell.getData())
        }
    ];

    const detailColumns = [
        { title: "Category", field: "Category", width: 150 },
        { title: "Perno", field: "Perno", width: 100 },
        { title: "Name", field: "Name", width: 200 },
        { title: "Department", field: "Department", width: 200 }
    ];

    return (
        <Box sx={{ p: 3 }}>
            <Typography variant="h4">Respondent List Approval</Typography>
            <Divider sx={{ my: 2 }} />

            <Grid container spacing={3}>
                <Grid item xs={12} md={5}>
                    <Typography variant="h6">Pending Approvals</Typography>
                    <Paper sx={{ p: 1, mt: 1 }}>
                        <ReactTabulator data={pendingLists} columns={mainColumns} layout="fitColumns" />
                    </Paper>
                </Grid>

                <Grid item xs={12} md={7}>
                    {selectedAssessee ? (
                        <Box>
                            <Typography variant="h6">Details: {selectedAssessee.Name}</Typography>
                            <Paper sx={{ p: 1, mt: 1, mb: 2 }}>
                                <ReactTabulator data={respondents} columns={detailColumns} layout="fitColumns" />
                            </Paper>
                            <Box sx={{ display: 'flex', gap: 2 }}>
                                <Button variant="contained" color="success" onClick={() => handleAction('approve')}>Approve</Button>
                                <Button variant="contained" color="error" onClick={() => handleAction('reject')}>Return to Assessee</Button>
                            </Box>
                        </Box>
                    ) : (
                        <Alert severity="info">Select an assessee to review their respondent list.</Alert>
                    )}
                </Grid>
            </Grid>
        </Box>
    );
};

export default ApprovalPage;
