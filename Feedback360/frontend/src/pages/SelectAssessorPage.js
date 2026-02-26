import React, { useState, useEffect, useCallback } from 'react';
import { Box, Typography, Paper, Grid, Button, TextField, MenuItem, Select, FormControl, InputLabel, Card, CardContent, Divider, Alert, IconButton, Tooltip, Autocomplete } from '@mui/material';
import { Delete, AutoFixHigh, CheckCircle } from '@mui/icons-material';
import { useUser } from '../context/UserContext';
import axiosAPI from '../variables/axiosAPI';
import { ReactTabulator } from 'react-tabulator';

const SelectAssessorPage = () => {
    const { user } = useUser();
    const [respondents, setRespondents] = useState([]);
    const [rules, setRules] = useState([]);
    const [loading, setLoading] = useState(false);
    const [searchResults, setSearchResults] = useState([]);
    const [selectedEmp, setSelectedEmp] = useState(null);
    const [category, setCategory] = useState('');
    const [isSubmitted, setIsSubmitted] = useState(false);
    const [isApproved, setIsApproved] = useState(false);

    const refreshData = useCallback(async () => {
        try {
            setLoading(true);
            const [listRes, rulesRes] = await Promise.all([
                axiosAPI.get('/respondent/list', { params: { assesPno: user.userId, fy: user.fiscalYear, cycle: user.cycle } }),
                axiosAPI.get('/respondent/rules', { params: { level: user.level || 'I4' } })
            ]);
            setRespondents(listRes.data);
            setRules(rulesRes.data);
            if (listRes.data.length > 0) {
                setIsSubmitted(listRes.data.some(r => r.Status === 'SU' || r.AppStatus === 'AP'));
                setIsApproved(listRes.data.every(r => r.AppStatus === 'AP'));
            }
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    }, [user]);

    useEffect(() => { if (user.userId) refreshData(); }, [user, refreshData]);

    const handleAutoPopulate = async () => {
        try {
            setLoading(true);
            const res = await axiosAPI.get('/respondent/auto-populated', {
                params: { assesPno: user.userId, fy: user.fiscalYear, cycle: user.cycle, level: user.level }
            });
            // Loop and add each auto-populated respondent
            for (const r of res.data) {
                await axiosAPI.post('/respondent/add', {
                    AssesPno: user.userId,
                    AssesLevel: user.level,
                    Perno: r.Perno,
                    Name: r.Name,
                    Designation: r.Designation,
                    Department: r.Department,
                    Email: r.Email,
                    Category: r.Category,
                    Type: 'ORG',
                    AdminId: user.userId,
                    FiscalYear: user.fiscalYear,
                    Cycle: user.cycle
                });
            }
            refreshData();
        } catch (err) {
            alert("Auto-population failed");
        } finally {
            setLoading(false);
        }
    };

    const handleSearch = async (val) => {
        if (val.length > 2) {
            const res = await axiosAPI.get('/employee/search', { params: { query: val } });
            setSearchResults(res.data);
        }
    };

    const addAssessor = async () => {
        if (!selectedEmp || !category) return;
        try {
            await axiosAPI.post('/respondent/add', {
                AssesPno: user.userId,
                AssesLevel: user.level,
                Perno: selectedEmp.Perno,
                Name: selectedEmp.Name,
                Designation: selectedEmp.Designation,
                Department: selectedEmp.Department,
                Email: selectedEmp.Email,
                Category: category,
                Type: 'ORG',
                AdminId: user.userId,
                FiscalYear: user.fiscalYear,
                Cycle: user.cycle
            });
            setSelectedEmp(null);
            setCategory('');
            refreshData();
        } catch (err) {
            alert(err.response?.data?.Message || "Failed to add");
        }
    };

    const removeAssessor = async (row) => {
        try {
            await axiosAPI.delete('/respondent/remove', {
                params: {
                    assesPno: user.userId,
                    respPno: row.Perno,
                    category: row.Category,
                    fy: user.fiscalYear,
                    cycle: user.cycle
                }
            });
            refreshData();
        } catch (err) {
            alert("Failed to remove");
        }
    };

    const submitList = async () => {
        // Simple validation check against rules
        for (const rule of rules) {
            const count = respondents.filter(r => r.Category === rule.Category).length;
            if (count < rule.MinCount) {
                alert(`Minimum ${rule.MinCount} ${rule.CategoryName} required.`);
                return;
            }
        }
        try {
            await axiosAPI.post('/respondent/submit', null, {
                params: { assesPno: user.userId, fy: user.fiscalYear, cycle: user.cycle, me: user.userId }
            });
            alert("List submitted for approval");
            refreshData();
        } catch (err) {
            alert("Submission failed");
        }
    };

    const columns = [
        { title: "Category", field: "Category", width: 150 },
        { title: "Perno", field: "Perno", width: 100 },
        { title: "Name", field: "Name", width: 250 },
        { title: "Department", field: "Department", width: 200 },
        {
            title: "Actions",
            formatter: () => isSubmitted ? "" : '<button style="color: red; cursor: pointer; border: none; background: none;">Delete</button>',
            cellClick: (e, cell) => !isSubmitted && removeAssessor(cell.getData())
        }
    ];

    return (
        <Box sx={{ p: 3 }}>
            <Typography variant="h4">Select Your Assessors</Typography>
            <Typography variant="subtitle1" color="textSecondary" sx={{ mb: 2 }}>
                Please select your managers, peers, and subordinates for the 360 feedback cycle.
            </Typography>

            {isApproved ? (
                <Alert severity="success" sx={{ mb: 3 }} icon={<CheckCircle />}>
                    Your assessor list has been <strong>Approved</strong>. Feedback collection is in progress.
                </Alert>
            ) : isSubmitted ? (
                <Alert severity="warning" sx={{ mb: 3 }}>
                    Your assessor list has been <strong>Submitted</strong> and is awaiting manager approval.
                </Alert>
            ) : (
                <Alert severity="info" sx={{ mb: 3 }} action={
                    <Button color="inherit" size="small" startIcon={<AutoFixHigh />} onClick={handleAutoPopulate}>
                        Auto-Populate
                    </Button>
                }>
                    Click "Auto-Populate" to automatically add your reporting managers, peers, and subordinates.
                </Alert>
            )}

            <Grid container spacing={3}>
                <Grid item xs={12} md={4}>
                    <Card variant="outlined" sx={{ opacity: isSubmitted ? 0.6 : 1, pointerEvents: isSubmitted ? 'none' : 'auto' }}>
                        <CardContent>
                            <Typography variant="h6">Add Assessor Manually</Typography>
                            <Autocomplete
                                options={searchResults}
                                getOptionLabel={(option) => `${option.Name} (${option.Perno})`}
                                onInputChange={(e, val) => handleSearch(val)}
                                onChange={(e, val) => setSelectedEmp(val)}
                                renderInput={(params) => <TextField {...params} label="Search Employee" size="small" margin="normal" />}
                                disabled={isSubmitted}
                            />
                            <FormControl fullWidth size="small" margin="normal" disabled={isSubmitted}>
                                <InputLabel>Category</InputLabel>
                                <Select value={category} onChange={(e) => setCategory(e.target.value)}>
                                    {rules.map(r => <MenuItem key={r.Category} value={r.Category}>{r.CategoryName}</MenuItem>)}
                                </Select>
                            </FormControl>
                            <Button variant="contained" fullWidth onClick={addAssessor} sx={{ mt: 2 }} disabled={isSubmitted}>Add Assessor</Button>
                        </CardContent>
                    </Card>

                    <Paper sx={{ p: 2, mt: 3 }}>
                        <Typography variant="h6" gutterBottom>Validation Rules</Typography>
                        {rules.map(rule => (
                            <Box key={rule.Category} sx={{ mb: 1, display: 'flex', justifyContent: 'space-between' }}>
                                <Typography variant="body2">{rule.CategoryName}:</Typography>
                                <Typography variant="body2" fontWeight="bold">Min {rule.MinCount} / Max {rule.MaxCount}</Typography>
                            </Box>
                        ))}
                    </Paper>
                </Grid>

                <Grid item xs={12} md={8}>
                    <Paper sx={{ p: 2 }}>
                        <Typography variant="h6" gutterBottom>Your Assessor List</Typography>
                        <ReactTabulator data={respondents} columns={columns} layout="fitColumns" />
                        <Box sx={{ mt: 3, textAlign: 'right' }}>
                            <Button variant="contained" color="success" size="large" startIcon={<CheckCircle />} onClick={submitList} disabled={isSubmitted}>
                                {isSubmitted ? "Already Submitted" : "Submit for Approval"}
                            </Button>
                        </Box>
                    </Paper>
                </Grid>
            </Grid>
        </Box>
    );
};

export default SelectAssessorPage;
